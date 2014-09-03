using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon.UserCenter
{
    public static class AucConfig
    {
        /// <summary>
        /// 云存在上传Url
        /// </summary>
        public static readonly string CloudUploadUrl = GetAppSetting("cloud.upload.Url");
        public static readonly string DefualtBucketName = GetAppSetting("cloud.upload.DefualtBucketName");
        public static readonly string AvatarImageBucketName = GetAppSetting("cloud.upload.AvatarImageBucketName");
        public static readonly string IdCardImageBucketName = GetAppSetting("cloud.upload.IdCardImageBucketName");

        /// <summary>
        /// 密码找回Token过期分钟数
        /// </summary>
        public static readonly int RecoverTokenExpireMinutes = GetAppSettingForInt("AUC.RecoverTokenExpireMinutes");

        public static readonly string SendmailSmtp = GetAppSetting("sendmail.smtp");
        public static readonly int SendmailPort = GetAppSettingForInt("sendmail.port");
        public static readonly string SendmailFromAddress = GetAppSetting("sendmail.fromAddress");
        public static readonly string SendmailPassword = GetAppSetting("sendmail.passWord");
        public static readonly bool SendmailEnableSsl = GetAppSettingForBoolean("sendmail.enableSsl");

        /// <summary>
        /// auc的cloud的appid
        /// </summary>
        public static readonly int AucAppId = GetAppSettingForInt("OAuth.CredentialsClient.ClientId");


        public static readonly int PicVerifyCodeExpireMinutes = GetAppSettingForInt("AUC.PicVerifyCodeExpireMinutes");
        public static readonly int MobileVerifyCodeExpireMinutes = GetAppSettingForInt("AUC.MobileVerifyCodeExpireMinutes");
        public static readonly int MobileVerifyCodeVerifyTimes = GetAppSettingForInt("AUC.MobileVerifyCodeVerifyTimes");
        public static readonly int EmailVerifyCodeExpireMinutes = GetAppSettingForInt("AUC.EmailVerifyCodeExpireMinutes");
        /// <summary>
        /// 图片验证统计信息生存秒数
        /// </summary>
        public static readonly int KeepPicVerifyStatSeconds = GetAppSettingForInt("AUC.KeepPicVerifyStatSeconds");

        /// <summary>
        /// 手机验证统计信息生存秒数
        /// </summary>
        public static readonly int KeepMobileVerifyStatSeconds = GetAppSettingForInt("AUC.KeepMobileVerifyStatSeconds");
        //当前IP N秒内注册成功了M次
        public static readonly int PicVerifyRegisterLimitSeconds = GetAppSettingForInt("AUC.PicVerifyRegisterLimitSeconds");
        public static readonly int PicVerifyRegisterLimitTimes = GetAppSettingForInt("AUC.PicVerifyRegisterLimitTimes");
        //当前IP N秒内登录失败了M次
        public static readonly int PicVerifyLogonLimitSeconds = GetAppSettingForInt("AUC.PicVerifyLogonLimitSeconds");
        public static readonly int PicVerifyLogonLimitTimes = GetAppSettingForInt("AUC.PicVerifyLogonLimitTimes");
        //一个手机号，n秒内限制发送1条
        public static readonly int MobileVerifyOneLimitSeconds = GetAppSettingForInt("AUC.MobileVerifyOneLimitSeconds");
        //一个手机号，n秒内不能超过最大上限条数
        public static readonly int MobileVerifyNoLimitSeconds = GetAppSettingForInt("AUC.MobileVerifyNoLimitSeconds");
        public static readonly int MobileVerifyNoLimitNumbers = GetAppSettingForInt("AUC.MobileVerifyNoLimitNumbers");
        //一个ip，在n秒内只能发m个手机验证码
        public static readonly int MobileVerifyIpLimitSeconds = GetAppSettingForInt("AUC.MobileVerifyIpLimitSeconds");
        public static readonly int MobileVerifyIpLimitNumbers = GetAppSettingForInt("AUC.MobileVerifyIpLimitNumbers");

        public static readonly int UserPasswordErrorMaxCount = GetAppSettingForInt("AUC.UserPasswordErrorMaxCount");
        public static readonly int UserPasswordErrorStatExpireMinutes = GetAppSettingForInt("AUC.UserPasswordErrorStatExpireMinutes");
        public static readonly int UserPasswordErrorAutoFrozenMinutes = GetAppSettingForInt("AUC.UserPasswordErrorAutoFrozenMinutes");


        public static readonly int UserVeriyCodeRepeateSendMaxCount = GetAppSettingForInt("AUC.UserVeriyCodeRepeateSendMaxCount");
        public static readonly int UserVeriyCodeRepeateSendStatExpireSeconds = GetAppSettingForInt("AUC.UserVeriyCodeRepeateSendStatExpireSeconds");
        public static readonly int UserVeriyCodeRepeateSendLimitIntervalMinutes = GetAppSettingForInt("AUC.UserVeriyCodeRepeateSendLimitIntervalMinutes");



        private static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name] ?? string.Empty;
        }

        private static int GetAppSettingForInt(string name)
        {
            int v;
            Int32.TryParse(ConfigurationManager.AppSettings[name], out v);
            return v;
        }

        private static long GetAppSettingForLong(string name)
        {
            long v;
            long.TryParse(ConfigurationManager.AppSettings[name], out v);
            return v;
        }

        private static bool GetAppSettingForBoolean(string name)
        {
            bool v;
            Boolean.TryParse(ConfigurationManager.AppSettings[name], out v);
            return v;
        }
    }
}
