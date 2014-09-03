using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public enum UserCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,

        /// <summary>
        /// 用户名不能为空
        /// </summary>
        [Description("用户名不能为空")]
        EmptyUserName = 400001,

        /// <summary>
        /// 邮箱不能为空
        /// </summary>
        [Description("邮箱不能为空")]
        EmptyEmail = 400002,

        /// <summary>
        /// 手机号不能为空
        /// </summary>
        [Description("手机号不能为空")]
        EmptyMobile = 400003,

        /// <summary>
        /// 证件号不能为空
        /// </summary>
        [Description("证件号不能为空")]
        EmptyIDCard = 400004,

        /// <summary>
        /// 密码不能为空
        /// </summary>
        [Description("密码不能为空")]
        EmptyPassword = 400005,

        /// <summary>
        /// 帐号不能为空
        /// </summary>
        [Description("帐号不能为空")]
        EmptyAccount = 400006,

        /// <summary>
        /// 帐号或密码错误
        /// </summary>
        [Description("帐号或密码错误")]
        AccountOrPasswordError = 400007,

        /// <summary>
        /// 帐号方案代码不能为空
        /// </summary>
        [Description("帐号方案代码不能为空")]
        EmptySolutionCode = 400008,


        /// <summary>
        /// 至少要有一种合法的登录方式
        /// </summary>
        [Description("至少要有一种合法的登录方式")]
        NeedLeastOneLogonWay = 400010,

        /// <summary>
        /// 无效的用户名
        /// </summary>
        [Description("无效的用户名")]
        InvalidUserName = 400011,

        /// <summary>
        /// 无效的邮箱格式
        /// </summary>
        [Description("无效的邮箱格式")]
        InvalidEmail = 400012,

        /// <summary>
        /// 无效的手机号格式
        /// </summary>
        [Description("无效的手机号格式")]
        InvalidMobile = 400013,

        /// <summary>
        /// 无效的证件号，请输入有效的身份证、军官证或回乡证号码
        /// </summary>
        [Description("无效的证件号，请输入有效的身份证、军官证或回乡证号码")]
        InvalidIDCard = 400014,

        /// <summary>
        /// 无效的自定义帐号方案编码格式
        /// </summary>
        [Description("无效的自定义帐号方案编码格式")]
        InvalidSolutionCode = 400015,

        /// <summary>
        /// 无效的自定义帐号方案
        /// </summary>
        [Description("无效的自定义帐号方案")]
        InvalidSolution = 400016,

        /// <summary>
        /// 无效的用户
        /// </summary>
        [Description("无效的用户")]
        InvalidUser = 400117,

        /// <summary>
        /// 无效的自定义帐号用户
        /// </summary>
        [Description("无效的自定义帐号用户")]
        InvalidAccount = 400018,

        /// <summary>
        /// 用户名长度限6-20个半角字符
        /// </summary>
        [Description("用户名长度限6-20个字符")]
        InvalidUserNameLength = 400021,

        /// <summary>
        /// 邮箱长度限30个半角字符内
        /// </summary>
        [Description("邮箱长度限60个字符内")]
        InvalidEmailLength = 400022,

        /// <summary>
        /// 手机号应为11个字符
        /// </summary>
        [Description("手机号应为11个字符")]
        InvalidMobileLength = 400023,


        /// <summary>
        /// 姓名应为2~20个字符
        /// </summary>
        [Description("姓名应为2~20个字符")]
        InvalidUserFullNameLength = 400026,

        /// <summary>
        /// 密码应为6~20个字符
        /// </summary>
        [Description("密码应为6~20个字符")]
        InvalidPasswordLength = 400027,

        /// <summary>
        /// 帐号系统简称最大为10个字符
        /// </summary>
        [Description("帐号系统简称最大为10个字符")]
        InvalidSystemAbbreviation = 400028,

        /// <summary>
		/// 无效的自定义帐号名称
        /// </summary>
		[Description("无效的自定义帐号名称，实际错误消息由自定义通行证定义")]
        InvalidCustomAccount = 400029,

		/// <summary>
		/// 无效的自定义帐号密码长度
		/// </summary>
		[Description("无效的自定义帐号密码长度，实际错误消息由自定义通行证定义")]
        InvalidCustomPasswordLength = 400030,


        /// <summary>
        /// 用户名已经被使用
        /// </summary>
        [Description("用户名已经被使用")]
        RepeatedUserName = 400031,

        /// <summary>
        /// 邮箱已经被使用
        /// </summary>
        [Description("邮箱已经被使用")]
        RepeatedEmail = 400032,

        /// <summary>
        /// 手机号已经被使用
        /// </summary>
        [Description("手机号已经被使用")]
        RepeatedMobile = 400033,

        /// <summary>
        /// 证件号已经被使用
        /// </summary>
        [Description("证件号已经被使用")]
        RepeatedIDCard = 400034,

        /// <summary>
        /// 帐号已经被使用
        /// </summary>
        [Description("帐号已经被使用")]
        RepeatedAccount = 400035,

        /// <summary>
        /// 自定义帐号编码已经被使用
        /// </summary>
        [Description("自定义帐号编码已经被使用")]
        RepeatedSolutionCode = 400036,

        /// <summary>
        /// 自定义帐号名称已经被使用
        /// </summary>
        [Description("自定义帐号名称已经被使用")]
        RepeatedSolutionName = 400037,

        /// <summary>
        /// 已绑定密保邮箱
        /// </summary>
        [Description("已绑定密保邮箱")]
        SecurityEmailExist = 400038,

        /// <summary>
        /// 已绑定密保手机
        /// </summary>
        [Description("已绑定密保手机")]
        SecurityMobileExist = 400039,

        /// <summary>
        /// 无效第三方Token
        /// </summary>
        [Description("无效第三方Token")]
        InvalidThirdToken = 400040,

        /// <summary>
        /// 无效验证码
        /// </summary>
        [Description("无效验证码")]
        InvalidVerifyCode = 400041,

        /// <summary>
        /// 密码错误
        /// </summary>
        [Description("密码错误")]
        WrongPassword = 400042,

        /// <summary>
        /// 超出手机验证码发送次数
        /// </summary>
        [Description("超出手机验证码发送次数")]
        ExceedMobileVerifyCodeSendNumber = 400050,

        /// <summary>
        /// 用户没有设置手机找回密码功能
        /// </summary>
        [Description("用户没有设置手机找回密码功能")]
        UserNotExistRecoverMobile = 400051,

        /// <summary>
        /// 用户没有设置邮箱找回密码功能
        /// </summary>
        [Description("用户没有设置邮箱找回密码功能")]
        UserNotExistRecoverEmail = 400052,

        /// <summary>
        /// 登录邮箱已激活
        /// </summary>
        [Description("登录邮箱已激活")]
        AlreadyActiveUserEmail = 400053,

        /// <summary>
        /// 密保邮箱已激活
        /// </summary>
        [Description("密保邮箱已激活")]
        AlreadyActiveSecurityEmail = 400054,

        /// <summary>
        /// 用户没有登录邮箱
        /// </summary>
        [Description("用户没有登录邮箱")]
        UserNotExistLoginEmail = 400055,

        /// <summary>
        /// 尝试次数过多
        /// </summary>
        [Description("尝试次数过多")]
        TryManyTimes = 400056,

        /// <summary>
        /// 手机验证码已经发送，请注意查收
        /// </summary>
        [Description("手机验证码已经发送，请注意查收")]
        MobileVerifyCodeSended = 400118,

        /// <summary>
        /// 帐号方案不存在
        /// </summary>
        [Description("帐号方案不存在")]
        NotExistsSolution = 400120,

        /// <summary>
        /// 帐号方案不可用
        /// </summary>
        [Description("帐号方案不可用")]
        DisableSolution = 400121,

        /// <summary>
        /// 非第三方帐号方案
        /// </summary>
        [Description("非第三方帐号方案")]
        UnThirdSolution = 400122,

        /// <summary>
        /// 非自定义帐号方案
        /// </summary>
        [Description("非自定义帐号方案")]
        UnCustomSolution = 400123,

        /// <summary>
        /// 不允许自定义帐号方案注册
        /// </summary>
        [Description("不允许自定义帐号方案注册")]
        NoAllowCustomSolutionRegister = 400124,

        /// <summary>
        /// 非密码验证类的帐号方案
        /// </summary>
        [Description("非密码验证类的帐号方案")]
        UnVaildPasswordSolution = 400125,

        /// <summary>
        /// Uap帐号方案类型错误，实际错误消息内容由UAP定义
        /// </summary>
        [Description("Uap帐号方案类型错误，实际错误消息内容由UAP定义")]
        UapSolutionError = 400126,

        /// <summary>
        /// IdStar帐号方案类型错误，实际错误消息内容由IdStar定义
        /// </summary>
        [Description("IdStar帐号方案类型错误，实际错误消息内容由IdStar定义")]
        IdStarSolutionError = 400127,

        /// <summary>
        /// 该帐号已被禁止登录，请联系客服处理
        /// </summary>
        [Description("该帐号已被禁止登录，请联系客服处理")]
        UserDisable = 400130,

        /// <summary>
        /// 该帐号已被冻结登录，请联系客服处理
        /// </summary>
        [Description("该帐号已被冻结登录，请联系客服处理")]
        UserFrozen = 400131,

        /// <summary>
        /// 无帐号唯一标识信息
        /// </summary>
        [Description("无帐号唯一标识信息")]
        NoIdentity = 400150,

        /// <summary>
        /// 无效的recovertoken
        /// </summary>
        [Description("无效的recovertoken")]
        InvalidRecoverToken = 400151,

        /// <summary>
        /// 证件号与recovertoken所对应的用户信息不一致
        /// </summary>
        [Description("证件号与recovertoken所对应的用户信息不一致")]
        IDCardPassword = 400152,

        /// <summary>
        /// 密码编码算法与服务器的不一致
        /// </summary>
        [Description("密码编码算法与服务器的不一致")]
        PasswordEncryptError = 400153,

        /// <summary>
        /// 您的密保邮箱（或登录邮箱）已不存在，请重新申请！
        /// </summary>
        [Description("您的密保邮箱（或登录邮箱）已不存在，请重新申请！")]
        EmailNonExist = 400154,

        /// <summary>
        /// 您的帐号没有设置密保或绑定登录邮箱、手机，暂时不能修改密码
        /// </summary>
        [Description("您的帐号没有设置密保或绑定登录邮箱、手机，暂时不能修改密码")]
        SecurityLow = 400155,

        /// <summary>
        /// 短信失效，请重新请求发送验证短信
        /// </summary>
        [Description("短信失效，请重新请求发送验证短信")]
        InvalidSms = 400156,

        /// <summary>
        /// 您的帐号没有设置密保邮箱、手机，暂时不能修改密码
        /// </summary>
        [Description("您的帐号没有设置密保邮箱、手机，暂时不能修改密码")]
        CustomSecurityLow = 400157,

        /// <summary>
        /// 系统接口异常
        /// </summary>
        [Description("系统接口异常")]
        ApiException = 400998,

        /// <summary>
        /// 无效的参数值
        /// </summary>
        [Description("无效的参数值")]
        InvalidArguments = 400999,
    }
}
