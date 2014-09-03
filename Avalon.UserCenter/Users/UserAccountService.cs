using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Avalon.UserCenter.Models;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.UserCenter.Users
{
    public class UserAccountService : IService
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserMainHistoryLogRepository _userMainHistoryLogRepository;
        private readonly IUserAccountCreateInfoRepository _userAccountCreateInfoRepository;
        private readonly IUserLoginLogRepository _userLoginLogRepository;
        private readonly UserService _userService;
        private readonly AppBaseService _appBaseService;
        private readonly VerifyCodeService _verifyCodeService;
        private readonly PasswordService _passwordService;

        public UserAccountService(ISolutionRepository solutionRepository,
            IUserAccountRepository userAccountRepository,
            IUserMainHistoryLogRepository userMainHistoryLogRepository,
            IUserAccountCreateInfoRepository userAccountCreateInfoRepository,
            IUserLoginLogRepository userLoginLogRepository,
            UserService userService,
            AppBaseService appBaseService, VerifyCodeService verifyCodeService,
            PasswordService passwordService)
        {
            _solutionRepository = solutionRepository;
            _userAccountRepository = userAccountRepository;
            _userAccountCreateInfoRepository = userAccountCreateInfoRepository;
            _userLoginLogRepository = userLoginLogRepository;
            _userService = userService;
            _appBaseService = appBaseService;
            _verifyCodeService = verifyCodeService;
            _userMainHistoryLogRepository = userMainHistoryLogRepository;
            _passwordService = passwordService;
        }

        /// <summary>
        /// 创建自定义通行证
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public UserCode CreatSolution(Solution solution)
        {
            var usercode = ValidSolution(solution);
            if (usercode == UserCode.Success)
            {
                var solutiontemp = _userService.GetSolutionByCode(solution.Code);
                if (solutiontemp != null)
                    return UserCode.RepeatedSolutionCode;

                solutiontemp = GetSolutionByName(solution.Name);
                if (solutiontemp != null)
                    return UserCode.RepeatedSolutionName;

                solution.CreateTime = NetworkTime.Now;
                _solutionRepository.Create(solution);
            }
            return usercode;
        }

        /// <summary>
        /// 编辑自定义通行证
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public UserCode EditSolution(Solution solution)
        {
            var solutionin = GetSolution(solution.Id);
            if (solutionin == null)
                return UserCode.NotExistsSolution;
            var usercode = ValidSolution(solution);
            if (usercode == UserCode.Success)
            {
                var solutiontemp = _userService.GetSolutionByCode(solution.Code);
                if (solutiontemp != null && solutiontemp.Id != solution.Id)
                    return UserCode.RepeatedSolutionCode;

                solutiontemp = GetSolutionByName(solution.Name);
                if (solutiontemp != null && solutiontemp.Id != solution.Id)
                    return UserCode.RepeatedSolutionName;
                solutionin.Code = solution.Code;
                solutionin.Name = solution.Name;
                solutionin.AppId = solution.AppId;
                solutionin.Type = solution.Type;
                solutionin.Status = solution.Status;
                solutionin.Settings = solution.Settings;
                _solutionRepository.Update(solutionin);
            }
            return usercode;
        }

        /// <summary>
        /// 验证自定义帐号方案
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        private UserCode ValidSolution(Solution solution)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => SolutionValid.ValidCodeFormat(solution.Code));
            validor.AppendValidAndSet(() => SolutionValid.ValidSystemAbbreviationFormat(solution.SystemAbbreviation));
            return validor.Valid();
        }

        /// <summary>
        /// 删除自定义通行证
        /// </summary>
        /// <param name="solutionid"></param>
        public void DeleteSolution(int solutionid)
        {
            var solution = GetSolution(solutionid);
            if (solution != null)
                _solutionRepository.Delete(solution);
        }

        /// <summary>
        /// 通过ID获取自定义通行证
        /// </summary>
        /// <param name="solutionid"></param>
        /// <returns></returns>
        public Solution GetSolution(int solutionid)
        {
            return _solutionRepository.Get(ShardParams.Empty, solutionid);
        }

        public List<Solution> GetCustomSolutions()
        {
            return _solutionRepository.FindAll(_solutionRepository.CreateSpecification()
                .Where(s => s.Type == SolutionType.Custom)).ToList();
        }

        public List<Solution> GetUsableCustomSolutions()
        {
            return _solutionRepository.FindAll(_solutionRepository.CreateSpecification()
                .Where(s => s.Type == SolutionType.Custom && s.Status == SolutionStatus.Normal)).ToList();
        }

        /// <summary>
        /// 创建帐号
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <param name="solutionId"></param>
        /// <param name="appId"></param>
        /// <param name="createappId"></param>
        /// <param name="terminalCode"></param>
        /// <param name="ipAddress"></param>
        /// <param name="registerMode"></param>
        /// <returns></returns>
        public ResultWrapper<UserCode, string> CreateUserAccount(string accountName, string password, int appId, int solutionId,
            int createappId, int terminalCode, long ipAddress, RegisterMode registerMode = RegisterMode.Import)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            Solution solution = null;
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => ValidSolution(solutionId, out solution));
            validor.AppendValidAndSet(() => solution.VaildPassword(password));
            validor.AppendValidAndSet(() => solution.VaildAccountName(accountName));
            validor.AppendValidAndSet(() => _userService.UserAccountExists(solution, accountName));
            var usercode = validor.Valid();
            if (usercode != UserCode.Success)
            {
                switch (usercode)
                {
                    case UserCode.InvalidCustomPasswordLength:
                        return new ResultWrapper<UserCode, string>(usercode,
                            solution.PasswordRuleDesc.IsNullOrWhiteSpace() ? "密码长度错误" : solution.PasswordRuleDesc);
                    case UserCode.InvalidCustomAccount:
                        return new ResultWrapper<UserCode, string>(usercode,
                            solution.AccountNameRuleDesc.IsNullOrWhiteSpace() ? "帐号名称不符合规则" : solution.AccountNameRuleDesc);
                    default:
                        return new ResultWrapper<UserCode, string>(usercode);
                }
            }
            var createtime = NetworkTime.Now;
            var user = new User(createtime);
            var accountcreate = new UserAccount(createtime)
            {
                Account = accountName,
                SolutionId = solutionId,
                AppId = appId
            };
            accountcreate.SetPassword(password);
            var rs = _userService.TryCreateUser(user, accountcreate, createappId, terminalCode, ipAddress);
            return new ResultWrapper<UserCode, string>(rs.Code);
        }

        private UserCode ValidAccount(long accountId, out UserAccount account)
        {
            account = GetUserAccount(accountId);
            if (account == null)
                return UserCode.EmptyAccount;
            return UserCode.Success;
        }

        private UserCode ValidSolution(int solutionId, out Solution solution)
        {
            solution = GetSolution(solutionId);
            if (solution == null)
                return UserCode.NotExistsSolution;
            return solution.Valid();
        }

        private UserCode ValidPasswordFormat(Solution solution, string password)
        {
            if (password.Length < solution.MinPasswordLength || password.Length > solution.MaxPasswordLength)
                return UserCode.InvalidCustomPasswordLength;
            return UserCode.Success;
        }

        /// <summary>
        /// 通过方案名称来获取方案
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Solution GetSolutionByName(string name)
        {
            return _solutionRepository.FindOne(_solutionRepository.CreateSpecification().Where(s => s.Name == name));
        }

        /// <summary>
        /// 重置自定义帐号密码
        /// </summary>
        public ResultWrapper<UserCode, string> SetAccountPassword(long accountId
            , string password
            , string emailDisplayName
            , string emailSubject
            , string emailTemplateExist
            , string emailTemplateNoExist
            , string smsTemplateExist
            , string smsTemplateNoExist, long ip, int appid, int terminalcode)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            UserAccount account = null;
            Solution solution = null;
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(() => ValidAccount(accountId, out account));
            validor.AppendValidAndSet(() => ValidSolution(account.SolutionId, out solution));
            validor.AppendValidAndSet(() => ValidPasswordFormat(solution, password));
            var usercode = validor.Valid();
            if (usercode != UserCode.Success)
            {
                if (usercode == UserCode.InvalidCustomPasswordLength)
                    return new ResultWrapper<UserCode, string>(usercode,
                        solution.PasswordRuleDesc.IsNullOrWhiteSpace() ? "密码长度错误" : solution.PasswordRuleDesc);
                return new ResultWrapper<UserCode, string>(usercode);
            }
            var oldpwd = account.Password;
            account.SetPassword(password);
            var newpwd = account.Password;
            _userAccountRepository.Update(account);
            var userSecurity = _userService.GetUserSecurity(account.UserId);
            var mobile = !string.IsNullOrWhiteSpace(userSecurity.Mobile)
                ? userSecurity.Mobile : string.Empty;
            var email = !string.IsNullOrWhiteSpace(userSecurity.Email)
                ? userSecurity.Email : string.Empty;

            _passwordService.SendEmailAndMobile(email, emailDisplayName, emailSubject, emailTemplateExist, emailTemplateNoExist,
                mobile, smsTemplateExist, smsTemplateNoExist, ip);
            AccountUserMainHistoryLog(account.UserId, account.SolutionId, oldpwd, newpwd, ip, appid, terminalcode, UserHistoryValueType.AccountPassword);
            return new ResultWrapper<UserCode, string>(usercode);
        }

        private void AccountUserMainHistoryLog(long userId, int solutionId, string oldvalue, string newvalue, long ip, int appid, int terminalcode, UserHistoryValueType valType = UserHistoryValueType.Account)
        {
            var oldstr = string.Format("{0}:{1}", solutionId, oldvalue);
            var newstr = string.Format("{0}:{1}", solutionId, newvalue);
            InsertUserMainHistoryLog(userId, oldstr, newstr, valType, ip, appid, terminalcode);
        }

        public void InsertUserMainHistoryLog(long userId, string oldVal, string newVal, UserHistoryValueType valType, long ip, int appid, int terminalcode)
        {
            var userMainHistoryLog = new UserMainHistoryLog
            {
                UserId = userId,
                OldVal = oldVal,
                NewVal = newVal,
                ValType = valType,
                IpAddress = ip,
                AppId = appid,
                TerminalCode = terminalcode,
                IpCityId = _appBaseService.GetCityIdByIp(ip)
            };
            _userMainHistoryLogRepository.Create(userMainHistoryLog);
        }

        /// <summary>
        /// 禁用解禁自定义帐号
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="isdisable"></param>
        /// <returns></returns>
        public UserCode DisableAccount(long accountId, bool isdisable)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            UserAccount account = null;
            Solution solution = null;
            validor.AppendValidAndSet(() => ValidAccount(accountId, out account));
            validor.AppendValidAndSet(() => ValidSolution(account.SolutionId, out solution));
            var usercode = validor.Valid();
            if (usercode == UserCode.Success)
            {
                account.Status = isdisable ? AccountStatus.Disabled : AccountStatus.Ready;
                account.FrozenExpire = NetworkTime.Null;
                _userAccountRepository.Update(account);
            }
            return usercode;
        }

        /// <summary>
        /// 冻结解冻自定义帐号
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="frozentime">冻结时间（小时）</param>
        /// <param name="isfrozen">是否冻结</param>
        /// <returns></returns>
        public UserCode FrozenAccount(long accountId, int frozentime, bool isfrozen)
        {
            var validor = new Validor<UserCode>(UserCode.Success);
            UserAccount account = null;
            Solution solution = null;
            validor.AppendValidAndSet(() => ValidAccount(accountId, out account));
            validor.AppendValidAndSet(() => ValidSolution(account.SolutionId, out solution));
            var usercode = validor.Valid();
            if (usercode == UserCode.Success)
            {
                if (isfrozen)
                {
                    account.Status = AccountStatus.Frozen;
                    account.FrozenExpire = NetworkTime.Now.AddHours(frozentime);
                }
                else
                {
                    account.Status = AccountStatus.Ready;
                    account.FrozenExpire = NetworkTime.Null;
                }
                _userAccountRepository.Update(account);
            }
            return usercode;
        }

        /// <summary>
        /// 通过ID获取自定义帐号
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private UserAccount GetUserAccount(long accountId)
        {
            return _userAccountRepository.Get(ShardParams.Empty, accountId);
        }

        /// <summary>
        /// 获取应用有帐号注册的自定义帐号方案列表
        /// </summary>
        /// <param name="appId">应用</param>
        /// <returns></returns>
        public IList<Solution> GetAppHasUserCustomSolutions(int appId)
        {
            var solutionIds = _solutionRepository.GetAppHasUserSolutionIds(appId);
            if (solutionIds.Any())
            {
                var spec = _solutionRepository.CreateSpecification();
                spec = spec.Where(s => s.Type == SolutionType.Custom && solutionIds.Contains(s.Id));
                return _solutionRepository.FindAll(spec);
            }
            return new List<Solution>();
        }

        public AccountInfo GetAccountInfoByUserId(long userId)
        {
            var account = _userAccountRepository.FindOne(
                    _userAccountRepository.CreateSpecification().Where(a => a.UserId == userId));
            if (account == null)
                return null;
            var solution = _solutionRepository.Get(ShardParams.Empty, account.SolutionId);
            var accountinfo = new AccountInfo
            {
                CreateTime = account.CreateTime,
                Account = account.Account,
                AppId = account.AppId,
                Status = account.Status,
                SolutionName = solution != null ? solution.Name : string.Empty
            };
            var usercreate = _userAccountCreateInfoRepository.Get(ShardParams.Empty, account.Id);
            if (usercreate != null)
                accountinfo.RegisterIp = IpAddress.IpIntoToString(usercreate.IpAddress);
            var userlogin = _userLoginLogRepository.FindOne(_userLoginLogRepository.CreateSpecification()
                        .Where(u => u.UserId == account.UserId)
                        .OrderByDescending(u => u.LoginTime));
            if (userlogin != null)
                accountinfo.LastLoginTime = userlogin.LoginTime;
            return accountinfo;
        }

        public UserCode ValidAccountUserName(string userName, string solutionCode)
        {
            var solution = _userService.GetSolutionByCode(solutionCode);
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => string.IsNullOrWhiteSpace(userName) ? UserCode.EmptyUserName : UserCode.Success);
            validor.AppendValidAndSet(solution.NeedCustomSolution);
            validor.AppendValidAndSet(() => solution.VaildAccountName(userName));
            validor.AppendValidAndSet(() => _userService.UserAccountExists(solution, userName));
            return validor.Valid();
        }

        public ResultWrapper<UserCode, LoginOrRegisterResult> CustomRegisterUser(
            Solution solutionModel, string account, string password
            , string sessionId
            , string verifyCode
            , long ipAddress
            , int appId
            , int terminalCode)
        {
            var result = new LoginOrRegisterResult();
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(() => RsaUtil.DecryptAndCheck(password, pwd => { password = pwd; }));
            validor.AppendValidAndSet(solutionModel.AllowCustomSolutionRegister);
            var verifyCodeAppearRule = VerifyCodeAppearRule.Need;
            validor.AppendValidAndSet(() =>
            {
                verifyCodeAppearRule = solutionModel.RegisterVerifyCodeAppearRule;
                result.NeedPicVerifyCode = (!String.IsNullOrEmpty(sessionId) && !String.IsNullOrEmpty(verifyCode))
                    || _verifyCodeService.JudgeIfNeedPicVerifyCode(verifyCodeAppearRule, ipAddress, PicVerifyIpStatType.RegisterSuccess);

                if (result.NeedPicVerifyCode)
                {
                    if (!_verifyCodeService.ValidVerifyCode(new PicVerifyCode(sessionId, verifyCode), true))
                        return UserCode.InvalidVerifyCode;
                }
                return UserCode.Success;
            });
            validor.AppendValidAndSet(() => solutionModel.VaildPassword(password));
            validor.AppendValidAndSet(() => solutionModel.VaildAccountName(account));
            validor.AppendValidAndSet(() => _userService.UserAccountExists(solutionModel, account));
            var code = validor.Valid();
            if (code == UserCode.Success)
            {
                var createtime = NetworkTime.Now;
                var user = new User(createtime);
                var accountcreate = new UserAccount(createtime)
                {
                    Account = account,
                    SolutionId = solutionModel.Id,
                    AppId = appId,
                    Status = AccountStatus.Ready,
                };
                accountcreate.SetPassword(password);
                var rs = _userService.TryCreateUser(user, accountcreate, appId, terminalCode, ipAddress, RegisterMode.Register);
                code = rs.Code;
                if (code == UserCode.Success)
                {
                    result.UserId = rs.Data.Id;
                    result.UserDisplayName = account;
                    result.AccountId = accountcreate.Id;
                    if (verifyCodeAppearRule == VerifyCodeAppearRule.Judge)
                    {
                        _verifyCodeService.RecordPicVerifyIpStat(ipAddress,
                            PicVerifyIpStatType.RegisterSuccess);
                    }
                }
            }
            return new ResultWrapper<UserCode, LoginOrRegisterResult>(code, result);
        }
    }
}
