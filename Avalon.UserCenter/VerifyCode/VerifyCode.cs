using System;
using System.Linq;

namespace Avalon.UserCenter
{
    public class VerifyCodeKey
    {
        public VerifyCodeKey(string key)
        {
            Key = key;
        }

        public string Key { get;protected set; }

        public override string ToString()
        {
            return Key;
        }
    }

    public interface IVerifyCode
    {
        /// <summary>
        /// Redis保存的值
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// 获取键值
        /// </summary>
        /// <returns></returns>
        VerifyCodeKey GetKey();

        /// <summary>
        /// 校验验证码是否匹配
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        bool Valid(IVerifyCode verifyCode);
    }

    public interface IEmailVerifyCode
    {
        
    }

    public abstract class VerifyCodeBase : IVerifyCode
    {
        /// <summary>
        /// Redis保存的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 获取键值
        /// </summary>
        /// <returns></returns>
        public abstract VerifyCodeKey GetKey();

        /// <summary>
        /// 转换为当前对象
        /// </summary>
        /// <param name="value"></param>
        public virtual void Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            Value = value;
        }

        /// <summary>
        /// 校验验证码是否匹配
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public virtual bool Valid(IVerifyCode verifyCode)
        {
            if (verifyCode == null)
                return false;

            return !verifyCode.Value.IsNullOrWhiteSpace() && String.Equals(Value, verifyCode.Value, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    /// <summary>
    /// 校验用验证码
    /// </summary>
    public class CheckVerifyCode : VerifyCodeBase
    {
        public CheckVerifyCode()
        {
            
        }
        public string Key { get; set; }

        public override VerifyCodeKey GetKey()
        {
            return new VerifyCodeKey(Key);
        }
#if DEBUG
        public override bool Valid(IVerifyCode verifyCode)
        {
            if (verifyCode.Value == "777" || Value == "777")
                return true;
            else
            {
                return base.Valid(verifyCode);
            }
        }
#endif
    }

    /// <summary>
    /// 图片验证码
    /// </summary>
    public class PicVerifyCode : VerifyCodeBase
    {
        public PicVerifyCode()
        {
            
        }
        public PicVerifyCode(string sessionId, string verifyCode)
        {
            SessionId = sessionId;
            Value = verifyCode;
            VerifyCode = verifyCode;
        }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        public override VerifyCodeKey GetKey()
        {
            return BuildKey(SessionId);
        }

        public static VerifyCodeKey BuildKey(string sessionId)
        {
            return new VerifyCodeKey(string.Format("vc:pic:{0}", sessionId));
        }

        public override void Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            Value = value;
            VerifyCode = value;
        }

#if DEBUG
        public override bool Valid(IVerifyCode verifyCode)
        {
            if (verifyCode.Value == "777" || Value == "777")
                return true;
            else
            {
                return base.Valid(verifyCode);
            }
        }
#endif
        
    }

    /// <summary>
    /// 手机验证码
    /// </summary>
    public abstract class MobileVerifyCode : VerifyCodeBase
    {
        public void Instance(string mobile, string verifyCode)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
        }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        public override void Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            Value = value;
            VerifyCode = value;
        }

        public override bool Valid(IVerifyCode verifyCode)
        {
#if DEBUG
            if (verifyCode.Value == "777" || Value == "777")
                return true;
            else 
#endif
            {
                return base.Valid(verifyCode);
            }
        }
    }

    /// <summary>
    /// 登陆手机验证码
    /// </summary>
    public class LoginMobileVerifyCode : MobileVerifyCode
    {
        public LoginMobileVerifyCode()
        {
            
        }

        public LoginMobileVerifyCode(string mobile, string verifyCode)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildLoginKey(Mobile);
        }

        public static VerifyCodeKey BuildLoginKey(string mobile)
        {
            return new VerifyCodeKey(string.Format("loginsms:{0}", mobile));
        }
    }

    /// <summary>
    /// 注册手机验证码
    /// </summary>
    public class RegisterMobileVerifyCode : MobileVerifyCode
    {
        public RegisterMobileVerifyCode()
        {
            
        }

        public RegisterMobileVerifyCode(string mobile, string verifyCode)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildRegisterKey(Mobile);
        }

        public static VerifyCodeKey BuildRegisterKey(string mobile)
        {
            return new VerifyCodeKey(string.Format("registersms:{0}", mobile));
        }
    }

    /// <summary>
    /// 忘记密码手机验证码
    /// </summary>
    public class ForgetMobileVerifyCode : MobileVerifyCode
    {
        public ForgetMobileVerifyCode()
        {
            
        }

        public ForgetMobileVerifyCode(string mobile, string verifyCode)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildForgetKey(Mobile);
        }

        public static VerifyCodeKey BuildForgetKey(string mobile)
        {
            return new VerifyCodeKey(string.Format("forgetsms:{0}", mobile));
        }
    }

    /// <summary>
    /// 密保手机验证码
    /// </summary>
    public class SecurityMobileVerifyCode : MobileVerifyCode
    {
        public SecurityMobileVerifyCode()
        {
            
        }

        public SecurityMobileVerifyCode(string mobile, string verifyCode)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildSecurityKey(Mobile);
        }

        public static VerifyCodeKey BuildSecurityKey(string mobile)
        {
            return new VerifyCodeKey(string.Format("securitysms:{0}", mobile));
        }
    }

    public abstract class SolutionSmsCode : MobileVerifyCode
    {
        public int SolutionId { get; set; }
        public void Instance(string mobile, string verifyCode,int solutionId)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
            SolutionId = solutionId;
        }
    }

    public class SolutionSecuritySmsCode : SolutionSmsCode
    {
        public SolutionSecuritySmsCode()
        {

        }

        public SolutionSecuritySmsCode(string mobile, string verifyCode, int solutionId)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
            SolutionId = solutionId;
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildSolutionSecurityKey(Mobile, SolutionId);
        }

        public static VerifyCodeKey BuildSolutionSecurityKey(string mobile,int solutionId)
        {
            return new VerifyCodeKey(string.Format("solutionsecuritysms:{0}_{1}", mobile, solutionId));
        }
    }

    public class SolutionForgetSmsCode : SolutionSmsCode
    {
        public SolutionForgetSmsCode()
        {

        }

        public SolutionForgetSmsCode(string mobile, string verifyCode, int solutionId)
        {
            Mobile = mobile;
            Value = verifyCode;
            VerifyCode = verifyCode;
            SolutionId = solutionId;
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildSolutionForgetKey(Mobile, SolutionId);
        }

        public static VerifyCodeKey BuildSolutionForgetKey(string mobile, int solutionId)
        {
            return new VerifyCodeKey(string.Format("solutionforgetysms:{0}_{1}", mobile, solutionId));
        }
    }

    /// <summary>
    /// 邮箱验证码
    /// </summary>
    public class EmailVerifyCode : VerifyCodeBase, IEmailVerifyCode
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; protected set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; protected set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; protected set; }

        public EmailVerifyCode()
        {
            
        }

        public EmailVerifyCode(long userId, string email, string verifyCode)
        {
            UserId = userId;
            Email = email;
            VerifyCode = verifyCode;
            Value = string.Format("{0}:{1}", UserId, Email);
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildKey(VerifyCode);
        }

        /// <summary>
        /// 邮箱创建键值
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static VerifyCodeKey BuildKey(string verifyCode)
        {
            return new VerifyCodeKey(string.Format("email:{0}", verifyCode));
        }

        public override void Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var array = value.Split(':');
            if (array.Length != 2)
                return;
            Value = value;
            VerifyCode = value;
            var userIdStr = array[0];
            Email = array[1];

            long userId;
            long.TryParse(userIdStr, out userId);
            UserId = userId;
        }

    }

    /// <summary>
    /// 密保邮箱验证码
    /// </summary>
    public class SecurityEmailVerifyCode : EmailVerifyCode, IEmailVerifyCode
    {
        public SecurityEmailVerifyCode()
        {
            
        }

        public SecurityEmailVerifyCode(long userId, string email, string verifyCode)
            : base(userId,email,verifyCode)
        {
        }

        public override VerifyCodeKey GetKey()
        {
            return BuildSecurityKey(VerifyCode);
        }

        /// <summary>
        /// 密保邮箱创建键值
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static VerifyCodeKey BuildSecurityKey(string verifyCode)
        {
            return new VerifyCodeKey(string.Format("securityemail:{0}", verifyCode));
        }
    }

    /// <summary>
    /// 找回密码邮件验证码
    /// </summary>
    public class RecoverEmailVerifyCode : VerifyCodeBase, IEmailVerifyCode
    {
        public RecoverEmailVerifyCode()
        {
            
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; protected set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; protected set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; protected set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string Identity { get; protected set; }

        /// <summary>
        /// 帐号方案编号
        /// </summary>
        public string Solution { get; protected set; }

        public RecoverEmailVerifyCode(string verifyCode, long userId, string email, string identity,string solution="")
        {
            UserId = userId;
            Email = email;
            Identity = identity;
            VerifyCode = verifyCode;
            Solution = solution;
            Value = string.Format("{0}:{1}:{2}:{3}", UserId, Email, Identity, Solution);
        }

        public override VerifyCodeKey GetKey()
        {
            return VerifyCode.IsNullOrWhiteSpace() ? null : BuildKey(VerifyCode);
        }

        public static VerifyCodeKey BuildKey(string verifyCode)
        {
            return new VerifyCodeKey(string.Format("recoveremail:{0}", verifyCode));
        }

        public override void Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var array = value.Split(':');
            if (array.Length != 4)
                return;
            Value = value;
            VerifyCode = value;
            var userIdStr = array[0];
            Email = array[1];
            Identity = array[2];
            Solution = array[3];

            long userId;
            long.TryParse(userIdStr, out userId);
            UserId = userId;
        }
    }

    /// <summary>
    /// 令牌验证码
    /// </summary>
    public class RecoverToken : VerifyCodeBase
    {
        public RecoverToken()
        {
            
        }
        public RecoverToken(string token, long userId, string identity,string solution="")
        {
            Token = token;
            UserId = userId;
            Identity = identity;
            Solution = solution;
            Value = string.Format("{0}:{1}:{2}", UserId, Identity, solution);
        }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get;protected set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; protected set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string Identity { get; protected set; }

        /// <summary>
        /// 帐号方案编号
        /// </summary>
        public string Solution { get; protected set; }

        public override VerifyCodeKey GetKey()
        {
            return BuildKey(Token);
        }

        public static VerifyCodeKey BuildKey(string token)
        {
            return new VerifyCodeKey(string.Format("vc:recoverToken:{0}", token));
        }

        public override void Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var array = value.Split(':');
            if (array.Length != 3)
                return;
            Value = value;
            var userIdStr = array[0];
            Identity = array[1];
            Solution = array[2];

            long userId;
            long.TryParse(userIdStr, out userId);
            UserId = userId;
        }
    }
}
