using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Avalon.UserCenter.Models;
using Avalon.Framework;
using Avalon.Utility;

using UserRegister = Avalon.UserCenter.Models.UserRegister;
using Avalon.CloudClient;


namespace Avalon.UserCenter
{
    public class UserService : IService
    {
        private static ILog _logger = LogManager.GetLogger<string>();

        private readonly IUserRepository _userRepository;
        private readonly IUserRegisterLogRepository _userRegisterLogRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ISolutionRepository _accountSolutionRepository;
        private readonly IUserLoginLogRepository _userLoginLogRepository;
        private readonly IUserMainHistoryLogRepository _userMainHistoryLogRepository;

        private readonly IUserSecurityRepository _userSecurityRepository;
        private readonly IUserPasswordErrorStatRepository _userPasswordErrorStatRepository;
        private readonly IUserAccountCreateInfoRepository _userAccountCreateInfoRepository;
        private readonly IUserOldPasswordRepository _userOldPasswordRepository;


        private readonly AppBaseService _appBaseService;
        private readonly VerifyCodeService _verifyCodeService;
        private readonly ThirdPartyService _thirdPartyService;

        private CacheDomain<UserId, string> usernameDomain;
        private CacheDomain<UserId, string> mobileDomain;
        private CacheDomain<UserId, string> emailDomain;
        private CacheDomain<UserId, string> idCardDomain;

        /// <summary>
        /// 接口中城市替换代码
        /// </summary>
        public const string LoginCity = "$login_city$";

        public UserService(IUserRepository userRepository
            , IUserRegisterLogRepository userRegisterLogRepository
            , IUserAccountRepository userAccountRepository
            , ISolutionRepository accountSolutionRepository
            , IUserLoginLogRepository userLoginLogRepository
            , IUserMainHistoryLogRepository userMainHistoryLogRepository
            , IUserSecurityRepository userSecurityRepository
            , IUserPasswordErrorStatRepository userPasswordErrorStatRepository
            , IUserAccountCreateInfoRepository userAccountCreateInfoRepository
            , IUserOldPasswordRepository userOldPasswordRepository
            , AppBaseService appBaseService
            , VerifyCodeService verifyCodeService, ThirdPartyService thirdPartyService)
        {
            _userRepository = userRepository;
            _userRegisterLogRepository = userRegisterLogRepository;
            _userAccountRepository = userAccountRepository;
            _accountSolutionRepository = accountSolutionRepository;
            _userLoginLogRepository = userLoginLogRepository;
            _userMainHistoryLogRepository = userMainHistoryLogRepository;
            _userSecurityRepository = userSecurityRepository;
            _userPasswordErrorStatRepository = userPasswordErrorStatRepository;
            _userAccountCreateInfoRepository = userAccountCreateInfoRepository;
            _appBaseService = appBaseService;
            _verifyCodeService = verifyCodeService;
            _thirdPartyService = thirdPartyService;
            _userOldPasswordRepository = userOldPasswordRepository;

            const string nameextend = ":{0}";
            const int lifetime = 864000;
            usernameDomain = CacheDomain.CreateSingleKey<UserId, string>(o => o.Key, GetUserIdByName, null,
                User.CacheRegionLoginName, User.CacheRegionLoginName + nameextend, lifetime);
            mobileDomain = CacheDomain.CreateSingleKey<UserId, string>(o => o.Key, GetUserIdByMobile, null,
                User.CacheRegionLoginMobile, User.CacheRegionLoginMobile + nameextend, lifetime);
            emailDomain = CacheDomain.CreateSingleKey<UserId, string>(o => o.Key, GetUserIdByEmail, null,
                User.CacheRegionLoginEmail, User.CacheRegionLoginEmail + nameextend, lifetime);
            idCardDomain = CacheDomain.CreateSingleKey<UserId, string>(o => o.Key, GetUserIdByIdCard, null,
                User.CacheRegionIDCard, User.CacheRegionIDCard + nameextend, lifetime);
        }

        private UserId GetUserIdByName(string key)
        {
            var user = _userRepository.FindOne(_userRepository.CreateSpecification().Where(u => u.LoginName == key));
            return user != null ? new UserId { Id = user.Id, Key = user.LoginName } : null;
        }

        private UserId GetUserIdByMobile(string key)
        {
            var user = _userRepository.FindOne(_userRepository.CreateSpecification().Where(u => u.LoginMobile == key));
            return user != null ? new UserId { Id = user.Id, Key = user.LoginName } : null;
        }

        private UserId GetUserIdByEmail(string key)
        {
            var user = _userRepository.FindOne(_userRepository.CreateSpecification().Where(u => u.LoginEmail == key));
            return user != null ? new UserId { Id = user.Id, Key = user.LoginName } : null;
        }

        private UserId GetUserIdByIdCard(string key)
        {
            var user = _userRepository.FindOne(_userRepository.CreateSpecification().Where(u => u.IDCard == key));
            return user != null ? new UserId { Id = user.Id, Key = user.LoginName } : null;
        }

        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public UserCode ValidLoginMobile(string mobile)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => string.IsNullOrWhiteSpace(mobile) ? UserCode.EmptyMobile : UserCode.Success);
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(mobile));
            validor.AppendValidAndSet(() => LoginMobileExists(mobile));

            return validor.Valid();
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserCode ValidLoginEmail(string email)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => string.IsNullOrWhiteSpace(email) ? UserCode.EmptyEmail : UserCode.Success);
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(email));
            validor.AppendValidAndSet(() => LoginEmailExists(email));

            return validor.Valid();
        }

        /// <summary>
        /// 验证身用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserCode ValidUserName(string userName)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => string.IsNullOrWhiteSpace(userName) ? UserCode.EmptyUserName : UserCode.Success);
            validor.AppendValidAndSet(() => UserValid.ValidUserNameFormat(userName));
            validor.AppendValidAndSet(() => UserNameExists(userName));

            return validor.Valid();
        }

        /// <summary>
        /// 验证身份证件
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public UserCode ValidIdCard(string idCard)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => string.IsNullOrWhiteSpace(idCard) ? UserCode.EmptyIDCard : UserCode.Success);
            validor.AppendValidAndSet(() => UserValid.ValidIDCardFormat(idCard));
            validor.AppendValidAndSet(() => IDCardExists(idCard));

            return validor.Valid();
        }



        /// <summary>
        /// 用户否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="mobile">手机号</param>
        /// <param name="email">邮箱</param>
        /// <param name="idCard">身份证件</param>
        /// <returns></returns>
        private bool UserExists(string userName = null, string mobile = null, string email = null, string idCard = null)
        {
            if (userName == null && mobile == null && email == null && idCard == null)
                return false;

            var spec = _userRepository.CreateSpecification().Where(u => u.Id == 0);
            spec = GetUserExistsConditionSpecification(spec, userName, mobile, email, idCard);
            return _userRepository.Count(spec) > 0;
        }

        /// <summary>
        /// 登录手机号是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private UserCode LoginMobileExists(string mobile)
        {
            return UserExists(mobile: mobile) ? UserCode.RepeatedMobile : UserCode.Success;
        }

        /// <summary>
        /// 登录邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private UserCode LoginEmailExists(string email)
        {
            return UserExists(email: email) ? UserCode.RepeatedEmail : UserCode.Success;
        }

        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private UserCode UserNameExists(string userName)
        {
            return UserExists(userName) ? UserCode.RepeatedUserName : UserCode.Success;
        }

        /// <summary>
        /// 身份证件是否存在
        /// </summary>
        /// <param name="idCard">身份证件</param>
        /// <returns></returns>
        private UserCode IDCardExists(string idCard)
        {
            return UserExists(idCard: idCard) ? UserCode.RepeatedIDCard : UserCode.Success;
        }

        /// <summary>
        /// 帐号映射是否存在
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public UserCode UserAccountExists(Solution solution, string account)
        {

            if (solution != null)
            {
                var spec = _userAccountRepository.CreateSpecification()
                    .Where(a => a.SolutionId == solution.Id && a.Account == account);
                if (_userAccountRepository.Count(spec) > 0)
                {
                    return UserCode.RepeatedAccount;
                }
                return UserCode.Success;
            }
            return UserCode.NotExistsSolution;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(long userId)
        {
            return _userRepository.Get(userId);
        }

        public UserSecurity GetUserSecurity(long userId)
        {
            return _userSecurityRepository.Get(userId);
        }


        private IConditionSpecification<User> GetUserExistsConditionSpecification(IConditionSpecification<User> spec, string userName = null, string mobile = null, string email = null, string idCard = null)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                userName = userName.Trim().ToLower();
                spec = spec.Or(u => u.LoginName == userName);
            }
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                mobile = mobile.Trim();
                spec = spec.Or(u => u.LoginMobile == mobile);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                email = email.Trim().ToLower();
                spec = spec.Or(u => u.LoginEmail == email);
            }
            if (!string.IsNullOrWhiteSpace(idCard))
            {
                idCard = UserValid.GetIdCardStoreStr(idCard);
                spec = spec.Or(u => u.IDCard == idCard);
            }
            return spec;
        }

        private void SetUserMainVal(string userName, string email, string mobile, string idCard, User user, Validor<UserCode> validor)
        {
            validor.AppendValidAndSet(() => UserValid.ValidUserNameFormat(userName));

            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(email));

            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(mobile));

            validor.AppendValidAndSet(() => UserValid.ValidIDCardFormat(idCard));

            user.LoginName = SetValNotEquals(userName, user.LoginName, () => validor.AppendValidAndSet(() => UserNameExists(userName)));

            user.LoginEmail = SetValNotEquals(email, user.LoginEmail, () => validor.AppendValidAndSet(() => LoginEmailExists(email)));

            user.LoginMobile = SetValNotEquals(mobile, user.LoginMobile, () => validor.AppendValidAndSet(() => LoginMobileExists(mobile)));

            user.SetIDCard(SetIdCardNotEquals(idCard, user, () => validor.AppendValidAndSet(() => IDCardExists(idCard))));
        }


        public ResultWrapper<UserCode, LoginOrRegisterResult> TryRegister(UserRegister register, int appId, int terminalCode)
        {
            var result = new LoginOrRegisterResult();
            string msg = null;
            var user = new User(NetworkTime.Now);
            var hasAccount = false;
            var mobileMode = !String.IsNullOrWhiteSpace(register.Mobile);
            var validor = new Validor<UserCode>(UserCode.Success);
            var verifyCodeAppearRule = VerifyCodeAppearRule.Need;
            validor.AppendValidAndSet(() =>
            {
                if (mobileMode)
                {
                    var mobileverifycode = new RegisterMobileVerifyCode(register.Mobile, register.SmsVerifyCode);
                    var usercode = _verifyCodeService.SmsVerifyCodeEffective<RegisterMobileVerifyCode>(register.Mobile,
                        register.SmsVerifyCode);
                    if (usercode != UserCode.Success)
                        return usercode;
                    _verifyCodeService.RemoveMobileVerifyCode(mobileverifycode);
                }
                else
                {
                    verifyCodeAppearRule = _appBaseService.GetRegisterVerifyCodeAppearRule(appId);
                    result.NeedPicVerifyCode = (!String.IsNullOrEmpty(register.SessionId) && !String.IsNullOrEmpty(register.VerifyCode))
                        || _verifyCodeService.JudgeIfNeedPicVerifyCode(verifyCodeAppearRule, register.IpAddressInt, PicVerifyIpStatType.RegisterSuccess);

                    if (result.NeedPicVerifyCode)
                    {
                        if (!_verifyCodeService.ValidVerifyCode(new PicVerifyCode(register.SessionId, register.VerifyCode), true))
                            return UserCode.InvalidVerifyCode;
                    }
                }
                return UserCode.Success;
            });


            SetUserMainVal(register.UserName, register.Email, register.Mobile, register.IDCard, user, validor);


            if (ExistsOneLoginWay(user) == UserCode.Success)
                hasAccount = true;
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(register.Password, pwd => { register.Password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(register.Password), () => user.SetPassword(register.Password));

            validor.AppendValidAndSet(() => hasAccount ? UserCode.Success : UserCode.NoIdentity);

            var code = validor.Valid();
            if (code == UserCode.Success)
            {
                if (mobileMode)
                    user.IsVerifyLoginMobile = true;

                user.Status = UserStatus.Ready;
                var rs = TryCreateUser(user, null, appId, terminalCode, register.IpAddressInt,
                    RegisterMode.Register);

                if (rs.Code == UserCode.Success)
                {

                    if (user.LoginEmail != null)
                    {
                        if (!register.EmailActiveTemplate.IsNullOrWhiteSpace())
                        {
                            try
                            {
                                SendLoginEmailVerifyMail(user, register.EmailActiveTemplate,
                                register.EmailActiveSubject, register.EmailActiveDisplayName);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error("注册发送邮箱激活邮件，发送失败", ex);
                            }

                        }
                    }
                    result.UserId = user.Id;
                    result.UserDisplayName = user.DisplayName;
                    result.UserDisplayNameWithCover = user.DisplayNameWithCover;
                    if (!mobileMode)
                    {
                        if (verifyCodeAppearRule == VerifyCodeAppearRule.Judge)
                        {
                            _verifyCodeService.RecordPicVerifyIpStat(register.IpAddressInt,
                                PicVerifyIpStatType.RegisterSuccess);
                        }
                    }
                    else
                    {
                        var mobileVerifyCode = new LoginMobileVerifyCode(register.Mobile, register.SmsVerifyCode);
                        _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
                    }
                }
                else
                {
                    code = rs.Code;
                    msg = rs.Message;
                }

            }

            return new ResultWrapper<UserCode, LoginOrRegisterResult>(code, result, msg);
        }


        public IList<ImportUserResult> ImportUsers(IEnumerable<ImportUser> accounts, int appId, int terminalCode, long ipAddress)
        {
            var importResultList = new List<ImportUserResult>();
            foreach (var import in accounts)
            {
                var hasAccount = false;
                var user = new User(NetworkTime.Now);
                var validor = new Validor<UserCode>(UserCode.Success);
                var importResult = new ImportUserResult { ImportInfo = import };

                var importData = import;
                validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(importData.Password, pwd => { importData.Password = pwd; }));
                validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(importData.Password));

                SetUserMainVal(import.UserName, import.Email, import.Mobile, import.IDCard, user, validor);

                if (ExistsOneLoginWay(user) == UserCode.Success)
                    hasAccount = true;

                validor.AppendValidAndSet(() => hasAccount ? UserCode.Success : UserCode.NoIdentity);

                var validCode = validor.Valid();

                if (validCode != UserCode.Success)
                {
                    importResult.ErrorCode = validCode;
                    importResult.Error = importResult.ErrorCode.GetDescription();
                    importResultList.Add(importResult);
                    continue;
                }

                user.Status = UserStatus.Ready;
                user.SetPassword(importData.Password);
                var rs = TryCreateUser(user, null, appId, terminalCode, ipAddress);
                if (rs.Code == UserCode.Success)
                {
                    importResult.UserId = user.Id;
                }
                else
                {
                    importResult.ErrorCode = rs.Code;
                    importResult.Error = rs.Message ?? rs.Code.GetDescription();
                }

                importResultList.Add(importResult);
            }

            return importResultList;
        }

        public IList<ImportCustomAccountResult> ImportCustomAccounts(IEnumerable<ImportAccount> accounts, Solution solution
            , int appId, int terminalCode, long ipAddress)
        {
            var importResultList = new List<ImportCustomAccountResult>();
            foreach (var import in accounts)
            {
                var validor = new Validor<UserCode>(UserCode.Success);
                var importResult = new ImportCustomAccountResult { ImportInfo = import, Status = ImportResultStatus.Error };

                var importData = import;
                validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(importData.Password, pwd => { importData.Password = pwd; }));
                validor.AppendValidAndSet(() => solution.VaildPassword(importData.Password));
                validor.AppendValidAndSet(() => solution.VaildAccountName(importData.Account));
                validor.AppendValidAndSet(() => UserAccountExists(solution, importData.Account));

                var validCode = validor.Valid();

                if (validCode != UserCode.Success)
                {
                    importResult.ErrorCode = validCode;
                    if (validCode == UserCode.InvalidCustomAccount)
                        importResult.Error = solution.AccountNameRuleDesc;
                    else if (validCode == UserCode.InvalidCustomPasswordLength)
                        importResult.Error = solution.PasswordRuleDesc;
                    else
                        importResult.Error = importResult.ErrorCode.GetDescription();

                    importResultList.Add(importResult);
                    continue;
                }
                var user = new User(NetworkTime.Now);
                var userAccount = new UserAccount(user.CreateTime)
                {
                    SolutionId = solution.Id,
                    Account = importData.Account,
                    AppId = appId,
                    Status = AccountStatus.Ready,
                };
                userAccount.SetPassword(importData.Password);
                user.Status = UserStatus.Ready;
                var rs = TryCreateUser(user, userAccount, appId, terminalCode, ipAddress);
                if (rs.Code == UserCode.Success)
                {
                    importResult.UserId = user.Id;
                    importResult.Status = ImportResultStatus.New;
                }
                else
                {
                    importResult.ErrorCode = rs.Code;
                    importResult.Error = rs.Message ?? rs.Code.GetDescription();
                }

                importResultList.Add(importResult);
            }

            return importResultList;
        }

        public ResultWrapper<UserCode, User> TryCreateUser(User user, UserAccount userAccount, int appId, int terminalCode, long ipAddress, RegisterMode registerMode = RegisterMode.Import)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            try
            {
                using (var tran = new TransactionScope())
                {
                    _userRepository.Create(user);

                    if (user.HasLoginName)
                    {
                        InsertUserMainHistoryLog(user.Id, null, user.LoginName, UserHistoryValueType.UserName);
                    }
                    if (user.HasIDCard)
                    {
                        InsertUserMainHistoryLog(user.Id, null, user.IDCard, UserHistoryValueType.IDCard);
                    }

                    if (user.HasLoginMobile)
                    {
                        InsertUserMainHistoryLog(user.Id, null, user.LoginMobile, UserHistoryValueType.LoginMobile);
                    }

                    if (user.HasLoginEmail)
                    {
                        InsertUserMainHistoryLog(user.Id, null, user.LoginEmail, UserHistoryValueType.LoginEmail);
                    }
                    if (!user.Password.IsNullOrWhiteSpace())
                    {
                        InsertUserMainHistoryLog(user.Id, null, user.Password, UserHistoryValueType.Password);
                    }


                    if (userAccount != null)
                    {
                        userAccount.UpdateTime = user.UpdateTime;
                        userAccount.UserId = user.Id;
                        userAccount.Status = AccountStatus.Ready;
                        _userAccountRepository.Create(userAccount);
                        InsertUserMainHistoryAccountlog(user.Id, null, userAccount);
                        var userAccountCreateInfo = new UserAccountCreateInfo
                        {
                            AppId = appId,
                            TerminalCode = terminalCode,
                            RegisterMode = registerMode,
                            CreateTime = user.UpdateTime,
                            IpAddress = ipAddress,
                            UserAccountId = userAccount.Id,
                            IpCityId = _appBaseService.GetCityIdByIp(ipAddress)
                        };
                        _userAccountCreateInfoRepository.Create(userAccountCreateInfo);
                    }

                    var userRegisterLog = new UserRegisterLog
                    {
                        UserId = user.Id,
                        RegisterMode = registerMode,
                        RegisterAppId = appId,
                        TerminalCode = terminalCode,
                        IpAddress = ipAddress,
                        RegisterTime = user.UpdateTime,
                        IpCityId = _appBaseService.GetCityIdByIp(ipAddress)
                    };
                    _userRegisterLogRepository.Create(userRegisterLog);

                    var userSecurity = new UserSecurity(user.Id);
                    _userSecurityRepository.Create(userSecurity);

                    tran.Complate();
                }
            }
            catch (Exception ex)
            {
                var mysqlex = ex.InnerException as DbException;
                if (mysqlex != null)
                {
                    var msg = mysqlex.Message;
                    var code = UserCode.ApiException;
                    if (msg.Contains("Uk_LoginName"))
                    {
                        code = UserCode.RepeatedUserName;
                    }
                    else if (msg.Contains("Uk_LoginEmail"))
                    {
                        code = UserCode.RepeatedEmail;
                    }
                    else if (msg.Contains("Uk_LoginMobile"))
                    {
                        code = UserCode.RepeatedMobile;
                    }
                    else if (msg.Contains("Uk_LoginIDCard"))
                    {
                        code = UserCode.RepeatedIDCard;
                    }
                    else if (msg.Contains("UK_AccountSolutionIdAndValue"))
                    {
                        code = UserCode.RepeatedAccount;
                    }

                    return new ResultWrapper<UserCode, User>(code, user, code == UserCode.ApiException ? mysqlex.Message : code.GetDescription());
                }
                return new ResultWrapper<UserCode, User>(UserCode.ApiException, user, ex.Message);
            }
            return new ResultWrapper<UserCode, User>(UserCode.Success, user);
        }

        public void SetWorkbenchVal(int appId, int terminalCode, long ipAddress)
        {
            Workbench.Current.Items[AucLogConst.auclogIp] = ipAddress;
            Workbench.Current.Items[AucLogConst.auclogAppId] = appId;
            Workbench.Current.Items[AucLogConst.auclogTerminalCode] = terminalCode;
        }

        public ResultWrapper<UserCode, UserAccount> TryThirdLogin(string openId, string nickName, Solution solution
           , long ipAddress
           , int appId
           , int terminalCode)
        {
            if (solution != null)
            {
                User user = null;
                var userAccount = GetUserAccountByAccount(openId, solution.Id);
                if (solution.Type == SolutionType.Uap)
                {

                    if (userAccount == null)
                    {
                        user = new User(NetworkTime.Now);
                        userAccount = new UserAccount(user.CreateTime)
                        {
                            AppId = appId,
                            Account = openId,
                            NickName = nickName,
                            SolutionId = solution.Id,
                            Status = AccountStatus.Ready,
                        };
                        var rs = TryCreateUser(user, userAccount, appId, terminalCode, ipAddress,
                            RegisterMode.Register);
                        if (rs.Code != UserCode.Success)
                            return new ResultWrapper<UserCode, UserAccount>(rs.Code);
                    }
                }


                if (userAccount != null)
                {
                    var now = NetworkTime.Now;
                    if (user == null)
                        user = _userRepository.Get(userAccount.UserId);

                    var validor = new Validor<UserCode>(UserCode.Success);
                    validor.AppendValidAndSet(() =>
                    {
                        if (user.Status == UserStatus.Disabled)
                            return UserCode.UserDisable;

                        if (user.Status == UserStatus.Frozen && user.FrozenExpire > now)
                            return UserCode.UserFrozen;

                        using (var tran = new TransactionScope())
                        {

                            if (user.Status == UserStatus.Frozen)
                            {
                                user.Status = UserStatus.Ready;
                                _userRepository.Update(user);
                            }
                            userAccount.NickName = nickName;
                            _userAccountRepository.Update(userAccount);

                            var userLoginLog = new UserLoginLog
                            {
                                UserId = user.Id,
                                AppId = appId,
                                IdentityType = UserIdentityType.Solution,
                                SolutionId = solution.Id,
                                TerminalCode = terminalCode,
                                IpAddress = ipAddress,
                                LoginTime = now,
                                IpCityId = _appBaseService.GetCityIdByIp(ipAddress)
                            };

                            _userLoginLogRepository.Create(userLoginLog);
                            tran.Complate();

                        }


                        return UserCode.Success;

                    });
                    return new ResultWrapper<UserCode, UserAccount>(validor.Valid(), userAccount);
                }
            }
            return new ResultWrapper<UserCode, UserAccount>(UserCode.InvalidUser);
        }

        public ResultWrapper<UserCode, LoginOrRegisterResult> TryLogin(string identity
           , string password
           , long ipAddress
           , int appId
           , int terminalCode
           , string sessionId
           , string verifyCode)
        {
            var result = new LoginOrRegisterResult();
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            var verifyCodeAppearRule = _appBaseService.GetLoginVerifyCodeAppearRule(appId);
            result.DisplayFormat = _appBaseService.GetPicVerifyCodeDisplayFormat(appId);

            var needPicVerifyCode = (!String.IsNullOrEmpty(sessionId) && !String.IsNullOrEmpty(verifyCode))
                        || _verifyCodeService.JudgeIfNeedPicVerifyCode(verifyCodeAppearRule, ipAddress, PicVerifyIpStatType.LogonFailure);
            result.NeedPicVerifyCode = needPicVerifyCode;

            var validor = new Validor<UserCode>(UserCode.Success);

            if (needPicVerifyCode)
            {
                validor.AppendValidAndSet(() => _verifyCodeService.ValidVerifyCode(new PicVerifyCode(sessionId, verifyCode), true)
                    ? UserCode.Success
                    : UserCode.InvalidVerifyCode);
            }

            var now = NetworkTime.Now;

            var rs = GetUserByIdentity(identity);
            var user = rs.Data;

            validor.AppendValidAndSet(() => UserStatusTest(user, now));
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));

            validor.AppendValidAndSet(() =>
            {
                if (string.IsNullOrEmpty(user.Password))
                {
                    var userOldPassword = _userOldPasswordRepository.Get(ShardParams.Empty, user.Id);
                    if (userOldPassword != null)
                    {
                        var ePwd = AvalonPassportCoder.EncryptPassword(password);
                        if (string.Equals(ePwd, userOldPassword.Password, StringComparison.OrdinalIgnoreCase))
                        {
                            user.SetPassword(password);
                            _userRepository.Update(user);
                        }
                    }
                }

                if (user.EqualsPassword(password))
                {
                    if (user.Status == UserStatus.Frozen)
                    {
                        user.Status = UserStatus.Ready;
                        _userRepository.Update(user);
                    }

                    var userLoginLog = new UserLoginLog { UserId = user.Id, AppId = appId, IdentityType = rs.Code, TerminalCode = terminalCode, IpAddress = ipAddress, LoginTime = now, IpCityId = _appBaseService.GetCityIdByIp(ipAddress) };
                    _userLoginLogRepository.Create(userLoginLog);
                    result.UserDisplayName = user.DisplayName;
                    result.UserDisplayNameWithCover = user.DisplayNameWithCover;
                    result.UserId = user.Id;
                }
                else
                {
                    return UserFrozen(user, now);
                }

                if (verifyCodeAppearRule == VerifyCodeAppearRule.Judge)
                {
                    _verifyCodeService.RemovePicVerifyIpStat(ipAddress, PicVerifyIpStatType.LogonFailure);
                }

                return UserCode.Success;
            });

            var code = validor.Valid();

            if (code != UserCode.Success)
            {
                if (verifyCodeAppearRule == VerifyCodeAppearRule.Judge)
                {
                    _verifyCodeService.RecordPicVerifyIpStat(ipAddress, PicVerifyIpStatType.LogonFailure);
                    if (!result.NeedPicVerifyCode)
                    {
                        result.NeedPicVerifyCode =
                            _verifyCodeService.JudgeIfNeedPicVerifyCode(verifyCodeAppearRule,
                                ipAddress, PicVerifyIpStatType.LogonFailure);
                    }
                }
            }

            return new ResultWrapper<UserCode, LoginOrRegisterResult>(code, result);
        }


        public ResultWrapper<UserCode, UserAccount> ThirdLoginBind(
              string openId
            , string nickName
            , Solution solution
            , string identity
            , string password
            , long ipAddress
            , int appId
            , int terminalCode
            )
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            UserAccount outUserAccount = null;
            var now = NetworkTime.Now;
            var validor = new Validor<UserCode>(UserCode.Success);
            var rs = GetUserByIdentity(identity);
            var user = rs.Data;

            validor.AppendValidAndSet(() => UserStatusTest(user, now));
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(password));
            validor.AppendValidAndSet(() =>
                {
                    if (user.EqualsPassword(password))
                    {
                        if (user.Status == UserStatus.Frozen)
                        {
                            user.Status = UserStatus.Ready;
                            _userRepository.Update(user);
                        }

                    }
                    else
                    {
                        return UserFrozen(user, now);
                    }

                    return UserCode.Success;
                });

            validor.AppendValidAndSet(() => VaildUserBindAccount(user.Id, solution.Id));

            validor.AppendValidAndSet(() =>
            {
                var userAccount = GetUserAccountByAccount(openId, solution.Id);
                if (userAccount != null)
                {
                    if (userAccount.UserId != user.Id)
                        return UserCode.RepeatedAccount;

                    return UserCode.Success;
                }


                userAccount = new UserAccount(NetworkTime.Now)
                {
                    UserId = user.Id,
                    Account = openId,
                    SolutionId = solution.Id,
                    Status = AccountStatus.Ready,
                    UpdateTime = now,
                    FrozenExpire = NetworkTime.Null,
                    NickName = nickName,
                };
                _userAccountRepository.Create(userAccount);
                InsertUserMainHistoryAccountlog(user.Id, null, userAccount);
                var userAccountCreateInfo = new UserAccountCreateInfo
                {
                    AppId = appId,
                    TerminalCode = terminalCode,
                    RegisterMode = RegisterMode.Register,
                    CreateTime = now,
                    IpAddress = ipAddress,
                    UserAccountId = userAccount.Id,
                    IpCityId = _appBaseService.GetCityIdByIp(ipAddress)
                };
                _userAccountCreateInfoRepository.Create(userAccountCreateInfo);

                outUserAccount = userAccount;
                return UserCode.Success;
            });


            var code = validor.Valid();


            return new ResultWrapper<UserCode, UserAccount>(code, outUserAccount);
        }

        public ResultWrapper<UserCode, UserAccount> ThirdRegisterBind(string openId, string nickName, Solution solution
            , string userName, string idCard, string password
            , string email, string emailActiveSubject, string emailActiveTemplate, string emailActiveDisplayName
            , int appId, int terminalCode, long ipAddress)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);

            var now = NetworkTime.Now;
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => UserAccountExists(solution, openId));

            var user = new User(NetworkTime.Now);
            UserAccount outUserAccount = null;
            var hasAccount = false;

            SetUserMainVal(userName, email, null, idCard, user, validor);

            if (ExistsOneLoginWay(user) == UserCode.Success)
                hasAccount = true;

            if (hasAccount)
            {
                validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
                validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(password), () => user.SetPassword(password));
            }

            validor.AppendValidAndSet(() =>
            {
                outUserAccount = new UserAccount(NetworkTime.Now)
                {
                    Account = openId,
                    SolutionId = solution.Id,
                    Status = AccountStatus.Ready,
                    UpdateTime = now,
                    FrozenExpire = NetworkTime.Null,
                    NickName = nickName,
                };
                user.Status = UserStatus.Ready;
                var rs = TryCreateUser(user, outUserAccount, appId, terminalCode, ipAddress,
                    RegisterMode.Register);

                if (user.LoginEmail != null && !emailActiveTemplate.IsNullOrEmpty())
                {
                    try
                    {
                        SendLoginEmailVerifyMail(user, emailActiveTemplate, emailActiveSubject, emailActiveDisplayName);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("注册发送邮箱激活邮件，发送失败", ex);
                    }

                }

                return rs.Code;
            });
            var code = validor.Valid();

            return new ResultWrapper<UserCode, UserAccount>(code, outUserAccount);
        }


        private UserCode UserStatusTest(User user, DateTime now)
        {
            if (user == null)
                return UserCode.AccountOrPasswordError;

            if (user.Status == UserStatus.Disabled)
                return UserCode.UserDisable;

            if (user.Status == UserStatus.Frozen && user.FrozenExpire > now)
                return UserCode.UserFrozen;

            return UserCode.Success;
        }

        private UserCode UserFrozen(User user, DateTime now)
        {
            _userPasswordErrorStatRepository.Create(new UserPasswordErrorStat
                {
                    UserId = user.Id,
                    ExpireTime = now.AddMinutes(AucConfig.UserPasswordErrorStatExpireMinutes)
                });
            var errorSpec = _userPasswordErrorStatRepository.CreateSpecification()
                .Where(s => s.UserId == user.Id && s.ExpireTime > now);
            var errorCount = _userPasswordErrorStatRepository.Count(errorSpec);
            if (errorCount < AucConfig.UserPasswordErrorMaxCount)
                return UserCode.AccountOrPasswordError;

            user.FrozenExpire = now.AddMinutes(AucConfig.UserPasswordErrorAutoFrozenMinutes);
            user.Status = UserStatus.Frozen;
            _userRepository.Update(user);

            return UserCode.UserFrozen;
        }

        private UserCode UserAccountFrozen(UserAccount userAccount, DateTime now)
        {
            _userPasswordErrorStatRepository.Create(new UserPasswordErrorStat
            {
                UserId = userAccount.UserId,
                ExpireTime = now.AddMinutes(AucConfig.UserPasswordErrorStatExpireMinutes)
            });
            var errorSpec = _userPasswordErrorStatRepository.CreateSpecification()
                .Where(s => s.UserId == userAccount.UserId && s.ExpireTime > now);
            var errorCount = _userPasswordErrorStatRepository.Count(errorSpec);
            if (errorCount < AucConfig.UserPasswordErrorMaxCount)
                return UserCode.AccountOrPasswordError;

            userAccount.FrozenExpire = now.AddMinutes(AucConfig.UserPasswordErrorAutoFrozenMinutes);
            userAccount.Status = AccountStatus.Frozen;
            _userAccountRepository.Update(userAccount);

            return UserCode.UserFrozen;
        }

        private UserCode UserAccountStatusTest(UserAccount userAccount, DateTime now)
        {
            if (userAccount == null)
                return UserCode.AccountOrPasswordError;

            if (userAccount.Status == AccountStatus.Disabled)
                return UserCode.UserDisable;

            if (userAccount.Status == AccountStatus.Frozen && userAccount.FrozenExpire > now)
                return UserCode.UserFrozen;

            return UserCode.Success;
        }

        /// <summary>
        /// 获取帐号方案
        /// </summary>
        /// <param name="solutionCode">方案名称</param>
        /// <returns></returns>
        public Solution GetSolutionByCode(string solutionCode)
        {
            if (string.IsNullOrWhiteSpace(solutionCode))
                return null;
            var spec = _accountSolutionRepository.CreateSpecification().Where(o => o.Code == solutionCode);
            return _accountSolutionRepository.Cache(Solution.CacheRegionCode + "{0}", solutionCode)
                .Depend<Solution>(Solution.CacheRegionCode, solutionCode).Proxy().FindOne(spec);
        }

        /// <summary>
        /// 获取自定义帐号绑定信息
        /// </summary>
        /// <param name="account">自定义帐号</param>
        /// <param name="solutionId">帐号方案标识</param>
        /// <returns></returns>
        public UserAccount GetUserAccountByAccount(string account, int solutionId)
        {
            var spec = _userAccountRepository.CreateSpecification().Where(o => o.SolutionId == solutionId && o.Account == account);
            return _userAccountRepository.FindOne(spec);
        }

        /// <summary>
        /// 用户修改资料
        /// </summary>
        public ResultWrapper<UserCode, string> ModifyUserSecurity(long userId
            , string password
            , string securityEmail
            , string securityMobile
            , string emailSubject
            , string emailTemplate
            , string smsVerifyCode
            , long ipAddress
            , int appId
            , int terminalCode
            , string emailDisplayName = null)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            var user = _userRepository.Get(userId);
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(password));
            validor.AppendValidAndSet(() => user == null ? UserCode.InvalidUser : UserCode.Success);
            validor.AppendValidAndSet(() => user.EqualsPasswordReturnCode(password));
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(securityMobile));
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(securityEmail));

            var security = _userSecurityRepository.Get(user.Id);
            if (!securityEmail.IsNullOrWhiteSpace())
                validor.AppendValidAndSet(security.IsBindSecurityEmail);
            if (!securityMobile.IsNullOrWhiteSpace())
                validor.AppendValidAndSet(security.IsBindSecurityMobile);
            var hasSendMail = false;
            SetValNotEquals(securityMobile, security.Mobile
                , () => validor.AppendValidAndSet(() => _verifyCodeService.SmsVerifyCodeEffective<SecurityMobileVerifyCode>(securityMobile, smsVerifyCode), () => security.Mobile = securityMobile));

            var code = validor.Valid();
            var verifyCode = string.Empty;
            if (code == UserCode.Success)
            {
                SetValNotEquals(securityEmail, security.Email, () =>
                {
                    security.IsVerifyEmail = false;
                    hasSendMail = true;
                    security.Email = securityEmail;
                });
                _userSecurityRepository.Update(security);
                if (hasSendMail)
                {
                    var rs = SendSecurityEmailVerifyMail(security, emailTemplate, emailSubject, emailDisplayName);
                    verifyCode = rs.Data;
                }

                if (!smsVerifyCode.IsNullOrWhiteSpace())
                {
                    var mobileVerifyCode = new SecurityMobileVerifyCode(securityMobile, smsVerifyCode);
                    _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
                }
            }

            return new ResultWrapper<UserCode, string>(code, data: verifyCode);
        }

        /// <summary>
        /// 用户修改资料(网站用，解绑要发送短信给之前邮箱)
        /// </summary>
        public ResultWrapper<UserCode, string> ModifyUserSecurity(long userId
            , string password
            , string securityEmail
            , string securityMobile
            , string emailSubject
            , string emailSubjectUnbind
            , string emailTemplate
            , string emailTemplateExist
            , string emailTemplateNoExist
            , string smsVerifyCode
            , string smsTemplateExist
            , string smsTemplateNoExist
            , long ipAddress
            , int appId
            , int terminalCode
            , string emailDisplayName = null)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            var user = _userRepository.Get(userId);
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(password));
            validor.AppendValidAndSet(() => user == null ? UserCode.InvalidUser : UserCode.Success);
            validor.AppendValidAndSet(() => user.EqualsPasswordReturnCode(password));
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(securityMobile));
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(securityEmail));

            var security = _userSecurityRepository.Get(user.Id);
            var email = security.Email;
            var mobile = security.Mobile;
            var hasSendMail = false;
            SetValNotEquals(securityMobile, security.Mobile
                , () => validor.AppendValidAndSet(() => _verifyCodeService.SmsVerifyCodeEffective<SecurityMobileVerifyCode>(securityMobile, smsVerifyCode), () => security.Mobile = securityMobile));

            var code = validor.Valid();
            var verifyCode = string.Empty;
            if (code == UserCode.Success)
            {
                SetValNotEquals(securityEmail, security.Email, () =>
                {
                    security.IsVerifyEmail = false;
                    hasSendMail = true;
                    security.Email = securityEmail;
                });
                _userSecurityRepository.Update(security);
                if (hasSendMail)
                {
                    if (!email.IsNullOrWhiteSpace())
                        SendEmailToSecurity(email, emailDisplayName, emailSubjectUnbind, emailTemplateExist,
                            emailTemplateNoExist, ipAddress);
                    var rs = SendSecurityEmailVerifyMail(security, emailTemplate, emailSubject, emailDisplayName);
                    verifyCode = rs.Data;
                }

                if (!smsVerifyCode.IsNullOrWhiteSpace())
                {
                    if (!mobile.IsNullOrWhiteSpace() && securityMobile != mobile)
                        SendSmsToSecurity(mobile, smsTemplateExist, smsTemplateNoExist, ipAddress);
                    var mobileVerifyCode = new SecurityMobileVerifyCode(securityMobile, smsVerifyCode);
                    _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
                }
            }

            return new ResultWrapper<UserCode, string>(code, data: verifyCode);
        }

        /// <summary>
        /// 自定义帐号用户修改资料
        /// </summary>
        public ResultWrapper<UserCode, string> ModifyAccountSecurity(Solution solution, long userId
            , string password
            , string securityEmail
            , string securityMobile
            , string emailSubject
            , string emailTemplate
            , string smsVerifyCode
            , long ipAddress
            , int appId
            , int terminalCode
            , string emailDisplayName = null)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            UserAccount account = null;
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(solution.NeedCustomSolution, () => { account = GetUserAccountByUserId(userId, solution.Id); });
            validor.AppendValidAndSet(() => account == null ? UserCode.InvalidAccount : UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidAccountPasswordFormat(password, solution));
            validor.AppendValidAndSet(() => account.EqualsPasswordReturnCode(password));
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(securityMobile));
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(securityEmail));

            var security = _userSecurityRepository.Get(userId);
            if (!securityEmail.IsNullOrWhiteSpace())
                validor.AppendValidAndSet(security.IsBindSecurityEmail);
            if (!securityMobile.IsNullOrWhiteSpace())
                validor.AppendValidAndSet(security.IsBindSecurityMobile);
            var hasSendMail = false;

            SetValNotEquals(securityMobile, security.Mobile
                , () => validor.AppendValidAndSet(() => _verifyCodeService.SmsVerifyCodeEffective<SolutionSecuritySmsCode>(securityMobile, smsVerifyCode, solution.Id), () => security.Mobile = securityMobile));

            var code = validor.Valid();
            var verifyCode = string.Empty;
            if (code == UserCode.Success)
            {
                SetValNotEquals(securityEmail, security.Email, () =>
                {
                    security.IsVerifyEmail = false;
                    hasSendMail = true;
                    security.Email = securityEmail;
                });
                _userSecurityRepository.Update(security);
                if (hasSendMail)
                {
                    var rs = SendSecurityEmailVerifyMail(security, emailTemplate, emailSubject, emailDisplayName);
                    verifyCode = rs.Data;
                }

                if (!smsVerifyCode.IsNullOrWhiteSpace())
                {
                    var mobileVerifyCode = new SolutionSecuritySmsCode(securityMobile, smsVerifyCode, solution.Id);
                    _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
                }
            }

            return new ResultWrapper<UserCode, string>(code, data: verifyCode);
        }

        /// <summary>
        /// 自定义帐号用户修改资料(网站用，解绑要发送短信给之前邮箱)
        /// </summary>
        public ResultWrapper<UserCode, string> ModifyAccountSecurity(Solution solution, long userId
            , string password
            , string securityEmail
            , string securityMobile
            , string emailSubject
            , string emailSubjectUnbind
            , string emailTemplate
            , string emailTemplateExist
            , string emailTemplateNoExist
            , string smsVerifyCode
            , string smsTemplateExist
            , string smsTemplateNoExist
            , long ipAddress
            , int appId
            , int terminalCode
            , string emailDisplayName = null)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            UserAccount account = null;
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(solution.NeedCustomSolution, () => { account = GetUserAccountByUserId(userId, solution.Id); });
            validor.AppendValidAndSet(() => account == null ? UserCode.InvalidAccount : UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidAccountPasswordFormat(password, solution));
            validor.AppendValidAndSet(() => account.EqualsPasswordReturnCode(password));
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(securityMobile));
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(securityEmail));

            var security = _userSecurityRepository.Get(userId);
            var email = security.Email;
            var mobile = security.Mobile;
            var hasSendMail = false;
            SetValNotEquals(securityEmail, security.Email, () =>
            {
                security.IsVerifyEmail = false;
                hasSendMail = true;
                security.Email = securityEmail;
            });

            SetValNotEquals(securityMobile, security.Mobile
                , () => validor.AppendValidAndSet(() => _verifyCodeService.SmsVerifyCodeEffective<SolutionSecuritySmsCode>(securityMobile, smsVerifyCode, solution.Id), () => security.Mobile = securityMobile));

            var code = validor.Valid();
            var verifyCode = string.Empty;
            if (code == UserCode.Success)
            {
                _userSecurityRepository.Update(security);
                if (hasSendMail)
                {
                    if (!email.IsNullOrWhiteSpace())
                        SendEmailToSecurity(email, emailDisplayName, emailSubjectUnbind, emailTemplateExist,
                            emailTemplateNoExist, ipAddress);
                    var rs = SendSecurityEmailVerifyMail(security, emailTemplate, emailSubject, emailDisplayName);
                    verifyCode = rs.Data;
                }

                if (!smsVerifyCode.IsNullOrWhiteSpace())
                {
                    if (!mobile.IsNullOrWhiteSpace() && securityMobile != mobile)
                        SendSmsToSecurity(mobile, smsTemplateExist, smsTemplateNoExist, ipAddress);
                    var mobileVerifyCode = new SolutionSecuritySmsCode(securityMobile, smsVerifyCode, solution.Id);
                    _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
                }
            }

            return new ResultWrapper<UserCode, string>(code, data: verifyCode);
        }

        /// <summary>
        /// 解除密保
        /// </summary>
        public UserCode UnBindSecurity(long userId
            , string password
            , string securityEmail
            , string securityMobile
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
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(password));
            validor.AppendValidAndSet(() => user == null ? UserCode.InvalidUser : UserCode.Success);
            validor.AppendValidAndSet(() => user.EqualsPasswordReturnCode(password));

            var code = validor.Valid();

            if (code == UserCode.Success)
            {
                var security = _userSecurityRepository.Get(user.Id);
                var hasChange = false;
                if (securityEmail != null)
                {
                    var email = security.Email;
                    _verifyCodeService.RemoveUserEmailVerifyCode(security.UserId, email);
                    security.Email = null;
                    security.IsVerifyEmail = false;
                    hasChange = true;
                    SendEmailToSecurity(email, emailDisplayName, emailSubject, emailTemplateExist, emailTemplateNoExist, ipAddress);
                }

                if (securityMobile != null)
                {
                    var mobile = security.Mobile;
                    security.Mobile = null;
                    hasChange = true;
                    SendSmsToSecurity(mobile, smsTemplateExist, smsTemplateNoExist, ipAddress);
                }

                if (hasChange)
                    _userSecurityRepository.Update(security);
            }

            return code;
        }

        /// <summary>
        /// 自定义帐号用户解除密保
        /// </summary>
        public UserCode UnBindAccountSecurity(Solution solution, long userId
            , string password
            , string securityEmail
            , string securityMobile
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

            UserAccount account = null;
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(solution.NeedCustomSolution, () => { account = GetUserAccountByUserId(userId, solution.Id); });
            validor.AppendValidAndSet(() => account == null ? UserCode.InvalidAccount : UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidAccountPasswordFormat(password, solution));
            validor.AppendValidAndSet(() => account.EqualsPasswordReturnCode(password));

            var code = validor.Valid();

            if (code == UserCode.Success)
            {
                var security = _userSecurityRepository.Get(userId);
                var hasChange = false;
                if (securityEmail != null)
                {
                    var email = security.Email;
                    _verifyCodeService.RemoveUserEmailVerifyCode(security.UserId, email, solution.Code);
                    security.Email = null;
                    security.IsVerifyEmail = false;
                    hasChange = true;
                    SendEmailToSecurity(email, emailDisplayName, emailSubject, emailTemplateExist, emailTemplateNoExist, ipAddress);
                }

                if (securityMobile != null)
                {
                    var mobile = security.Mobile;
                    security.Mobile = null;
                    hasChange = true;
                    SendSmsToSecurity(mobile, smsTemplateExist, smsTemplateNoExist, ipAddress);
                }

                if (hasChange)
                    _userSecurityRepository.Update(security);
            }

            return code;
        }

        //至少需要一种合法的登录方式
        public UserCode ExistsOneLoginWay(User user)
        {
            var allIsEmpty = !user.HasLoginName && !user.HasLoginMobile &&
                             !user.HasLoginEmail && !user.HasIDCard;
            return (allIsEmpty)
                ? UserCode.NeedLeastOneLogonWay
                : UserCode.Success;
        }

        /// <summary>
        /// 计算登陆方式个数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CountLoginWay(User user)
        {
            var count = 0;
            if (user.HasLoginEmail)
                count++;
            if (user.HasLoginMobile)
                count++;
            if (user.HasLoginName)
                count++;
            if (user.HasIDCard)
                count++;
            return count;
        }

        /// <summary>
        /// 修改用户资料--头像
        /// </summary>
        public UserCode ModifyUserInfo(long userId, StoreObject avatarImage)
        {
            var user = _userRepository.Get(userId);
            if (user == null)
            {
                return UserCode.InvalidUser;
            }
            if (avatarImage != null)
            {
                user.AvatarObjectId = avatarImage.Id;
                _userRepository.Update(user);
            }
            return UserCode.Success;
        }

        public UserAccount GetUserAccountByUserId(long userId, int solutionId)
        {
            var spec2 = _userAccountRepository.CreateSpecification();
            spec2 = spec2.Where(u => u.UserId == userId);
            spec2 = spec2.Where(u => u.SolutionId == solutionId);
            return _userAccountRepository.FindOne(spec2);
        }

        /// <summary>
        /// 修改用户资料-标识信息
        /// </summary>
        public UserCode ModifyUserIdentity(long userId
            , string password
            , string userName
            , string mobile
            , string smsVerifyCode
            , string email
            , string emailSubject
            , string emailTemplate
            , string idCard
            , long ipAddress
            , int appId
            , int terminalCode
            , string emailDisplay = null)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            //判断用户名手机邮箱修改时候,如果清空任何一种方式的时候，是否仍然至少会有一种可以登录的方式。
            var user = _userRepository.Get(userId);
            if (user == null)
                return UserCode.InvalidUser;


            if (user.HasIDCard && !string.IsNullOrWhiteSpace(idCard))
            {
                throw new NotImplementedException("用户身份证件不支持修改");
            }


            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            if (user.Password.IsNullOrEmpty())
            {
                validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(password), () => user.SetPassword(password));
            }
            else
                validor.AppendValidAndSet(() => user.EqualsPasswordReturnCode(password));

            validor.AppendValidAndSet(() => UserValid.ValidUserNameFormat(userName));
            validor.AppendValidAndSet(() => UserValid.ValidMobileFormat(mobile));
            validor.AppendValidAndSet(() => UserValid.ValidEmailFormat(email));
            validor.AppendValidAndSet(() => UserValid.ValidIDCardFormat(idCard));

            if (!user.HasIDCard && !idCard.IsNullOrWhiteSpace())
            {
                user.SetIDCard(SetIdCardNotEquals(idCard, user, () => validor.AppendValidAndSet(() => IDCardExists(idCard))));
            }

            SetValNotEquals(userName, user.LoginName,
                () =>
                {
                    validor.AppendValidAndSet(() => UserNameExists(userName));
                    user.LoginName = userName;
                });

            SetValNotEquals(mobile, user.LoginMobile, () =>
             {
                 validor.AppendValidAndSet(() => LoginMobileExists(mobile));
                 validor.AppendValidAndSet(() => _verifyCodeService.SmsVerifyCodeEffective<LoginMobileVerifyCode>(mobile, smsVerifyCode));
                 user.LoginMobile = mobile;
                 user.IsVerifyLoginMobile = true;
             });

            var hasChangeEmail = false;
            SetValNotEquals(email, user.LoginEmail, () =>
            {
                hasChangeEmail = true;
                validor.AppendValidAndSet(() => LoginEmailExists(email));
                _verifyCodeService.RemoveUserEmailVerifyCode(user.Id, user.LoginEmail);
                user.LoginEmail = email;
                user.IsVerifyLoginEmail = false;
            });


            var code = validor.Valid();

            if (code == UserCode.Success)
            {
                using (var tran = new TransactionScope())
                {
                    _userRepository.Update(user);
                    if (!smsVerifyCode.IsNullOrWhiteSpace())
                    {
                        var mobileVerifyCode = new LoginMobileVerifyCode(mobile, smsVerifyCode);
                        _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
                    }
                    tran.Complate();
                }
                //发送邮箱验证
                if (hasChangeEmail)
                {
                    SendGenVerifyCodeMail(userId, email, emailTemplate, emailSubject, emailDisplay);
                }
            }

            return code;
        }

        public UserCode UnBindIdentify(long userId
            , string password
            , string userName
            , string mobile
            , string email
            , string idCard
            , string solution
            , long ipAddress
            , int appId
            , int terminalCode)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            //判断用户名手机邮箱修改时候,如果清空任何一种方式的时候，是否仍然至少会有一种可以登录的方式。
            var user = _userRepository.Get(userId);
            if (user == null)
                return UserCode.InvalidUser;

            Solution solutionModel = null;
            UserAccount userAccountOld = null;

            if (!solution.IsNullOrWhiteSpace())
            {
                solutionModel = GetSolutionByCode(solution);
                if (solutionModel == null)
                    return UserCode.NotExistsSolution;

                if (solutionModel.Type != SolutionType.Custom)
                {
                    throw new NotImplementedException("第三方帐号解除绑定未实现");
                }

                userAccountOld = GetUserAccountByUserId(userId, solutionModel.Id);
            }


            var validor = new Validor<UserCode>(UserCode.Success);

            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => UserValid.ValidPasswordFormat(password));
            validor.AppendValidAndSet(() => user.EqualsPasswordReturnCode(password));

            if (userName != null)
            {
                user.LoginName = null;
            }

            if (mobile != null)
            {
                user.LoginMobile = null;
                user.IsVerifyLoginMobile = false;
            }

            if (email != null)
            {
                _verifyCodeService.RemoveUserEmailVerifyCode(user.Id, user.LoginEmail, "");
                user.LoginEmail = null;
                user.IsVerifyLoginEmail = false;
            }

            if (idCard != null)
            {
                user.SetIDCard(null);
            }

            validor.AppendValidAndSet(() => ExistsOneLoginWay(user));

            var code = validor.Valid();

            if (code == UserCode.Success)
            {
                using (var tran = new TransactionScope())
                {
                    if (userAccountOld != null)
                    {
                        InsertUserMainHistoryAccountlog(userId, userAccountOld, null);
                        _userAccountRepository.Delete(userAccountOld);
                    }
                    _userRepository.Update(user);
                    tran.Complate();
                }
            }
            return code;
        }

        public ResultWrapper<UserCode, string> SendLoginEmailVerifyMail(User user, string emailTemplate, [Optional] string emailSubject, string emailDisplayName = null)
        {
            var verifyCode = string.Empty;
            var code = UserCode.Success;
            if (user == null)
            {
                code = UserCode.InvalidUser;
            }
            else if (user.LoginEmail.IsNullOrWhiteSpace())
            {
                code = UserCode.UserNotExistLoginEmail;
            }
            else if (user.IsVerifyLoginEmail)
            {
                code = UserCode.AlreadyActiveUserEmail;
            }
            else
            {
                verifyCode = SendGenVerifyCodeMail(user.Id, user.LoginEmail, emailTemplate, emailSubject, emailDisplayName).VerifyCode;
            }

            return new ResultWrapper<UserCode, string>(code, data: verifyCode);
        }

        public ResultWrapper<UserCode, string> SendSecurityEmailVerifyMail(UserSecurity userSecurity, string emailTemplate, [Optional] string emailSubject, string emailDisplayName = null)
        {
            var verifyCode = string.Empty;
            var code = UserCode.Success;
            if (userSecurity == null)
            {
                code = UserCode.InvalidUser;
            }
            else if (userSecurity.Email.IsNullOrWhiteSpace())
            {
                code = UserCode.UserNotExistRecoverEmail;
            }
            else if (userSecurity.IsVerifyEmail)
            {
                code = UserCode.AlreadyActiveSecurityEmail;
            }
            else
            {
                var emailSecurityVerifyCode = _verifyCodeService.GenerateSecurityEmailVerifyCode(userSecurity.UserId, new EmailTemplate
                {
                    Email = userSecurity.Email,
                    BodyTemplate = emailTemplate,
                    SenderDispalyName = emailDisplayName,
                    Subject = emailSubject
                });
                verifyCode = emailSecurityVerifyCode.VerifyCode;
            }

            return new ResultWrapper<UserCode, string>(code, data: verifyCode);
        }

        private EmailVerifyCode SendGenVerifyCodeMail(long userId, string email, string emailTemplate,
            string emailSubject, string displayName = null)
        {
            var emailVerifyCode = _verifyCodeService.GenerateEmailVerifyCode(userId, new EmailTemplate
            {
                Email = email,
                BodyTemplate = emailTemplate,
                SenderDispalyName = displayName,
                Subject = emailSubject
            });
            return emailVerifyCode;
        }

        private string SetValNotEquals(string lVal, string rVal, Action action = null)
        {
            if (lVal != null && !string.Equals(rVal, lVal, StringComparison.OrdinalIgnoreCase))
            {
                if (!lVal.IsNullOrWhiteSpace())
                {
                    rVal = lVal;
                    if (action != null)
                        action();
                }
                else
                {
                    rVal = null;
                }
            }
            return rVal;
        }

        private string SetIdCardNotEquals(string idCard, User user, Action action = null)
        {
            string rVal = user.IDCard;
            if (idCard != null && !user.IDCardEquals(idCard))
            {
                if (!idCard.IsNullOrEmpty())
                {
                    rVal = UserValid.GetIdCardStoreStr(idCard);
                    if (action != null)
                        action();
                }
                else
                {
                    rVal = null;
                }
            }
            return rVal;
        }

        /// <summary>
        /// 用户修改资料-激活登录邮箱
        /// </summary>
        public ResultWrapper<UserCode, string> ActivatingUserEmail(string verifyCode)
        {
            var usercode = UserCode.InvalidVerifyCode;
            var email = string.Empty;
            //验证邮箱验证码是否合法,则保存邮箱激活状态
            var emailVerifyKey = EmailVerifyCode.BuildKey(verifyCode);
            var verifyCodeEmail = _verifyCodeService.GetEmailVerifyCode<EmailVerifyCode>(emailVerifyKey);
            if (verifyCodeEmail != null && verifyCodeEmail.UserId > 0)
            {
                var user = _userRepository.Get(verifyCodeEmail.UserId);
                if (user.LoginEmail.IsNullOrWhiteSpace())
                {
                    usercode = UserCode.EmailNonExist;
                }
                else
                {
                    email = user.LoginEmail;
                    var emailVerifyCode = new EmailVerifyCode(user.Id, user.LoginEmail, verifyCode);
                    if (verifyCodeEmail.Valid(emailVerifyCode))
                    {
                        //_verifyCodeService.RemoveVerifyCode(emailVerifyCode);
                        if (user.IsVerifyLoginEmail)
                        {
                            usercode = UserCode.AlreadyActiveUserEmail;
                        }
                        else
                        {
                            user.IsVerifyLoginEmail = true;
                            _userRepository.Update(user);
                            usercode = UserCode.Success;
                        }
                    }
                }
            }

            return new ResultWrapper<UserCode, string>(usercode, data: email);
        }

        /// <summary>
        /// 激活密保邮箱
        /// </summary>
        public UserCode ActivatingUserSecurityEmail(string verifyCode)
        {
            var emailVerifyKey = SecurityEmailVerifyCode.BuildSecurityKey(verifyCode);
            var securityEmailVerifyCode = _verifyCodeService.GetEmailVerifyCode<SecurityEmailVerifyCode>(emailVerifyKey);
            if (securityEmailVerifyCode != null && securityEmailVerifyCode.UserId > 0)
            {
                var userSecurity = _userSecurityRepository.Get(securityEmailVerifyCode.UserId);
                if (userSecurity.Email.IsNullOrWhiteSpace())
                    return UserCode.EmailNonExist;
                var emailVerifyCode = new SecurityEmailVerifyCode(userSecurity.UserId, userSecurity.Email, verifyCode);
                if (securityEmailVerifyCode.Valid(emailVerifyCode))
                {
                    //_verifyCodeService.RemoveVerifyCode(emailVerifyCode);
                    if (userSecurity.IsVerifyEmail)
                        return UserCode.AlreadyActiveSecurityEmail;

                    userSecurity.IsVerifyEmail = true;
                    _userSecurityRepository.Update(userSecurity);
                    return UserCode.Success;
                }
            }

            return UserCode.InvalidVerifyCode;
        }

        /// <summary>
        /// 激活登陆手机
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserCode ActivatingUserMobile(string verifyCode, User user)
        {
            var security = _userSecurityRepository.Get(user.Id);
            var mobile = !string.IsNullOrWhiteSpace(security.Mobile) ? security.Mobile : user.LoginMobile;
            var mobileVerifyCode = new LoginMobileVerifyCode(mobile, verifyCode);
            var code = _verifyCodeService.SmsVerifyCodeEffective<LoginMobileVerifyCode>(mobile, verifyCode);
            if (code != UserCode.Success)
                return code;
            //验证手机验证码是否合法
            user.IsVerifyLoginMobile = true;
            _userRepository.Update(user);
            _verifyCodeService.RemoveMobileVerifyCode(mobileVerifyCode);
            return UserCode.Success;
        }

        public IList<AccountResult> GetAccountList(long userId)
        {
            var rsList = new List<AccountResult>();
            var spec = _userAccountRepository.CreateSpecification().Where(o => o.UserId == userId);
            var userAccountList = _userAccountRepository.FindAll(spec);
            foreach (var userAccount in userAccountList)
            {
                var rsAccount = new AccountResult
                {
                    Id = userAccount.Id,
                    Account = userAccount.Account,
                    Solution = _accountSolutionRepository.Get(ShardParams.Empty, userAccount.SolutionId).Name
                };
                rsList.Add(rsAccount);
            }

            return rsList;
        }

        public IList<User> GetList(IEnumerable<long> ids)
        {
            return _userRepository.GetList(ids);
        }

        public string GetUserForgotMobile(User user)
        {
            var security = _userSecurityRepository.Get(user.Id);
            if (!security.Mobile.IsNullOrWhiteSpace())
            {
                return security.Mobile;
            }
            return !user.LoginMobile.IsNullOrWhiteSpace() ? user.LoginMobile : null;
        }

        public ResultWrapper<UserIdentityType, User> GetUserByIdentity(string identity, Solution solution = null)
        {
            if (identity.IsNullOrWhiteSpace())
                return null;

            User user = null;
            UserIdentityType identityType;
            if (solution == null)
            {
                identityType = UserValid.GetIdentityType(identity);
                var userid = new UserId();
                switch (identityType)
                {
                    case UserIdentityType.Email:
                        userid = emailDomain.GetItem(identity);
                        break;
                    case UserIdentityType.Mobile:
                        userid = mobileDomain.GetItem(identity);
                        break;
                    case UserIdentityType.IDCard:
                        userid = idCardDomain.GetItem(UserValid.GetIdCardStoreStr(identity));
                        break;
                    case UserIdentityType.UserName:
                        userid = usernameDomain.GetItem(identity);
                        break;
                }
                if (identityType != UserIdentityType.Unknown)
                {
                    if (userid != null)
                        user = _userRepository.Get(userid.Id);
                }
            }
            else
            {
                identityType = UserIdentityType.Solution;
                var userAccount = GetUserAccountByAccount(identity, solution.Id);
                if (userAccount != null)
                {
                    user = _userRepository.Get(userAccount.UserId);
                }
            }

            return new ResultWrapper<UserIdentityType, User>(identityType, user);
        }

        public void InterceptorUpdate(User entity)
        {
            _userRepository.SessionEvict(entity);
            User user = _userRepository.FindOne(_userRepository.CreateSpecification().Where(o => o.Id == entity.Id));

            if (!string.Equals(user.LoginName, entity.LoginName))
            {
                InsertUserMainHistoryLog(user.Id, user.LoginName, entity.LoginName, UserHistoryValueType.UserName);
                usernameDomain.RemoveCache(user.LoginName);
            }

            if (!string.Equals(user.Password, entity.Password))
            {
                InsertUserMainHistoryLog(user.Id, user.Password, entity.Password, UserHistoryValueType.Password);
            }

            if (!string.Equals(user.IDCard, entity.IDCard))
            {
                InsertUserMainHistoryLog(user.Id, user.IDCard, entity.IDCard, UserHistoryValueType.IDCard);
                idCardDomain.RemoveCache(user.IDCard);
            }

            if (!string.Equals(user.LoginEmail, entity.LoginEmail))
            {
                InsertUserMainHistoryLog(user.Id, user.LoginEmail, entity.LoginEmail, UserHistoryValueType.LoginEmail);
                emailDomain.RemoveCache(user.LoginEmail);
            }

            if (!string.Equals(user.LoginMobile, entity.LoginMobile))
            {
                InsertUserMainHistoryLog(user.Id, user.LoginMobile, entity.LoginMobile, UserHistoryValueType.LoginMobile);
                mobileDomain.RemoveCache(user.LoginMobile);
            }

            if (!string.Equals(user.Status, entity.Status))
            {
                InsertUserMainHistoryLog(user.Id, user.Status.ToString(), entity.Status.ToString(), UserHistoryValueType.Status);
            }
        }

        public void InterceptorUpdate(UserSecurity entity)
        {
            _userSecurityRepository.SessionEvict(entity);
            UserSecurity userSecurity = _userSecurityRepository.Get(entity.UserId);

            if (!string.Equals(userSecurity.Email, entity.Email, StringComparison.OrdinalIgnoreCase))
            {
                InsertUserMainHistoryLog(userSecurity.UserId, userSecurity.Email, entity.Email, UserHistoryValueType.SecurityEmail);
            }

            if (!string.Equals(userSecurity.Mobile, entity.Mobile, StringComparison.OrdinalIgnoreCase))
            {
                InsertUserMainHistoryLog(userSecurity.UserId, userSecurity.Mobile, entity.Mobile, UserHistoryValueType.SecurityMobile);
            }

        }

        private void InsertUserMainHistoryAccountlog(long userId, UserAccount oldAccount, UserAccount newAccount)
        {
            var oldVal = oldAccount == null
                ? null
                : string.Format("{0}:{1}", oldAccount.SolutionId, oldAccount.Account);

            var newVal = newAccount == null
                ? null
                : string.Format("{0}:{1}", newAccount.SolutionId, newAccount.Account);

            InsertUserMainHistoryLog(userId, oldVal, newVal, UserHistoryValueType.Account);
        }

        private void InsertUserMainHistoryLog(long userId, string oldVal, string newVal, UserHistoryValueType valType)
        {
            var userMainHistoryLog = new UserMainHistoryLog
            {
                UserId = userId,
                OldVal = oldVal,
                NewVal = newVal,
                ValType = valType,
                IpAddress = long.Parse(Workbench.Current.Items[AucLogConst.auclogIp].ToString()),
                AppId = int.Parse(Workbench.Current.Items[AucLogConst.auclogAppId].ToString()),
                TerminalCode = int.Parse(Workbench.Current.Items[AucLogConst.auclogTerminalCode].ToString()),
                IpCityId = _appBaseService.GetCityIdByIp(long.Parse(Workbench.Current.Items[AucLogConst.auclogIp].ToString()))
            };
            _userMainHistoryLogRepository.Create(userMainHistoryLog);
        }


        public UserCode VaildUserBindAccount(long userId, int solutionId)
        {
            var spec = _userAccountRepository.CreateSpecification()
                .Where(u => u.UserId == userId && u.SolutionId == solutionId);
            var count = _userAccountRepository.Count(spec);
            return count > 0 ? UserCode.RepeatedAccount : UserCode.Success;
        }


        public UserCode UserBindThirdAccount(string account, string nickName, Solution solution, long userId, int appId, int terminalCode, long ipAddress, bool valid = false)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            if (valid)
            {
                var validor = new Validor<UserCode>(UserCode.Success);
                validor.AppendValidAndSet(() => VaildUserBindAccount(userId, solution.Id));
                validor.AppendValidAndSet(() => UserAccountExists(solution, account));
                var code = validor.Valid();
                if (code != UserCode.Success)
                    return code;
            }
            var now = NetworkTime.Now;
            var usercode = UserCode.Success;
            try
            {
                using (var tran = new TransactionScope())
                {
                    var userAccount = new UserAccount(NetworkTime.Now)
                    {
                        Account = account,
                        SolutionId = solution.Id,
                        UserId = userId,
                        FrozenExpire = NetworkTime.Null,
                        NickName = nickName,
                        Status = AccountStatus.Ready,
                        UpdateTime = now
                    };
                    _userAccountRepository.Create(userAccount);
                    InsertUserMainHistoryAccountlog(userId, null, userAccount);
                    var userAccountCreateInfo = new UserAccountCreateInfo
                    {
                        AppId = appId,
                        TerminalCode = terminalCode,
                        RegisterMode = RegisterMode.Register,
                        CreateTime = now,
                        IpAddress = ipAddress,
                        UserAccountId = userAccount.Id,
                        IpCityId = _appBaseService.GetCityIdByIp(ipAddress)
                    };
                    _userAccountCreateInfoRepository.Create(userAccountCreateInfo);
                    tran.Complate();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.ToString();
                if (msg.Contains("UK_AccountSolutionIdAndValue"))
                {
                    usercode = UserCode.RepeatedAccount;
                }
            }
            return usercode;
        }

        public void UnBindUserAccount(UserAccount userAccount, Solution solution, long userId, int appId, int terminalCode,
            long ipAddress)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            if (userAccount != null && userAccount.SolutionId == solution.Id && userAccount.UserId == userId)
            {
                using (var tran = new TransactionScope())
                {
                    InsertUserMainHistoryAccountlog(userId, userAccount, null);
                    _userAccountRepository.Delete(userAccount);
                    tran.Complate();
                }
            }
        }

        /// <summary>
        /// 获取所有第三方oauth方式的帐号方案
        /// </summary>
        /// <returns></returns>
        public IList<Solution> GetAllThirdOAuthSolutions()
        {
            var spece = _accountSolutionRepository.CreateSpecification().Where(s =>
                s.Type == SolutionType.QQAccount
             || s.Type == SolutionType.SinaWeibo);
            return _accountSolutionRepository.Cache("noCustom").Proxy().FindAll(spece);
        }

        /// <summary>
        /// 获取某个用户所有的第三方oauth方式的帐号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IDictionary<SolutionType, ThirdAccountResult> GetUserThirdOAuthAccounts(long userId)
        {
            var solutions = GetAllThirdOAuthSolutions();
            var solutionIds = solutions.Select(s => s.Id).ToArray();
            var uaspece =
                _userAccountRepository.CreateSpecification()
                    .Where(ua => ua.UserId == userId && solutionIds.Contains(ua.SolutionId));
            var uaDict = _userAccountRepository.FindAll(uaspece).ToDictionary(u => u.SolutionId);
            var rtDict = new Dictionary<SolutionType, ThirdAccountResult>();
            foreach (var solution in solutions)
            {
                UserAccount userAccount;
                var rtUa = new ThirdAccountResult
                {
                    SolutionCode = solution.Code,
                    SolutionType = solution.Type,
                };
                if (uaDict.TryGetValue(solution.Id, out userAccount))
                {
                    rtUa.Id = userAccount.Id;
                    rtUa.NickName = userAccount.NickName;
                }
                rtDict.Add(solution.Type, rtUa);

            }
            return rtDict;
        }

        /// <summary>
        /// 获取最近十次登陆详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<LoginInfo> GetLoginLogsByUserId(long userId)
        {
            var loginInfos = new List<LoginInfo>();
            var spece = _userLoginLogRepository.CreateSpecification().Where(l => l.UserId == userId).OrderByDescending(o => o.LoginTime).Take(10);
            var userlogins = _userLoginLogRepository.FindAll(spece);
            foreach (var userLoginLog in userlogins)
            {
                var logininfo = new LoginInfo();
                logininfo.LoginTime = userLoginLog.LoginTime.GetYTDHM();
                if (userLoginLog.IdentityType == UserIdentityType.Solution)
                {
                    var solution = _accountSolutionRepository.FindOne(_accountSolutionRepository.CreateSpecification().Where(o => o.Id == userLoginLog.SolutionId));
                    logininfo.LoginType = solution.Name;
                }
                else
                {
                    logininfo.LoginType = userLoginLog.IdentityType.GetDescription();
                }
                logininfo.LoginIpAddress = IpAddress.IpIntoToString(userLoginLog.IpAddress);
                var city = _appBaseService.GetIpCityById(userLoginLog.IpCityId);
                logininfo.LoginLocal = city.CityDisplayNameByProvince(_appBaseService);
                loginInfos.Add(logininfo);
            }

            return loginInfos;
        }

        /// <summary>
        /// 更换密码或解绑时发送邮件给密保邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emaildisplayname"></param>
        /// <param name="emailsubject"></param>
        /// <param name="emailTemplateExist">匹配到城市时发送的模板</param>
        /// <param name="emailTemplateNoExist">没匹配到城市时发送的模板</param>
        /// <param name="ipaddress"></param>
        public void SendEmailToSecurity(string email, string emaildisplayname, string emailsubject,
            string emailTemplateExist, string emailTemplateNoExist, long ipaddress)
        {
            var cityid = _appBaseService.GetCityIdByIp(ipaddress);
            var city = _appBaseService.GetIpCityById(cityid);
            var province = _appBaseService.GetIpProvinceById(city == null ? 0 : city.ProvinceId);
            if (!email.IsNullOrWhiteSpace())
            {
                if (!emailTemplateExist.IsNullOrWhiteSpace())
                {
                    var emailTemplate = city != null
                        ? emailTemplateExist.Replace(LoginCity, province.ProvinceDisplayEmpty() + city.CityDisplayEmpty())
                        : emailTemplateNoExist;

                    if (!string.IsNullOrEmpty(emailTemplate))
                        MsgUtil.SendEmail(email, emailTemplate, emailsubject, emaildisplayname);
                }
            }
        }

        /// <summary>
        /// 更换密码或解绑时发送邮件给密保手机
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="smsTemplateExist">匹配到城市时发送的模板</param>
        /// <param name="smsTemplateNoExist">没匹配到城市时发送的模板</param>
        /// <param name="ipaddress"></param>
        public void SendSmsToSecurity(string mobile, string smsTemplateExist, string smsTemplateNoExist, long ipaddress)
        {
            var cityid = _appBaseService.GetCityIdByIp(ipaddress);
            var city = _appBaseService.GetIpCityById(cityid);
            var province = _appBaseService.GetIpProvinceById(city == null ? 0 : city.ProvinceId);
            if (!mobile.IsNullOrWhiteSpace())
            {
                if (!smsTemplateExist.IsNullOrWhiteSpace())
                {
                    var mobileTemplate = city != null
                        ? smsTemplateExist.Replace(LoginCity, province.ProvinceDisplayEmpty() + city.CityDisplayEmpty())
                        : smsTemplateNoExist;

                    if (!string.IsNullOrEmpty(mobileTemplate))
                        MsgUtil.SendSms(mobile, mobileTemplate);
                }
            }
        }

        /// <summary>
        /// 根据RecoverToken获取用户
        /// </summary>
        /// <param name="recoverToken"></param>
        /// <returns></returns>
        public ResultWrapper<UserCode, KeyValuePair<RecoverToken, User>> GetUserByRecoverToken(string recoverToken)
        {
            var code = UserCode.Success;
            var recoverTokenKey = RecoverToken.BuildKey(recoverToken);
            var recoverTokenInner = _verifyCodeService.GetVerifyCode<RecoverToken>(recoverTokenKey);
            var rt = new KeyValuePair<RecoverToken, User>();
            if (recoverTokenInner.UserId <= 0)
                return new ResultWrapper<UserCode, KeyValuePair<RecoverToken, User>>(UserCode.InvalidRecoverToken, rt);

            var user = GetUser(recoverTokenInner.UserId);
            if (user == null)
            {
                code = UserCode.InvalidRecoverToken;
            }
            else
            {
                rt = new KeyValuePair<RecoverToken, User>(recoverTokenInner, user);
            }

            return new ResultWrapper<UserCode, KeyValuePair<RecoverToken, User>>(code, rt);
        }

        public ResultWrapper<UserCode, LoginOrRegisterResult> TryPasswordSolutionLogin(string identity, string password, string solution,
            long ipAddress, int appId, int terminalCode, string sessionId,
            string verifyCode)
        {
            var verifyCodeAppearRule = VerifyCodeAppearRule.Judge;
            var result = new LoginOrRegisterResult();
            var notEpwd = string.Empty;
            var solutionModel = GetSolutionByCode(solution);

            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(solutionModel.NeedVaildPasswordSolution);


            validor.AppendValidAndSet(() =>
            {
                if (solutionModel.Type == SolutionType.Custom)
                {
                    result.DisplayFormat = solutionModel.PicVerifyCodeDisplayFormat;
                    verifyCodeAppearRule = solutionModel.LoginVerifyCodeAppearRule;
                    result.NeedPicVerifyCode = (!String.IsNullOrEmpty(sessionId) && !String.IsNullOrEmpty(verifyCode))
                                                ||
                                                _verifyCodeService.JudgeIfNeedPicVerifyCode(verifyCodeAppearRule,
                                                    ipAddress, PicVerifyIpStatType.LogonFailure);
                    if (result.NeedPicVerifyCode)
                    {
                        return _verifyCodeService.ValidVerifyCode(new PicVerifyCode(sessionId, verifyCode), true)
                            ? UserCode.Success
                            : UserCode.InvalidVerifyCode;
                    }
                }
                return UserCode.Success;
            });


            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, (pwd) => notEpwd = pwd));
            var now = NetworkTime.Now;
            UserAccount userAccount = null;
            if (solutionModel != null && solutionModel.Type == SolutionType.Custom)
            {
                validor.AppendValidAndSet(() =>
                {
                    userAccount = GetUserAccountByAccount(identity, solutionModel.Id);
                    return UserAccountStatusTest(userAccount, now);
                });
                validor.AppendValidAndSet(() =>
                {
                    if (userAccount != null)
                    {
                        if (userAccount.EqualsPassword(notEpwd))
                        {
                            if (userAccount.Status == AccountStatus.Frozen)
                            {
                                userAccount.Status = AccountStatus.Ready;
                                _userAccountRepository.Update(userAccount);
                            }

                            var userLoginLog = new UserLoginLog
                            {
                                UserId = userAccount.UserId,
                                AppId = appId,
                                IdentityType = UserIdentityType.Solution,
                                TerminalCode = terminalCode,
                                IpAddress = ipAddress,
                                LoginTime = now,
                                IpCityId = _appBaseService.GetCityIdByIp(ipAddress),
                                SolutionId = solutionModel.Id
                            };
                            _userLoginLogRepository.Create(userLoginLog);
                            result.UserDisplayName = userAccount.Account;
                            result.UserId = userAccount.UserId;
                            result.AccountId = userAccount.Id;
                        }
                        else
                        {
                            return UserAccountFrozen(userAccount, now);
                        }

                        if (verifyCodeAppearRule == VerifyCodeAppearRule.Judge)
                        {
                            _verifyCodeService.RemovePicVerifyIpStat(ipAddress, PicVerifyIpStatType.LogonFailure);
                        }

                        return UserCode.Success;
                    }
                    return UserCode.AccountOrPasswordError;
                });
            }
            else if (solutionModel != null && solutionModel.Type == SolutionType.Uap)
            {
                validor.AppendValidAndSet(() =>
                {
                    var uapClient = _thirdPartyService.GetUapClient(solutionModel);
                    var errorMsg = string.Empty;
                    var rt = uapClient.GetByPassword(identity, notEpwd, msg => { errorMsg = msg; });
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        var ex = new BusinessException(errorMsg) { Code = (int)UserCode.UapSolutionError };
                        throw ex;
                    }

                    if (rt != null)
                    {
                        return AuthAndAutoCreateThirdUser(rt.Uid, rt.Uid, solutionModel, result, appId, terminalCode,
                            ipAddress, now);
                    }
                    return UserCode.AccountOrPasswordError;
                });
            }
            else if (solutionModel != null && solutionModel.Type == SolutionType.IdStar)
            {
                validor.AppendValidAndSet(() =>
                {
                    var idStartClient = _thirdPartyService.GetIdStarClient(solutionModel);
                    var errorMsg = string.Empty;
                    var isOk = idStartClient.AuthPassword(identity, notEpwd, msg => { errorMsg = msg; });
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        var ex = new BusinessException(errorMsg) { Code = (int)UserCode.IdStarSolutionError };
                        throw ex;
                    }

                    if (isOk)
                    {
                        return AuthAndAutoCreateThirdUser(identity, identity, solutionModel, result, appId, terminalCode,
                            ipAddress, now);
                    }
                    return UserCode.AccountOrPasswordError;
                });
            }

            var code = validor.Valid();
            if (code != UserCode.Success)
            {
                if (solutionModel != null && solutionModel.Type == SolutionType.Custom && verifyCodeAppearRule == VerifyCodeAppearRule.Judge)
                {
                    _verifyCodeService.RecordPicVerifyIpStat(ipAddress, PicVerifyIpStatType.LogonFailure);
                    if (!result.NeedPicVerifyCode)
                    {
                        result.NeedPicVerifyCode =
                            _verifyCodeService.JudgeIfNeedPicVerifyCode(verifyCodeAppearRule,
                                ipAddress, PicVerifyIpStatType.LogonFailure);
                    }
                }
            }
            return new ResultWrapper<UserCode, LoginOrRegisterResult>(code, result);
        }

        private UserCode AuthAndAutoCreateThirdUser(string uid, string nickName, Solution solutionModel, LoginOrRegisterResult result, int appId, int terminalCode, long ipAddress, DateTime now)
        {
            var userAccount = GetUserAccountByAccount(uid, solutionModel.Id);
            User user = null;
            if (userAccount == null)
            {
                user = new User(now);
                userAccount = new UserAccount(user.CreateTime)
                {
                    AppId = appId,
                    Account = uid,
                    NickName = nickName,
                    SolutionId = solutionModel.Id,
                    Status = AccountStatus.Ready,
                };
                var rs = TryCreateUser(user, userAccount, appId, terminalCode, ipAddress,
                    RegisterMode.Register);
                if (rs.Code != UserCode.Success)
                    return rs.Code;
            }
            else
            {
                user = _userRepository.Get(userAccount.UserId);

                if (user.Status == UserStatus.Disabled)
                    return UserCode.UserDisable;

                if (user.Status == UserStatus.Frozen && user.FrozenExpire > now)
                    return UserCode.UserFrozen;
            }

            var userLoginLog = new UserLoginLog
            {
                UserId = userAccount.UserId,
                AppId = appId,
                IdentityType = UserIdentityType.Solution,
                TerminalCode = terminalCode,
                IpAddress = ipAddress,
                LoginTime = now,
                IpCityId = _appBaseService.GetCityIdByIp(ipAddress)
            };
            _userLoginLogRepository.Create(userLoginLog);
            result.UserDisplayName = userAccount.Account;
            result.UserId = userAccount.UserId;
            return UserCode.Success;
        }


        /// <summary>
        /// 修改用户资料-标识信息
        /// </summary>
        public UserCode AdminModifyUserIdentity(long userId
            , string idCard
            , long adminUserId
            , long ipAddress
            , int appId
            , int terminalCode)
        {
            SetWorkbenchVal(appId, terminalCode, ipAddress);
            //判断用户名手机邮箱修改时候,如果清空任何一种方式的时候，是否仍然至少会有一种可以登录的方式。
            var user = _userRepository.Get(userId);
            if (user == null)
                return UserCode.InvalidUser;

            var oldIdCared = user.IDCard;

            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => UserValid.ValidIDCardFormat(idCard));

            user.SetIDCard(SetIdCardNotEquals(idCard, user,
                () => validor.AppendValidAndSet(() => IDCardExists(idCard))));

            var code = validor.Valid();

            if (code == UserCode.Success)
            {
                if (oldIdCared != user.IDCard)
                {
                    using (var tran = new TransactionScope())
                    {
                        _userRepository.Update(user);
                        InsertUserMainHistoryLog(userId, string.Format("{0}管理员修改证件“{1}”", adminUserId, oldIdCared)
                            , user.IDCard, UserHistoryValueType.AdminEidtIDCard);
                        tran.Complate();
                    }
                }
            }

            return code;
        }
    }
}
