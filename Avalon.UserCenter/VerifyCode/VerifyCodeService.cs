using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Avalon.UserCenter.Models;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    public class VerifyCodeService : IService
    {
        /// <summary>
        /// 验证码默认生成的字符串
        /// </summary>
        const string strCode = "1234567890";

        /// <summary>
        ///用户验证码重复发送限制键格式
        /// </summary>
        private const string UserVeriyCodeRepeateSendLimitFmtKey = "user:vc:RepeateSend_u{0}_t{1}";

        static Random rnd = new Random(Environment.TickCount);

        private readonly IKeyValueRepository _keyValueRepository;

        private readonly IPicVerifyIpStatRepository _picVerifyIpStatRepository;

        private readonly IMobileVerifyCodeStatRepository _mobileVerifyCodeLogRepository;
        private readonly IUserVeriyCodeRepeateSendStatRepository _userVeriyCodeRepeateSendStatRepository;
        private readonly AppBaseService _appBaseService;
        private readonly IEmailVerifyRepository _emailVerifyRepository;
        private readonly IMobileVerifyRepository _mobileVerifyRepository;
        private readonly IUserRepository _userRepository;

        public VerifyCodeService(IKeyValueRepository keyValueRepository
            , IPicVerifyIpStatRepository userOptLogRepository
            , IMobileVerifyCodeStatRepository mobileVerifyCodeLogRepository
            , IUserVeriyCodeRepeateSendStatRepository userVeriyCodeRepeateSendStatRepository
            , AppBaseService appBaseService
            , IEmailVerifyRepository emailVerifyRepository
            , IMobileVerifyRepository mobileVerifyRepository
            , IUserRepository userRepository)
        {
            _keyValueRepository = keyValueRepository;
            _picVerifyIpStatRepository = userOptLogRepository;
            _mobileVerifyCodeLogRepository = mobileVerifyCodeLogRepository;
            _userVeriyCodeRepeateSendStatRepository = userVeriyCodeRepeateSendStatRepository;
            _appBaseService = appBaseService;
            _emailVerifyRepository = emailVerifyRepository;
            _mobileVerifyRepository = mobileVerifyRepository;
            _userRepository = userRepository;
        }


        /// <summary>
        /// 图片验证码判断逻辑
        /// 1、	用户在提交注册业务时，对于注册成功的操作记录，根据规则（N秒内成功的记录）生成失效时间戳，连同创建时间、用户IP地址存放在userOptLog表中。
        /// 在业务系统注册时非强制使用图片验证码，如果当前IP在N秒内注册成功了M次（判断userOptLog表中创建时间在N秒内的注册记录），
        /// 则也需要强制业务系统使用验证码。
        /// 2、	用户在提及登录业务时，对于登录失败的操作记录，根据规则（N秒内失败的记录）生成失效时间戳，连同创建时间、用户IP地址存放在userOptLog表中。
        /// 当前IP在N秒内登录失败了M次（判断userOptLog表中创建时间在N秒内的登录记录），则需要强制业务系统使用验证码。
        /// </summary>
        /// <param name="verifyCodeAppearRule">验证码出现规则</param>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="optType">操作类型1：注册成功2：登录失败</param>
        /// <returns></returns>
        public bool JudgeIfNeedPicVerifyCode(VerifyCodeAppearRule verifyCodeAppearRule, long ipAddress, PicVerifyIpStatType optType)
        {
            if (verifyCodeAppearRule == VerifyCodeAppearRule.Need)
                return true;

            var o = false;

            var c = CountPicVerifyCode(ipAddress, optType);

            switch (optType)
            {
                case PicVerifyIpStatType.LogonFailure:
                    o = c >= AucConfig.PicVerifyLogonLimitTimes;
                    break;
                case PicVerifyIpStatType.RegisterSuccess:
                    o = c >= AucConfig.PicVerifyRegisterLimitTimes;
                    break;
            }

            return o;
        }

        public int CountPicVerifyCode(long ipAddress, PicVerifyIpStatType optType)
        {
            var now = NetworkTime.Now;

            var last = DateTime.UtcNow;
            switch (optType)
            {
                case PicVerifyIpStatType.LogonFailure:
                    last = now.AddSeconds(-AucConfig.PicVerifyLogonLimitSeconds);
                    break;
                case PicVerifyIpStatType.RegisterSuccess:
                    last = now.AddSeconds(-AucConfig.PicVerifyRegisterLimitSeconds);
                    break;
            }

            return _picVerifyIpStatRepository.Count(ipAddress, last, optType);
        }

        /// <summary>
        /// 手机验证码判断逻辑
        /// 用户在手机注册、通过密保手机找回密码操作时，需要将生成的手机验证码、用户IP地址、手机号、创建时间
        /// 以及根据业务规则生成的失效时间戳保存在MobileVerifyCodeHistory表中。
        /// 1、	一个手机号，n秒内不能超过最大上限条数
        /// 判断N秒内MobileVerifyCodeHistory表中该手机号码生成的手机验证码个数。
        /// 2、	一个ip，在n秒内只能发m个手机验证码
        /// 判断MobileVerifyCodeHistory表中创建时间在N秒内的生成的手机验证码个数。
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="mobile"></param>
        /// <param name="smsType"></param>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        public UserCode JudgeIfCanSendMobileVerifyCode(string mobile, long ipAddress, SmsType smsType, int solutionId = -1)
        {
            var rs = UserCode.Success;
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                mobile = GetMobileKey(mobile, solutionId, smsType);
                var oc = CountMobileOneVerifyCode(mobile);
                if (oc > 0) return UserCode.MobileVerifyCodeSended;
                var c = CountMobileNoVerifyCode(mobile);
                if (c >= AucConfig.MobileVerifyNoLimitNumbers)
                {
                    return UserCode.ExceedMobileVerifyCodeSendNumber;
                }
                var ipCount = CountMobileIpVerifyCode(ipAddress);
                if (ipCount >= AucConfig.MobileVerifyIpLimitNumbers)
                {
                    return UserCode.ExceedMobileVerifyCodeSendNumber;
                }
            }
            return rs;
        }

        private string GetMobileKey(string mobile, int solutionId, SmsType smsType)
        {
            switch (smsType)
            {
                case SmsType.Forget:
                    var forgetSmsCode = new ForgetMobileVerifyCode(mobile, string.Empty);
                    return forgetSmsCode.GetKey().ToString();
                case SmsType.Login:
                    var loginSmsCode = new LoginMobileVerifyCode(mobile, string.Empty);
                    return loginSmsCode.GetKey().ToString();
                case SmsType.Register:
                    var registerSmsCode = new RegisterMobileVerifyCode(mobile, string.Empty);
                    return registerSmsCode.GetKey().ToString();
                case SmsType.Security:
                    var securitySmsCode = new SecurityMobileVerifyCode(mobile, string.Empty);
                    return securitySmsCode.GetKey().ToString();
                case SmsType.SolutionForget:
                    var solutionForgetSmsCode = new SolutionForgetSmsCode(mobile, string.Empty, solutionId);
                    return solutionForgetSmsCode.GetKey().ToString();
                case SmsType.SolutionSecurity:
                    var solutionSecuritySmsCode = new SolutionSecuritySmsCode(mobile, string.Empty, solutionId);
                    return solutionSecuritySmsCode.GetKey().ToString();
            }
            return mobile;
        }



        public int CountMobileOneVerifyCode(string mobile)
        {
            var spec = _mobileVerifyCodeLogRepository.CreateSpecification();

            spec = spec.Where(u => u.Mobile == mobile);
            var now = NetworkTime.Now;
            var last = now.AddSeconds(-AucConfig.MobileVerifyOneLimitSeconds);
            spec = spec.Where(u => u.CreateTime > last);

            return _mobileVerifyCodeLogRepository.Count(spec);
        }

        public int CountMobileNoVerifyCode(string mobile)
        {
            var spec = _mobileVerifyCodeLogRepository.CreateSpecification();

            spec = spec.Where(u => u.Mobile == mobile);
            var now = NetworkTime.Now;
            var last = now.AddSeconds(-AucConfig.MobileVerifyNoLimitSeconds);
            spec = spec.Where(u => u.CreateTime > last);

            return _mobileVerifyCodeLogRepository.Count(spec);
        }

        public int CountMobileIpVerifyCode(long ipAddress)
        {
            var spec = _mobileVerifyCodeLogRepository.CreateSpecification();

            spec = spec.Where(u => u.IPAddress == ipAddress);
            var now = NetworkTime.Now;
            var last = now.AddSeconds(-AucConfig.MobileVerifyIpLimitSeconds);
            spec = spec.Where(u => u.CreateTime > last);

            return _mobileVerifyCodeLogRepository.Count(spec);
        }


        /// <summary>
        /// 记录用户注册成功/登录失败统计
        /// </summary>
        public void RecordPicVerifyIpStat(long ipAddress, PicVerifyIpStatType optType)
        {
            var picVerifyIpStat = new PicVerifyIpStat
            {
                IPAddress = ipAddress,
                OptType = optType,
            };
            _picVerifyIpStatRepository.Create(picVerifyIpStat);
        }

        /// <summary>
        /// 删除用户注册成功/登录失败统计
        /// </summary>
        public void RemovePicVerifyIpStat(long ipAddress, PicVerifyIpStatType optType)
        {
            _picVerifyIpStatRepository.Remove(ipAddress, optType);
        }

        /// <summary>
        /// 记录手机验证码日志
        /// </summary>
        private void RecordMobileVerifyCodeLog(string mobile, long ipAddress)
        {
            var mobileVerifyCodeLog = new MobileVerifyCodeStat
            {
                IPAddress = ipAddress,
                Mobile = mobile,
            };
            _mobileVerifyCodeLogRepository.Create(mobileVerifyCodeLog);
        }


        static string GenerateVerifyCode(int length)
        {
            var verity = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                char c = strCode[rnd.Next(strCode.Length)];
                verity.Append(c);
            }
            return verity.ToString();
        }

        static string GeneratePicVerifyCode(int length, PicVerifyCodeDisplayFormat displayFormat)
        {
            string word;
            switch (displayFormat)
            {
                case PicVerifyCodeDisplayFormat.OnlyLetters:
                    word = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    break;
                case PicVerifyCodeDisplayFormat.DigitOrLetters:
                    word = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                default:
                    word = strCode;
                    break;
            }

            var verity = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                char c = word[rnd.Next(word.Length)];
                verity.Append(c);
            }
            return verity.ToString();
        }


        /// <summary>
        /// 生成bit64位的图片验证码串
        /// </summary>
        public static string GetVerifyPic(string verifyCode)
        {
            Arguments.NotNullOrWhiteSpace(verifyCode, "verifyCode");
            var img = GetVerifyImage(verifyCode);
            return ImageToBase64String(img);
        }

        public static Image GetVerifyImage(string verifyCode)
        {
            Arguments.NotNullOrWhiteSpace(verifyCode, "verifyCode");

            var len = verifyCode.Length;
            var width = 27 * len;
            var height = 50;
            var image = new Bitmap(width, height);

            int nRed, nGreen, nBlue;
            var rnd = new Random(Environment.TickCount);
            nRed = rnd.Next(128) + 128;
            nGreen = rnd.Next(128) + 128;
            nBlue = rnd.Next(128) + 128;

            var g = Graphics.FromImage(image);
            g.FillRectangle(new SolidBrush(Color.FromArgb(nRed, nGreen, nBlue)), 0, 0, width, height);

            //混淆线
            var lines = 3;
            using (var pen = new Pen(Color.FromArgb(nRed - 60, nGreen - 60, nBlue - 40), 2))
            {
                for (int i = 0; i < lines; i++)
                {
                    g.DrawLine(pen, rnd.Next(width), rnd.Next(height), rnd.Next(width), rnd.Next(height));
                }
            }

            int px = 5;
            int index = 0;
            foreach (var c in verifyCode)
            {
                int y = rnd.Next(6) + 2;
                var s = c.ToString();
                using (var font = new Font("Courier New", GetNextSize(rnd, index, len, width, px), FontStyle.Bold))
                {
                    g.DrawString(s, font, new SolidBrush(Color.FromArgb(nRed - 60 + y * 3, nGreen - 60 + y * 3, nBlue - 40 + y * 3)), px, y);
                    g.ResetTransform();
                    px += (int)(g.MeasureString(s, font).Width / 1.5);
                }
                index++;
            }

            return image;
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        static int GetNextSize(Random rnd, int index, int count, double width, double px)
        {
            double p = width / (px * count / index);
            double min = 3 * p;
            double size = (p == 0 ? 20 : 16 * p);
            return 16 + rnd.Next((int)min, (int)(min + size));
        }

        static string ImageToBase64String(Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                return Convert.ToBase64String(stream.ToArray());
            }
        }


        public PicVerifyCode GeneratePicVerifyCode(int appId)
        {
            var displayFormat = _appBaseService.GetPicVerifyCodeDisplayFormat(appId);
            return GeneratePicVerifyCode(displayFormat);
        }

        public PicVerifyCode GeneratePicVerifyCode(Solution solution)
        {
            var displayFormat = solution.PicVerifyCodeDisplayFormat;
            return GeneratePicVerifyCode(displayFormat);
        }

        public PicVerifyCode GeneratePicVerifyCode(PicVerifyCodeDisplayFormat displayFormat)
        {
            var sessionId = Guid.NewGuid().ToString("N");
            var verifyCode = GeneratePicVerifyCode(4, displayFormat);
            var picVerifyCode = new PicVerifyCode(sessionId, verifyCode);
            _keyValueRepository.CreateKeyValue(picVerifyCode.GetKey().ToString(), picVerifyCode.Value, TimeSpan.FromMinutes(AucConfig.PicVerifyCodeExpireMinutes));
            return picVerifyCode;
        }

        public MobileVerifyCode GenerateMobileVerifyCode(MobileTemplate mobileTemplate, long ipAddressInt,
            SmsType smsType, int solutionId = 0)
        {
            switch (smsType)
            {
                case SmsType.Forget:
                    return GenerateMobileVerifyCode<ForgetMobileVerifyCode>(mobileTemplate, ipAddressInt);
                case SmsType.Login:
                    return GenerateMobileVerifyCode<LoginMobileVerifyCode>(mobileTemplate, ipAddressInt);
                case SmsType.Register:
                    return GenerateMobileVerifyCode<RegisterMobileVerifyCode>(mobileTemplate, ipAddressInt);
                case SmsType.Security:
                    return GenerateMobileVerifyCode<SecurityMobileVerifyCode>(mobileTemplate, ipAddressInt);
                case SmsType.SolutionForget:
                    return GenerateMobileVerifyCode<SolutionForgetSmsCode>(mobileTemplate, ipAddressInt, solutionId);
                case SmsType.SolutionSecurity:
                    return GenerateMobileVerifyCode<SolutionSecuritySmsCode>(mobileTemplate, ipAddressInt, solutionId);
            }
            return null;
        }

        private T GenerateMobileVerifyCode<T>(MobileTemplate mobileTemplate, long ipAddressInt) where T : MobileVerifyCode, new()
        {
            var verifyCode = GenerateVerifyCode(6);
            var mobileVerifyCode = new T();
            mobileVerifyCode.Instance(mobileTemplate.Mobile, verifyCode);
            var key = mobileVerifyCode.GetKey().ToString();
            if (!_mobileVerifyRepository.GetValue(key).Value.IsNullOrWhiteSpace())
                _mobileVerifyRepository.Remove(key);
            _mobileVerifyRepository.CreateKeyValue(key, mobileVerifyCode.Value, TimeSpan.FromMinutes(AucConfig.MobileVerifyCodeExpireMinutes));
            mobileTemplate.BodyTemplate = mobileTemplate.BodyTemplate.Replace("$verify$", mobileVerifyCode.VerifyCode);
            MsgUtil.SendSms(mobileTemplate.Mobile, mobileTemplate.BodyTemplate);
            //插入历史记录表
            RecordMobileVerifyCodeLog(key, ipAddressInt);
            return mobileVerifyCode;
        }

        /// <summary>
        /// 自定义帐号短信验证码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mobileTemplate"></param>
        /// <param name="ipAddressInt"></param>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        private T GenerateMobileVerifyCode<T>(MobileTemplate mobileTemplate, long ipAddressInt, int solutionId) where T : SolutionSmsCode, new()
        {
            var mobileVerifyCode = new T();
            var verifyCode = GenerateVerifyCode(6);
            mobileVerifyCode.Instance(mobileTemplate.Mobile, verifyCode, solutionId);
            var key = mobileVerifyCode.GetKey().ToString();
            if (!_mobileVerifyRepository.GetValue(key).Value.IsNullOrWhiteSpace())
                _mobileVerifyRepository.Remove(key);
            _mobileVerifyRepository.CreateKeyValue(key, mobileVerifyCode.Value, TimeSpan.FromMinutes(AucConfig.MobileVerifyCodeExpireMinutes));
            mobileTemplate.BodyTemplate = mobileTemplate.BodyTemplate.Replace("$verify$", mobileVerifyCode.VerifyCode);
            MsgUtil.SendSms(mobileTemplate.Mobile, mobileTemplate.BodyTemplate);
            //插入历史记录表
            RecordMobileVerifyCodeLog(key, ipAddressInt);
            return mobileVerifyCode;
        }

        public RecoverToken GenerateRecoverToken(long userId, string identity, string solution = "")
        {
            var token = Guid.NewGuid().ToString("N");
            var recoverToken = new RecoverToken(token, userId, identity, solution);
            _keyValueRepository.CreateKeyValue(recoverToken.GetKey().ToString(), recoverToken.Value, TimeSpan.FromMinutes(AucConfig.RecoverTokenExpireMinutes));
            return recoverToken;
        }

        public RecoverEmailVerifyCode GenerateRecoverEmailVerifyCode(long userId, string identity, EmailTemplate emailTemplate, string solutionCode = "")
        {
            var verifyCode = Guid.NewGuid().ToString("N");
            var recoverToken = new RecoverEmailVerifyCode(verifyCode, userId, emailTemplate.Email, identity, solutionCode);
            _emailVerifyRepository.CreateKeyValue(recoverToken.GetKey().ToString(), recoverToken.Value, TimeSpan.FromMinutes(AucConfig.EmailVerifyCodeExpireMinutes));
            emailTemplate.BodyTemplate = emailTemplate.BodyTemplate.Replace("$verify$", recoverToken.VerifyCode);
            SendEmail(emailTemplate);
            return recoverToken;
        }

        public EmailVerifyCode GenerateEmailVerifyCode(long userId, EmailTemplate emailTemplate)
        {
            var verifyCode = Guid.NewGuid().ToString("N");
            var emailVerifyCode = new EmailVerifyCode(userId, emailTemplate.Email, verifyCode);
            _emailVerifyRepository.CreateKeyValue(emailVerifyCode.GetKey().ToString(), emailVerifyCode.Value, TimeSpan.FromMinutes(AucConfig.EmailVerifyCodeExpireMinutes));
            emailTemplate.BodyTemplate = emailTemplate.BodyTemplate.Replace("$verify$", emailVerifyCode.VerifyCode);
            SendEmail(emailTemplate);
            return emailVerifyCode;
        }

        public SecurityEmailVerifyCode GenerateSecurityEmailVerifyCode(long userId, EmailTemplate emailTemplate)
        {
            var verifyCode = Guid.NewGuid().ToString("N");
            var emailVerifyCode = new SecurityEmailVerifyCode(userId, emailTemplate.Email, verifyCode);
            _emailVerifyRepository.CreateKeyValue(emailVerifyCode.GetKey().ToString(), emailVerifyCode.Value, TimeSpan.FromMinutes(AucConfig.EmailVerifyCodeExpireMinutes));
            emailTemplate.BodyTemplate = emailTemplate.BodyTemplate.Replace("$verify$", emailVerifyCode.VerifyCode);
            SendEmail(emailTemplate);
            return emailVerifyCode;
        }

        public T GetVerifyCode<T>(VerifyCodeKey verifyCodeKey) where T : VerifyCodeBase, new()
        {
            var verifyCode = new T();
            var key = verifyCodeKey.ToString();
            var code = _keyValueRepository.GetStringValue(key);
            verifyCode.Parse(code);
            return verifyCode;
        }

        public T GetEmailVerifyCode<T>(VerifyCodeKey verifyCodeKey) where T : VerifyCodeBase, IEmailVerifyCode, new()
        {
            var verifyCode = new T();
            var key = verifyCodeKey.ToString();
            var code = _emailVerifyRepository.GetValue(key);
            verifyCode.Parse(code);
            return verifyCode;
        }

        public T GetMobileVerifyCode<T>(VerifyCodeKey verifyCodeKey) where T : MobileVerifyCode, new()
        {
            var verifyCode = new T();
            var key = verifyCodeKey.ToString();
            var code = _mobileVerifyRepository.GetValue(key);
            verifyCode.Parse(code.Value);
            return verifyCode;
        }

        public bool ValidVerifyCode(IVerifyCode verifyCode, bool deleteOnScuccess)
        {
            var vc = GetVerifyCode<CheckVerifyCode>(verifyCode.GetKey());
            if (!vc.Valid(verifyCode))
                return false;

            if (deleteOnScuccess)
                RemoveVerifyCode(verifyCode);

            return true;
        }

        public UserCode SmsVerifyCodeEffective<T>(string mobile, string smsVerifyCode, int solutionId = -1) where T : MobileVerifyCode, new()
        {
            var mobileVerifyCode = new T();
            mobileVerifyCode.Instance(mobile, smsVerifyCode);
            if (typeof(T) == typeof(SolutionSecuritySmsCode))
            {
                (mobileVerifyCode as SolutionSecuritySmsCode).SolutionId = solutionId;
            }
            if (typeof(T) == typeof(SolutionForgetSmsCode))
            {
                (mobileVerifyCode as SolutionForgetSmsCode).SolutionId = solutionId;
            }
            var innerVerifyCode = GetMobileVerifyCode<T>(mobileVerifyCode.GetKey());

            var success = innerVerifyCode.Valid(mobileVerifyCode);
            if (success)
            {
                var code = ValidSmsCodeCount(mobileVerifyCode);
                if (code != UserCode.Success)
                    return code;
                return UserCode.Success;
            }
            AddMobileVerifyCount(mobileVerifyCode);
            return UserCode.InvalidVerifyCode;
        }


        /// <summary>
        ///  销毁验证码
        /// </summary>
        public void RemoveVerifyCode(IVerifyCode verifyCode)
        {
            _keyValueRepository.Remove(verifyCode.GetKey().ToString());
        }

        /// <summary>
        /// 销毁短信验证码
        /// </summary>
        /// <param name="verifyCode"></param>
        public void RemoveMobileVerifyCode(IVerifyCode verifyCode)
        {
            _mobileVerifyRepository.Remove(verifyCode.GetKey().ToString());
        }

        /// <summary>
        /// 增加短信验证码验证次数
        /// </summary>
        /// <param name="verifyCode"></param>
        public void AddMobileVerifyCount(IVerifyCode verifyCode)
        {
            _mobileVerifyRepository.SetKeyCount(verifyCode.GetKey().ToString());
        }

        /// <summary>
        /// 校验短信验证码是否失效
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public UserCode ValidSmsCodeCount(IVerifyCode verifyCode)
        {
            var mobileverify = _mobileVerifyRepository.GetValue(verifyCode.GetKey().ToString());
            if (mobileverify.VerifyCount >= AucConfig.MobileVerifyCodeVerifyTimes)
            {
                //RemoveMobileVerifyCode(verifyCode);
                return UserCode.InvalidSms;
            }
            return UserCode.Success;
        }

        /// <summary>
        /// 销毁邮件验证码
        /// </summary>
        /// <param name="verifyCode"></param>
        public void RemoveEmailVerifyCode(IVerifyCode verifyCode)
        {
            _emailVerifyRepository.Remove(verifyCode.GetKey().ToString());
        }

        /// <summary>
        /// 销毁当前用户下某个邮箱的所有验证码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="solutionCode"></param>
        public void RemoveUserEmailVerifyCode(long userId, string email, string solutionCode = null)
        {
            var user = _userRepository.Get(ShardParams.Empty, userId);
            var emailVerifyCode = new EmailVerifyCode(userId, email, "");
            _emailVerifyRepository.RemoveByValue(emailVerifyCode.Value);
            var securityEmailVerifyCode = new SecurityEmailVerifyCode(userId, email, "");
            _emailVerifyRepository.RemoveByValue(securityEmailVerifyCode.Value);
            if (user != null)
            {
                var recoverEmailVerifyCode = new RecoverEmailVerifyCode("", userId, email, user.LoginName, solutionCode);
                _emailVerifyRepository.RemoveByValue(recoverEmailVerifyCode.Value);
                recoverEmailVerifyCode = new RecoverEmailVerifyCode("", userId, email, user.LoginEmail, solutionCode);
                _emailVerifyRepository.RemoveByValue(recoverEmailVerifyCode.Value);
                recoverEmailVerifyCode = new RecoverEmailVerifyCode("", userId, email, user.LoginMobile, solutionCode);
                _emailVerifyRepository.RemoveByValue(recoverEmailVerifyCode.Value);
                recoverEmailVerifyCode = new RecoverEmailVerifyCode("", userId, email, user.IDCard, solutionCode);
                _emailVerifyRepository.RemoveByValue(recoverEmailVerifyCode.Value);
            }
        }

        /// <summary>
        /// 用户是否可用重发验证码
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="type">验证码重发发送统计类型</param>
        /// <returns></returns>
        public UserCode UserVeriyCodeRepeateSendLimit(long userId, UserVeriyCodeRepeateSendStatType type)
        {
            var key = string.Format(UserVeriyCodeRepeateSendLimitFmtKey, userId, type);
            var isLimit = _keyValueRepository.GetValue<bool>(key);
            if (!isLimit)
            {
                var now = NetworkTime.Now;
                var last = now.AddSeconds(-AucConfig.UserVeriyCodeRepeateSendStatExpireSeconds);
                var spec = _userVeriyCodeRepeateSendStatRepository.CreateSpecification();

                spec = spec.Where(u => u.UserId == userId && u.Type == type && u.ExpireTime > last);

                var count = _userVeriyCodeRepeateSendStatRepository.Count(spec);
                isLimit = count >= AucConfig.UserVeriyCodeRepeateSendMaxCount;
                if (!isLimit)
                {
                    _userVeriyCodeRepeateSendStatRepository.Create(new UserVeriyCodeRepeateSendStat
                    {
                        UserId = userId,
                        Type = type
                    });
                    return UserCode.Success;
                }


                _keyValueRepository.SetKeyValue(key, true, TimeSpan.FromMinutes(AucConfig.UserVeriyCodeRepeateSendLimitIntervalMinutes));

            }
            return UserCode.TryManyTimes;
        }

        private void SendEmail(EmailTemplate emaiTemplate)
        {
            if (!emaiTemplate.BodyTemplate.IsNullOrWhiteSpace())
                MsgUtil.SendEmail(emaiTemplate.Email, emaiTemplate.BodyTemplate, emaiTemplate.Subject, emaiTemplate.SenderDispalyName);
        }
    }

}
