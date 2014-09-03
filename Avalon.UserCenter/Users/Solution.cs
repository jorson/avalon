using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Avalon.UserCenter.Models;

namespace Avalon.UserCenter
{
    /// <summary>
    /// 方案
    /// </summary>
    public class Solution //: IValidatable
    {
        /// <summary>
        /// 帐号方案的方案代码缓存键
        /// </summary>
        public const string CacheRegionCode = "code";

        /// <summary>
        /// 标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 方案编号
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 方案名称
        /// </summary>
		public virtual string Name { get; set; }

		private SolutionSettings _settings;
        /// <summary>
		/// 方案配置信息
		/// </summary>
		public virtual SolutionSettings Settings {
			get
			{
			    if (_settings == null)
			        _settings = new SolutionSettings();

				return _settings;
			}
			set {
				_settings = value??new SolutionSettings();
			}
		}
        /// <summary>
        /// 系统简称
        /// </summary>
        public virtual string SystemAbbreviation
        {
            get { return Settings.GetValue("SystemAbbreviation"); }
            set
            {
                Settings["SystemAbbreviation"] = value;
            }
        }

        /// <summary>
        /// 系统全称
        /// </summary>
        public virtual string SystemName
        {
            get { return Settings.GetValue("SystemName"); }
            set
            {
                
                Settings["SystemName"] = value;
            }
        }

        /// <summary>
        /// 方案所属应用标识
        /// </summary>
        public virtual int AppId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; protected set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get;  set; }

        /// <summary>
        /// 方案类型
        /// </summary>
        public virtual SolutionType Type { get; set; }

        /// <summary>
        /// 方案状态
        /// </summary>
        public virtual SolutionStatus Status { get; set; }

        /// <summary>
        /// 品牌Logo图片对象Id
        /// </summary>
        public virtual int LogoObjectId
        {
            get
            {
                int objectId = 0;
                if (int.TryParse(Settings.GetValue("LogoObjectId"), out objectId)) ;
                return objectId;
            }
            set
            {
               
                Settings["LogoObjectId"] = value.ToString();
            }
        }

        /// <summary>
        /// 帐号名规则
        /// </summary>
        public virtual string AccountNameRule
        {
            get { return Settings.GetValue("AccountNameRule"); }
            set { Settings["AccountNameRule"] = value; }
        }


		/// <summary>
		/// Vailds the name of the account.
		/// </summary>
		/// <returns>The account name.</returns>
		/// <param name="account">Account.</param>
		public virtual UserCode VaildAccountName(string account)
		{
		    if (string.IsNullOrEmpty(account))
		        return UserCode.EmptyAccount;
			Regex accountRegex=new Regex(AccountNameRule);
			return accountRegex.IsMatch (account) ? UserCode.Success : UserCode.InvalidCustomAccount;
		}

        /// <summary>
        /// 帐号名规则描述
        /// </summary>
        public virtual string AccountNameRuleDesc
        {
            get { return Settings.GetValue("AccountNameRuleDesc"); }
            set { Settings["AccountNameRuleDesc"] = value; }
        }

        /// <summary>
        /// 最小密码长度
        /// </summary>
        public virtual int MinPasswordLength
        {
            get
            {
                int objectId = 0;
                if (int.TryParse(Settings.GetValue("MinPasswordLength"), out objectId)) ;
                return objectId;
            }
            set { Settings["MinPasswordLength"] = value.ToString(); }
        }

        /// <summary>
        /// 最大密码长度
        /// </summary>
        public virtual int MaxPasswordLength
        {
            get
            {
                int objectId = 0;
                if (int.TryParse(Settings.GetValue("MaxPasswordLength"), out objectId)) ;
                return objectId;
            }
            set { Settings["MaxPasswordLength"] = value.ToString(); }
        }

		public virtual UserCode VaildPassword(string password)
		{
			if (password.Length >= MinPasswordLength
			   && password.Length <= MaxPasswordLength)
				return UserCode.Success;
            return UserCode.InvalidCustomPasswordLength;
		}

        /// <summary>
        /// 密码规则描述
        /// </summary>
        public virtual string PasswordRuleDesc
        {
            get { return Settings.GetValue("PasswordRuleDesc"); }
            set { Settings["PasswordRuleDesc"] = value; }
        }

        /// <summary>
        /// 是否开放注册
        /// </summary>
        public virtual bool OpenRegister
        {
            get
            {
                bool isOpen;
                bool.TryParse(Settings.GetValue("OpenRegister"), out isOpen);
                return isOpen;
            }
            set { Settings["OpenRegister"] = value.ToString().ToLower(); }
        }

        /// <summary>
        /// 注册时验证码显示规则
        /// </summary>
        public virtual VerifyCodeAppearRule RegisterVerifyCodeAppearRule
        {
            get
            {
                VerifyCodeAppearRule outRule;
                if (!VerifyCodeAppearRule.TryParse(Settings.GetValue("RegisterVerifyCodeAppearRule"), out outRule))
                    outRule = VerifyCodeAppearRule.Need;

                return outRule;
            }
            set { Settings["RegisterVerifyCodeAppearRule"] = value.ToString(); }
        }

        /// <summary>
        /// 登录时验证码显示规则
        /// </summary>
        public virtual VerifyCodeAppearRule LoginVerifyCodeAppearRule
        {
            get
            {
                VerifyCodeAppearRule outRule;
                if (!VerifyCodeAppearRule.TryParse(Settings.GetValue("LoginVerifyCodeAppearRule"), out outRule))
                    outRule = VerifyCodeAppearRule.Judge;

                return outRule;
            }
            set { Settings["LoginVerifyCodeAppearRule"] = value.ToString(); }
        }

        /// <summary>
        /// 图片验证码显示格式
        /// </summary>
        public virtual PicVerifyCodeDisplayFormat PicVerifyCodeDisplayFormat
        {
            get
            {
                PicVerifyCodeDisplayFormat outRule;
                if (!PicVerifyCodeDisplayFormat.TryParse(Settings.GetValue("PicVerifyCodeDisplayFormat"), out outRule))
                    outRule = PicVerifyCodeDisplayFormat.OnlyDigits;

                return outRule;
            }
            set { Settings["PicVerifyCodeDisplayFormat"] = value.ToString(); }
        }

    }

    public class SolutionSettings : Dictionary<string, string>, ICloneable
    {
        public SolutionSettings()
        {

        }

        public SolutionSettings(IDictionary<string, string> dict)
            : base(dict)
        {

        }

        public object Clone()
        {
            return new SolutionSettings(this);
        }

        public override bool Equals(object obj)
        {
            var setting = obj as SolutionSettings;
            if (setting == null)
                return false;

            if (setting.Keys.Count != Keys.Count)
                return false;

            foreach (var key in setting.Keys)
            {
                string outVal;
                if (!TryGetValue(key, out outVal))
                    return false;

                if (!string.Equals(outVal, setting[key]))
                    return false;
            }
            return true;
        }
    }

    public static class SolutionExtend
    {
        /// <summary>
        /// 验证帐号方案
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public static UserCode Valid(this Solution solution)
        {
            if (solution == null)
                return UserCode.NotExistsSolution;

            if (solution.Status == SolutionStatus.Disable)
                return UserCode.DisableSolution;

            return UserCode.Success;
        }

        /// <summary>
        /// 必须是第三方帐号方案
        /// </summary>
        public static UserCode NeedThirdSolution(this Solution solution)
        {
            var code = solution.Valid();
            if (code != UserCode.Success)
                return code;

            return solution.Type != SolutionType.Custom ? UserCode.Success : UserCode.UnThirdSolution;
        }

        /// <summary>
        /// 必须是自定义帐号方案
        /// </summary>
        public static UserCode NeedCustomSolution(this Solution solution)
        {
            var code = solution.Valid();
            if (code != UserCode.Success)
                return code;

            return solution.Type == SolutionType.Custom ? UserCode.Success : UserCode.UnCustomSolution;
        }

        public static UserCode AllowCustomSolutionRegister(this Solution solution)
        {
            var code = solution.NeedCustomSolution();
            if (code != UserCode.Success)
                return code;

            return solution.OpenRegister ? UserCode.Success : UserCode.NoAllowCustomSolutionRegister;
        }

        public static UserCode NeedVaildPasswordSolution(this Solution solution)
        {
            var code = solution.Valid();
            if (code != UserCode.Success)
                return code;

            return solution.Type != SolutionType.QQAccount && solution.Type != SolutionType.SinaWeibo ? UserCode.Success : UserCode.UnVaildPasswordSolution;
        }

        public static string GetValue(this SolutionSettings setting, string key)
        {
            string outStr;

            if (setting == null || !setting.TryGetValue(key,out outStr))
            {
                outStr = string.Empty;
            }
            return outStr;
        }
    }


}
