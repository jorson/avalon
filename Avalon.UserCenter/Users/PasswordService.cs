using System.Collections;
using System.IO.IsolatedStorage;
using System.Runtime.Remoting;
using System.Web;
using Avalon.UserCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;
using Avalon.Framework;
using Avalon.CloudClient;

namespace Avalon.UserCenter
{
    public class PasswordService : IService
    {
        private static ILog _logger = LogManager.GetLogger<string>();
        private readonly ICustomerSupportRepository _customerSupportRepository;
        private readonly IIDCardRetrieveRepository _idCardRetrieveRepository;
        private readonly IUserRegisterLogRepository _userRegisterLogRepository;
        private readonly IUserSecurityRepository _userSecurityRepository;
        private readonly VerifyCodeService _verifyCodeService;
        private readonly IUserRepository _userRepository;
        private readonly IUserMainHistoryLogRepository _userMainHistoryLogRepository;
        private readonly IUserLoginLogRepository _userLoginLogRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly AppBaseService _appBaseService;
        private readonly UserService _userService;

        public PasswordService(ICustomerSupportRepository customerSupportRepository, IIDCardRetrieveRepository iidCardRetrieveRepository,
            IUserRegisterLogRepository userRegisterLogRepository, IUserSecurityRepository userSecurityRepository, VerifyCodeService verifyCodeService,
            IUserRepository userRepository, IUserMainHistoryLogRepository userMainHistoryLogRepository,
            AppBaseService appBaseService, IUserLoginLogRepository userLoginLogRepository,
            UserService userService, IUserAccountRepository userAccountRepository)
        {
            _customerSupportRepository = customerSupportRepository;
            _idCardRetrieveRepository = iidCardRetrieveRepository;
            _userRegisterLogRepository = userRegisterLogRepository;
            _userSecurityRepository = userSecurityRepository;
            _verifyCodeService = verifyCodeService;
            _userRepository = userRepository;
            _userMainHistoryLogRepository = userMainHistoryLogRepository;
            _appBaseService = appBaseService;
            _userLoginLogRepository = userLoginLogRepository;
            _userService = userService;
            _userAccountRepository = userAccountRepository;
        }

        public IDCardRetrieve GetIdCardRetrieve(int id)
        {
            return _idCardRetrieveRepository.Get(id);
        }
        /// <summary>
        /// 根据ID查找省份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IpProvince GetIpProvinceById(int id)
        {
            return _appBaseService.GetIpProvinceById(id);
        }

        /// <summary>
        /// 根据ID查找城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IpCity GetIpCityById(int id)
        {
            return _appBaseService.GetIpCityById(id);
        }
        /// <summary>
        /// 获取登陆统计信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<object> GetLoginStaList(int id)
        {
            ManulRetrieve manulRetrieve = _customerSupportRepository.Get(id);
            IList<UserLoginLog> list = _userLoginLogRepository.GetLoginStaList(manulRetrieve.UserId, manulRetrieve.CreateTime);
            IList<object> listData = new List<object>();
            foreach (UserLoginLog userLoginLog in list)
            {
                IpCity ipCity = GetIpCityById(userLoginLog.IpCityId);
                listData.Add(new
                {
                    CityNm = ipCity.CityDisplayName(),
                    ProviceNm = GetIpProvinceById(ipCity == null ? 0 : ipCity.ProvinceId).ProvinceDisplayEmpty(),
                    LoginTime = userLoginLog.LoginTime.GetYTDHM(),
                    userLoginLog.IpCityIdNum
                });
            }
            return listData;
        }
        /// <summary>
        /// 根据编号获取申诉信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetManulRetrieve(int id)
        {
            ManulRetrieve manulRetrieve = _customerSupportRepository.Get(id);

            if (manulRetrieve == null)
            {
                return null;
            }
            IList<UserMainHistoryLog> list = GetMainHistoryLogBuUserId(manulRetrieve.UserId, manulRetrieve.CreateTime);
            UserRegisterLog modelReg = _userRegisterLogRepository.FindOne(_userRegisterLogRepository.CreateSpecification().Where(a => a.UserId == manulRetrieve.UserId));
            IList<UserLoginLog> listLog = _userLoginLogRepository.FindAll(_userLoginLogRepository.CreateSpecification().Where(a => a.UserId == manulRetrieve.UserId && a.LoginTime <= manulRetrieve.CreateTime));
            return new
            {
                manulRetrieve.Id,
                manulRetrieve.UserId,
                //姓名
                manulRetrieve.UserFullName,
                //申诉人
                manulRetrieve.UserIdentity,
                //创建时间
                manulRetrieve.CreateTime,
                //审核时间
                manulRetrieve.AuditTime,
                //审核状态
                manulRetrieve.AuditStatus,
                //审核人名称
                manulRetrieve.AuditorUserName,
                //联系邮箱
                manulRetrieve.ContactEmail,
                //联系电话
                manulRetrieve.ContactMobile,
                manulRetrieve.NofiyFlag,
                OtherInfo = new
                {
                    OldPassWord = GetCheckInfo(list, manulRetrieve.OtherInfo.OldPassWord, UserHistoryValueType.Password),
                    LoginMobile = GetCheckInfo(list, manulRetrieve.OtherInfo.LoginMobile, UserHistoryValueType.LoginMobile),
                    LoginMail = GetCheckInfo(list, manulRetrieve.OtherInfo.LoginMail, UserHistoryValueType.LoginEmail),
                    LoginIDCard = GetCheckInfo(list, manulRetrieve.OtherInfo.LoginIDCard, UserHistoryValueType.IDCard),
                    ProtectMail = GetCheckInfo(list, manulRetrieve.OtherInfo.ProtectMail, UserHistoryValueType.SecurityEmail),
                    ProtectMobile = GetCheckInfo(list, manulRetrieve.OtherInfo.ProtectMobile, UserHistoryValueType.SecurityMobile),
                    LoginArea = GetAreaCheckInfo(listLog, manulRetrieve.OtherInfo.LoginArea),
                    RegisArea = GetAreaCheckInfo(modelReg, manulRetrieve.OtherInfo.RegisArea),
                    manulRetrieve.OtherInfo.UserRemarks,
                    manulRetrieve.OtherInfo.Reason
                }

            };
        }
        /// <summary>
        /// 获取审核信息组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="infoList"></param>
        /// <returns></returns>
        public IList<object> GetAreaCheckInfo(IList<UserLoginLog> list, IList<AreaInfo> infoList)
        {
            IList<object> listRst = new List<object>();
            if (infoList != null)
            {

                foreach (AreaInfo cellinfo in infoList)
                {
                    listRst.Add(GetAreaCheckInfo(list, cellinfo));
                }
            }
            return listRst;
        }
        /// <summary>
        /// 获取地区审核信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="areaInfo"></param>
        /// <returns></returns>
        public object GetAreaCheckInfo(UserRegisterLog model, AreaInfo areaInfo)
        {
            if (areaInfo != null)
            {
                return new { CityNm = GetIpCityById(areaInfo.CityId).CityDisplayName(), ProviceNm = GetIpProvinceById(areaInfo.ProviceId).ProvinceDisplayEmpty(), CheckOk = (model.IpCityId == areaInfo.CityId) };

            }
            return new { CityNm = "", ProviceNm = "", CheckOk = false };
        }
        /// <summary>
        /// 获取地区审核信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="areaInfo"></param>
        /// <returns></returns>
        public object GetAreaCheckInfo(IList<UserLoginLog> list, AreaInfo areaInfo)
        {
            if (areaInfo != null)
            {
                foreach (UserLoginLog mainlog in list)
                {
                    if (areaInfo.CityId == mainlog.IpCityId)
                    {
                        return new { CityNm = GetIpCityById(areaInfo.CityId).CityDisplayName(), ProviceNm = GetIpProvinceById(areaInfo.ProviceId).ProvinceDisplayEmpty(), CheckOk = true };
                    }
                }
                return new { CityNm = GetIpCityById(areaInfo.CityId).CityDisplayName(), ProviceNm = GetIpProvinceById(areaInfo.ProviceId).ProvinceDisplayEmpty(), CheckOk = false };
            }
            return new { CityNm = "", ProviceNm = "", CheckOk = false };
        }
        /// <summary>
        /// 获取审核信息组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="infoList"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<object> GetCheckInfo(IList<UserMainHistoryLog> list, IList<string> infoList, UserHistoryValueType type)
        {
            IList<object> listRst = new List<object>();
            if (infoList != null)
            {

                foreach (string cellinfo in infoList)
                {
                    listRst.Add(GetCheckInfo(list, cellinfo, type));
                }
            }
            return listRst;
        }
        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="info"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetCheckInfo(IList<UserMainHistoryLog> list, string info, UserHistoryValueType type)
        {
            if (info != null)
            {
                foreach (UserMainHistoryLog mainlog in list)
                {
                    if (type == UserHistoryValueType.IDCard)
                    {
                        if (type.Equals(mainlog.ValType) &&
                            (UserValid.GetIdCardStoreStr(info).Equals(mainlog.OldVal) ||
                             UserValid.GetIdCardStoreStr(info).Equals(mainlog.NewVal)))
                        {
                            return new {ColumnText = info, CheckOk = true};
                        }
                    }
                    else
                    {
                        if (type.Equals(mainlog.ValType) && (info.Equals(mainlog.OldVal) || info.Equals(mainlog.NewVal)))
                        {
                            return new {ColumnText = info, CheckOk = true};
                        }
                    }
                }
                return new { ColumnText = info, CheckOk = false };
            }
            return new { ColumnText = "", CheckOk = false };
        }

        /// <summary>
        /// 根据用户编号获取修改历史信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="createTime"></param>
        /// <returns></returns>
        public IList<UserMainHistoryLog> GetMainHistoryLogBuUserId(long userid, DateTime createTime)
        {
            return _userMainHistoryLogRepository.FindAll(
                _userMainHistoryLogRepository.CreateSpecification().Where(a => a.UserId == userid && a.CreateTime <= createTime));
        }



        /// <summary>
        /// 提交人工审核信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="otherInfo">帐号核心信息和其他补充信息</param>
        /// <param name="contactEmail">联系邮箱</param>
        /// <param name="contactMobile">联系手机</param>
        /// <param name="fullName">姓名</param>
        /// <param name="appId">添加记录的appid</param>
        /// <param name="userIdentity">申诉帐号</param>
        /// <returns></returns>
        public ResultWrapper<UserCode, ManulRetrieve> SubmitCustomerSupport(User user
            , ManulRetrieveOtherInfo otherInfo, string contactEmail, string contactMobile, string fullName, int appId,
            string userIdentity)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(contactEmail));
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(contactMobile));
            validor.AppendValidAndSet(() => UserValid.ValidUserFullNameFormat(fullName));

            var code = validor.Valid();

            if (code == UserCode.Success)
            {
                for (int i = 0; i < otherInfo.OldPassWord.Count; i++)
                {
                    code = RsaUtil.DecryptAndCheck(otherInfo.OldPassWord[i], pwd => otherInfo.OldPassWord[i] = pwd);
                    if (code != UserCode.Success)
                        return new ResultWrapper<UserCode, ManulRetrieve>(code);
                    UserValid.ValidPasswordFormat(otherInfo.OldPassWord[i]);
                    otherInfo.OldPassWord[i] = User.EncryptPassword(otherInfo.OldPassWord[i], user);
                }
                var objCustomerSupport = new ManulRetrieve
                {
                    UserId = user.Id,
                    UserIdentity = userIdentity,
                    CreateTime = NetworkTime.Now,
                    AuditStatus = IDCardRetrieveAuditStatus.Submit,
                    AuditTime = NetworkTime.Null,
                    UserFullName = fullName,
                    ContactEmail = contactEmail,
                    ContactMobile = contactMobile,
                    OtherInfo = otherInfo,
                    RegistAppId =
                        _userRegisterLogRepository.FindOne(
                            _userRegisterLogRepository.CreateSpecification().Where(o => o.UserId == user.Id))
                            .RegisterAppId,
                    CreatAppId = appId
                };

                _customerSupportRepository.Create(objCustomerSupport);
                return new ResultWrapper<UserCode, ManulRetrieve>(UserCode.Success, objCustomerSupport);
            }
            return new ResultWrapper<UserCode, ManulRetrieve>(code);
        }
        /// <summary>
        /// 审核并保存人工申述信息
        /// </summary>
        public void ExamineManul(ExamineMessage examineMessage, int appId, int terminalCode, long ipAddress, string solutionCode)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            var recordData = _customerSupportRepository.Get(examineMessage.IdCardRetrieveId);
            recordData.AuditStatus = examineMessage.AditStatus;
            recordData.OtherInfo.Reason = examineMessage.DenyReason;
            recordData.NofiyFlag = examineMessage.NotifyStatus;
            recordData.AuditorUserId = examineMessage.AuditorUserId;
            recordData.AuditorUserName = examineMessage.AuditorUserName;
            recordData.AuditTime = NetworkTime.Now;
            switch (examineMessage.AditStatus)
            {
                case IDCardRetrieveAuditStatus.Deny:
                    examineMessage.EmailFailMessage = examineMessage.EmailFailMessage.Replace("{{reason}}", HttpUtility.HtmlEncode(examineMessage.DenyReason)).Replace("{{solution}}", solutionCode);
                    examineMessage.MobileFailMessage = examineMessage.MobileFailMessage.Replace("{{reason}}", examineMessage.DenyReason).Replace("{{solution}}", solutionCode);
                    SendMessage(recordData, examineMessage.MobileFailMessage, examineMessage.EmailFailMessage, examineMessage.EmailSender.Replace("{{solution}}", solutionCode), examineMessage.EmailTitle.Replace("{{solution}}", solutionCode));
                    break;
                case IDCardRetrieveAuditStatus.Pass:
                    var userSecurity = _userSecurityRepository.FindOne(
                            _userSecurityRepository.CreateSpecification().Where(o => o.UserId == recordData.UserId));
                    userSecurity.Email = recordData.ContactEmail;
                    userSecurity.Mobile = recordData.ContactMobile;
                    _userSecurityRepository.Update(userSecurity);
                    examineMessage.EmailSuccessMessage = examineMessage.EmailSuccessMessage.Replace("{{mobile}}", "&nbsp;" + recordData.ContactMobile + "&nbsp;")
                            .Replace("{{email}}", "&nbsp;" + recordData.ContactEmail + "&nbsp;").Replace("{{solution}}", solutionCode);
                    examineMessage.MobileSuccessMessage = examineMessage.MobileSuccessMessage.Replace("{{mobile}}", recordData.ContactMobile)
                            .Replace("{{email}}", recordData.ContactEmail).Replace("{{solution}}", solutionCode);
                    SendMessage(recordData, examineMessage.MobileSuccessMessage, examineMessage.EmailSuccessMessage, examineMessage.EmailSender.Replace("{{solution}}", solutionCode), examineMessage.EmailTitle.Replace("{{solution}}", solutionCode));
                    break;
            }
            _customerSupportRepository.Update(recordData);
        }
        /// <summary>
        /// 人工申述信息
        /// </summary>
        /// <param name="manulRetrieve"></param>
        /// <param name="sms"></param>
        /// <param name="emailmessage"></param>
        /// <param name="emailsender"></param>
        /// <param name="emailtitle"></param>
        private void SendMessage(ManulRetrieve manulRetrieve, string sms, string emailmessage, string emailsender, string emailtitle)
        {
            switch (manulRetrieve.NofiyFlag)
            {
                case AuditNotifyStatus.Mail:
                    if (!manulRetrieve.ContactEmail.IsNullOrWhiteSpace())
                        manulRetrieve.EmailNofiyJobId =
                            MsgUtil.SendEmail(manulRetrieve.ContactEmail, emailmessage, emailtitle, emailsender).Id;
                    break;
                case AuditNotifyStatus.Sms:
                    if (!manulRetrieve.ContactMobile.IsNullOrWhiteSpace())
                        manulRetrieve.MobileNofiyJobId = MsgUtil.SendSms(manulRetrieve.ContactMobile, sms).Id;
                    break;
                case AuditNotifyStatus.SmsAndMail:
                    if (!manulRetrieve.ContactEmail.IsNullOrWhiteSpace())
                        manulRetrieve.EmailNofiyJobId =
                            MsgUtil.SendEmail(manulRetrieve.ContactEmail, emailmessage, emailtitle, emailsender).Id;
                    if (!manulRetrieve.ContactMobile.IsNullOrWhiteSpace())
                        manulRetrieve.MobileNofiyJobId = MsgUtil.SendSms(manulRetrieve.ContactMobile, sms).Id;
                    break;
            }

        }
        /// <summary>
        /// 审核并保存证件信息
        /// </summary>
        public void ExamineIdCard(ExamineMessage examineMessage, int appId, int terminalCode, long ipAddress, string solutionCode)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            var idCardRetrieve = _idCardRetrieveRepository.Get(examineMessage.IdCardRetrieveId);
            idCardRetrieve.AuditStatus = examineMessage.AditStatus;
            if (idCardRetrieve.AuditStatus == IDCardRetrieveAuditStatus.Deny)
                idCardRetrieve.AuditDenyReason = examineMessage.DenyReason;
            idCardRetrieve.NofiyFlag = examineMessage.NotifyStatus;
            idCardRetrieve.AuditorUserId = examineMessage.AuditorUserId;
            idCardRetrieve.AuditorUserName = examineMessage.AuditorUserName;
            idCardRetrieve.AuditTime = NetworkTime.Now;
            switch (examineMessage.AditStatus)
            {
                case IDCardRetrieveAuditStatus.Deny:
                    examineMessage.MobileFailMessage = examineMessage.MobileFailMessage.Replace("{{reason}}", examineMessage.DenyReason).Replace("{{solution}}", solutionCode);
                    examineMessage.EmailFailMessage = examineMessage.EmailFailMessage.Replace("{{reason}}", HttpUtility.HtmlEncode(examineMessage.DenyReason)).Replace("{{solution}}", solutionCode);
                    SendMessage(idCardRetrieve, examineMessage.MobileFailMessage, examineMessage.EmailFailMessage, examineMessage.EmailSender.Replace("{{solution}}", solutionCode), examineMessage.EmailTitle.Replace("{{solution}}", solutionCode));
                    break;
                case IDCardRetrieveAuditStatus.Pass:
                    var userSecurity = _userSecurityRepository.FindOne(
                            _userSecurityRepository.CreateSpecification().Where(o => o.UserId == idCardRetrieve.UserId));
                    userSecurity.Email = idCardRetrieve.ContactEmail;
                    userSecurity.Mobile = idCardRetrieve.ContactMobile;
                    _userSecurityRepository.Update(userSecurity);
                    examineMessage.EmailSuccessMessage = examineMessage.EmailSuccessMessage.Replace("{{mobile}}", "&nbsp;" + idCardRetrieve.ContactMobile + "&nbsp;")
                            .Replace("{{email}}", "&nbsp;" + idCardRetrieve.ContactEmail + "&nbsp;").Replace("{{solution}}", solutionCode);
                    examineMessage.MobileSuccessMessage = examineMessage.MobileSuccessMessage.Replace("{{mobile}}", idCardRetrieve.ContactMobile)
                            .Replace("{{email}}", idCardRetrieve.ContactEmail).Replace("{{solution}}", solutionCode);
                    SendMessage(idCardRetrieve, examineMessage.MobileSuccessMessage, examineMessage.EmailSuccessMessage, examineMessage.EmailSender.Replace("{{solution}}", solutionCode), examineMessage.EmailTitle.Replace("{{solution}}", solutionCode));
                    break;
            }
            _idCardRetrieveRepository.Update(idCardRetrieve);
        }

        private void SendMessage(IDCardRetrieve idCardRetrieve, string sms, string emailmessage, string emailsender, string emailtitle)
        {
            switch (idCardRetrieve.NofiyFlag)
            {
                case AuditNotifyStatus.Mail:
                    if (!idCardRetrieve.ContactEmail.IsNullOrWhiteSpace())
                        idCardRetrieve.EmailNofiyJobId =
                            MsgUtil.SendEmail(idCardRetrieve.ContactEmail, emailmessage, emailtitle, emailsender).Id;
                    break;
                case AuditNotifyStatus.Sms:
                    if (!idCardRetrieve.ContactMobile.IsNullOrWhiteSpace())
                        idCardRetrieve.MobileNofiyJobId = MsgUtil.SendSms(idCardRetrieve.ContactMobile, sms).Id;
                    break;
                case AuditNotifyStatus.SmsAndMail:
                    if (!idCardRetrieve.ContactEmail.IsNullOrWhiteSpace())
                        idCardRetrieve.EmailNofiyJobId =
                            MsgUtil.SendEmail(idCardRetrieve.ContactEmail, emailmessage, emailtitle, emailsender).Id;
                    if (!idCardRetrieve.ContactMobile.IsNullOrWhiteSpace())
                        idCardRetrieve.MobileNofiyJobId = MsgUtil.SendSms(idCardRetrieve.ContactMobile, sms).Id;
                    break;
            }

        }

        /// <summary>
        /// 用户修改资料-邮箱找回密码
        /// </summary>
        public UserCode EmailResetPassword(string verifyCode
            , string newPassword, long ipAddress, int appId, int terminalCode)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            RsaUtil.DecryptAndCheck(newPassword, pwd => { newPassword = pwd; });
            var code = UserValid.ValidPasswordFormat(newPassword);
            if (code == UserCode.Success)
            {
                if (!verifyCode.IsNullOrWhiteSpace())
                {
                    var emailVerifyKey = RecoverEmailVerifyCode.BuildKey(verifyCode);
                    var recoverEmailVerifyCode = _verifyCodeService.GetEmailVerifyCode<RecoverEmailVerifyCode>(emailVerifyKey);
                    if (recoverEmailVerifyCode != null && recoverEmailVerifyCode.UserId > 0)
                    {
                        var user = _userRepository.Get(recoverEmailVerifyCode.UserId);
                        var email = GetUserForgotEmail(user);
                        var emailVerifyCode = new RecoverEmailVerifyCode(verifyCode, user.Id, email, recoverEmailVerifyCode.Identity);
                        if (recoverEmailVerifyCode.Valid(emailVerifyCode))
                        {
                            user.SetPassword(newPassword);
                            _userRepository.Update(user);
                            _verifyCodeService.RemoveEmailVerifyCode(emailVerifyCode);
                            return UserCode.Success;
                        }
                    }
                }
                code = UserCode.InvalidVerifyCode;
            }

            return code;
        }

        /// <summary>
        /// 自定义用户修改资料-邮箱找回密码
        /// </summary>
        public ResultWrapper<UserCode,string> EmailResetAccountPassword(string verifyCode, string newPassword,
            long ipAddress, int appId, int terminalCode)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            RsaUtil.DecryptAndCheck(newPassword, pwd => { newPassword = pwd; });
            if (!verifyCode.IsNullOrWhiteSpace())
            {
                var emailVerifyKey = RecoverEmailVerifyCode.BuildKey(verifyCode);
                var recoverEmailVerifyCode = _verifyCodeService.GetEmailVerifyCode<RecoverEmailVerifyCode>(emailVerifyKey);
                if (recoverEmailVerifyCode != null && recoverEmailVerifyCode.UserId > 0)
                {
                    var solution = _userService.GetSolutionByCode(recoverEmailVerifyCode.Solution);
                    var validor = new Validor<UserCode>(UserCode.Success);
                    validor.AppendValidAndSet(solution.NeedCustomSolution);
                    validor.AppendValidAndSet(() => UserValid.ValidAccountPasswordFormat(newPassword, solution));
                    var code = validor.Valid();
                    if (code == UserCode.Success)
                    {
                        var account = _userService.GetUserAccountByUserId(recoverEmailVerifyCode.UserId, solution.Id);
                        var security = _userSecurityRepository.Get(recoverEmailVerifyCode.UserId);
                        if (account == null)
                            return new ResultWrapper<UserCode, string>(UserCode.InvalidAccount, UserCode.InvalidAccount.GetDescription());
                        var email = security.Email;
                        var emailVerifyCode = new RecoverEmailVerifyCode(verifyCode, recoverEmailVerifyCode.UserId,
                            email, recoverEmailVerifyCode.Identity, solution.Code);
                        if (recoverEmailVerifyCode.Valid(emailVerifyCode))
                        {
                            account.SetPassword(newPassword);
                            _userAccountRepository.Update(account);
                            _verifyCodeService.RemoveEmailVerifyCode(emailVerifyCode);
                            return new ResultWrapper<UserCode, string>(UserCode.Success,UserCode.Success.GetDescription());
                        }
                        code = UserCode.InvalidVerifyCode;
                    }
                    var msg = code.GetDescription();
                    if (code == UserCode.InvalidCustomPasswordLength)
                        msg = solution.PasswordRuleDesc;
                    return new ResultWrapper<UserCode, string>(code,msg);
                }
            }

            return new ResultWrapper<UserCode, string>(UserCode.InvalidVerifyCode, UserCode.InvalidVerifyCode.GetDescription());
        }

        /// <summary>
        /// 获取用户的邮箱
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetUserForgotEmail(User user)
        {
            var security = _userSecurityRepository.Get(user.Id);
            if (!security.Email.IsNullOrWhiteSpace())
            {
                return security.Email;
            }
            return !user.LoginEmail.IsNullOrWhiteSpace() ? user.LoginEmail : null;
        }

        /// <summary>
        /// 用户修改资料-手机找回密码
        /// </summary>
        public UserCode MobileResetPassword(User user
            , string newPassword
            , string verifyCode
            , long ipAddress
            , int appId
            , int terminalCode
            )
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            RsaUtil.DecryptAndCheck(newPassword, pwd => { newPassword = pwd; });
            var code = UserValid.ValidPasswordFormat(newPassword);
            if (code == UserCode.Success)
            {
                var security = _userSecurityRepository.Get(user.Id);
                var mobile = !string.IsNullOrWhiteSpace(security.Mobile) ? security.Mobile : user.LoginMobile;
                var mobileVerifyCode = new ForgetMobileVerifyCode(mobile, verifyCode);
                //验证手机验证码是否合法,则保存新密码
                code = _verifyCodeService.SmsVerifyCodeEffective<ForgetMobileVerifyCode>(mobile, verifyCode);
                if (code != UserCode.Success)
                    return code;
                user.SetPassword(newPassword);
                _userRepository.Update(user);
                _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
            }
            return code;
        }

        /// <summary>
        /// 自定义用户修改资料-手机找回密码
        /// </summary>
        public ResultWrapper<UserCode,string> MobileResetAccountPassword(User user
            , string newPassword
            , Solution solution
            , string verifyCode
            , long ipAddress
            , int appId
            , int terminalCode
            )
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            RsaUtil.DecryptAndCheck(newPassword, pwd => { newPassword = pwd; });
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(solution.NeedCustomSolution);
            validor.AppendValidAndSet(() => UserValid.ValidAccountPasswordFormat(newPassword, solution));
            var code = validor.Valid();
            if (code == UserCode.Success)
            {
                var security = _userSecurityRepository.Get(user.Id);
                var mobile = !string.IsNullOrWhiteSpace(security.Mobile) ? security.Mobile : user.LoginMobile;
                var mobileVerifyCode = new SolutionForgetSmsCode(mobile, verifyCode, solution.Id);
                //验证手机验证码是否合法,则保存新密码
                code = _verifyCodeService.SmsVerifyCodeEffective<SolutionForgetSmsCode>(mobile, verifyCode, solution.Id);
                if (code != UserCode.Success)
                    return new ResultWrapper<UserCode, string>(code,code.GetDescription());
                var account = _userService.GetUserAccountByUserId(user.Id, solution.Id);
                if (account == null)
                    return new ResultWrapper<UserCode, string>(UserCode.InvalidAccount, UserCode.InvalidAccount.GetDescription());
                account.SetPassword(newPassword);
                _userAccountRepository.Update(account);
                _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
            }
            var msg = code.GetDescription();
            if (code == UserCode.InvalidCustomPasswordLength)
                msg = solution.PasswordRuleDesc;
            return new ResultWrapper<UserCode, string>(code,msg);
        }

        /// <summary>
        /// 校验手机验证码是否可用-手机找回密码
        /// </summary>
        public UserCode MobileResetPassword<T>(User user, string verifyCode, int solutionId = -1) where T : MobileVerifyCode, new()
        {
            var security = _userSecurityRepository.Get(user.Id);
            var mobile = !string.IsNullOrWhiteSpace(security.Mobile) ? security.Mobile : user.LoginMobile;
            //验证手机验证码是否合法
            return _verifyCodeService.SmsVerifyCodeEffective<T>(mobile, verifyCode, solutionId);
        }

        /// <summary>
        /// 找回密码——获取用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IList<ForgetModeInfo> GetUserForgetModes(User user)
        {
            Arguments.NotNull(user, "user");

            var rsList = new List<ForgetModeInfo>();

            var userSecurity = _userSecurityRepository.Get(user.Id);
            var emailForgetModeInfo = new ForgetModeInfo { Mode = ForgetMode.Email };
            if (!userSecurity.Email.IsNullOrEmpty() || !user.LoginEmail.IsNullOrEmpty())
            {
                emailForgetModeInfo.IsAvailable = true;
                emailForgetModeInfo.IsSecurity = !userSecurity.Email.IsNullOrEmpty();
                emailForgetModeInfo.ShowInfo = ShieldUtil.Email(emailForgetModeInfo.IsSecurity ? userSecurity.Email : user.LoginEmail);
            }
            rsList.Add(emailForgetModeInfo);
            var mobileForgetModeInfo = new ForgetModeInfo { Mode = ForgetMode.Mobile };
            if (!userSecurity.Mobile.IsNullOrEmpty() || !user.LoginMobile.IsNullOrEmpty())
            {
                mobileForgetModeInfo.IsAvailable = true;
                mobileForgetModeInfo.IsSecurity = !userSecurity.Mobile.IsNullOrEmpty();
                mobileForgetModeInfo.ShowInfo = ShieldUtil.Mobile(mobileForgetModeInfo.IsSecurity ? userSecurity.Mobile : user.LoginMobile);
            }
            rsList.Add(mobileForgetModeInfo);
            var idcardForgetModeInfo = new ForgetModeInfo { Mode = ForgetMode.IDCard };
            if (!user.IDCard.IsNullOrEmpty())
            {
                idcardForgetModeInfo.IsAvailable = true;
                idcardForgetModeInfo.ShowInfo = ShieldUtil.IdCard(user.DisplayIDCard);
            }
            rsList.Add(idcardForgetModeInfo);

            return rsList;
        }

        /// <summary>
        /// 提交证件找回密码信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idCard"></param>
        /// <param name="idCardImage"></param>
        /// <param name="contactEmail"></param>
        /// <param name="contactMobile"></param>
        /// <param name="fullName"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public ResultWrapper<UserCode, IDCardRetrieve> SubmitIDCardRecover(User user, string idCard, StoreObject idCardImage
            , string contactEmail, string contactMobile, string fullName, int appId)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => UserValid.ValidIDCardFormat(idCard));
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(contactEmail));
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(contactMobile));
            validor.AppendValidAndSet(() => UserValid.ValidUserFullNameFormat(fullName));
            validor.AppendValidAndSet(
                () => user.IDCardEquals(idCard) ? UserCode.Success : UserCode.IDCardPassword);

            var code = validor.Valid();

            if (code == UserCode.Success)
            {
                var userregisterlog = _userRegisterLogRepository.FindOne(_userRegisterLogRepository.CreateSpecification().Where(o => o.UserId == user.Id));
                var idCardRetrieve = new IDCardRetrieve
                {
                    IDCard = idCard,
                    CreateTime = NetworkTime.Now,
                    AuditStatus = IDCardRetrieveAuditStatus.Submit,
                    AuditTime = NetworkTime.Null,
                    ContactEmail = contactEmail,
                    ContactMobile = contactMobile,
                    IDCardImgObjectId = idCardImage.Id,
                    UserId = user.Id,
                    UserFullName = fullName,
                    CreatAppId = appId,
                    RegistAppId = userregisterlog.RegisterAppId
                };

                _idCardRetrieveRepository.Create(idCardRetrieve);
                return new ResultWrapper<UserCode, IDCardRetrieve>(UserCode.Success, idCardRetrieve);
            }
            return new ResultWrapper<UserCode, IDCardRetrieve>(code);
        }

        /// <summary>
        /// 修改用户资料--密码
        /// </summary>
        public UserCode ModifyUserPassword(long userId
            , string password
            , string newPassword
            , string emailDisplayName
            , string emailSubject
            , string emailTemplateExist
            , string emailTemplateNoExist
            , string smsTemplateExist
            , string smsTemplateNoExist
            , long ipAddress
            , int appId
            , int terminalCode)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            var user = _userRepository.Get(userId);
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => user == null ? UserCode.InvalidUser : UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(newPassword, pwd => { newPassword = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(newPassword));
            validor.AppendValidAndSet(() => ValidUserExEmailAndMobile(userId));
            validor.AppendValidAndSet(() => user.EqualsPasswordReturnCode(password), () =>
            {
                user.SetPassword(newPassword);
                _userRepository.Update(user);
                var userSecurity = _userService.GetUserSecurity(userId);
                var mobile = !string.IsNullOrWhiteSpace(userSecurity.Mobile)
                    ? userSecurity.Mobile
                    : !string.IsNullOrWhiteSpace(user.LoginMobile) ? user.LoginMobile : string.Empty;
                var email = !string.IsNullOrWhiteSpace(userSecurity.Email)
                    ? userSecurity.Email
                    : !string.IsNullOrWhiteSpace(user.LoginEmail) ? user.LoginEmail : string.Empty;

                SendEmailAndMobile(email, emailDisplayName, emailSubject, emailTemplateExist, emailTemplateNoExist,
                    mobile, smsTemplateExist, smsTemplateNoExist, ipAddress);
            });

            return validor.Valid();
        }

        /// <summary>
        /// 发送邮件和短信(含地点的)
        /// </summary>
        public void SendEmailAndMobile(string email, string emailDisplayName, string emailSubject,
            string emailTemplateExist, string emailTemplateNoExist, string mobile,
            string smsTemplateExist, string smsTemlateNoExist, long ipAddressInt)
        {
            if (!email.IsNullOrWhiteSpace())
            {
                if (UserValid.ValidEmailFormat(email) == UserCode.Success)
                {
                    _userService.SendEmailToSecurity(email, emailDisplayName, emailSubject,
                        emailTemplateExist, emailTemplateNoExist, ipAddressInt);
                }
            }
            if (!mobile.IsNullOrWhiteSpace())
            {
                if (UserValid.ValidMobileFormat(mobile) == UserCode.Success)
                {
                    _userService.SendSmsToSecurity(mobile, smsTemplateExist, smsTemlateNoExist, ipAddressInt);
                }
            }
        }

        /// <summary>
        /// 判断当前用户是否可以修改密码（前端用）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserCode ValidUserExEmailAndMobile(long userId)
        {
            var user = _userRepository.Get(userId);
            var userSecurity = _userSecurityRepository.Get(userId);
            return UserValid.ValidUserExEmailAndMobile(user, userSecurity);
        }

        private void SetWorkbenchVal(int appId, int terminalCode, long ipAddress)
        {
            Workbench.Current.Items[AucLogConst.auclogIp] = ipAddress;
            Workbench.Current.Items[AucLogConst.auclogAppId] = appId;
            Workbench.Current.Items[AucLogConst.auclogTerminalCode] = terminalCode;
        }

    }
}
