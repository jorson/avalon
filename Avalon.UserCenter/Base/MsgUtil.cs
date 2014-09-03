using Avalon.CloudClient;
using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public static class MsgUtil
    {
        private static readonly JobService _jobService = DependencyResolver.Resolve<JobService>();
        public static Job SendSms(string mobile, string content, string remark = null)
        {
            return _jobService.CreateSms(mobile, content, remark);
        }

        public static Job SendEmail(IList<string> toAddress, string body, string subject,string displayName = null,bool isHtmlBody=true)
        {
            return _jobService.CreateEmail(toAddress, body, subject, AucConfig.SendmailSmtp, AucConfig.SendmailPort
                ,AucConfig.SendmailFromAddress,displayName,AucConfig.SendmailPassword,isHtmlBody,AucConfig.SendmailEnableSsl);
        }

        public static Job SendEmail(string toAddress, string body, string subject,string displayName = null, bool isHtmlBody = true)
        {
            return SendEmail(new[] { toAddress }, body, subject,displayName, isHtmlBody);
        }

        public static void ValidTemplateParam(this string template, string param)
        {
            if (!template.Contains(param))
            {
                throw new AvalonException("{0}模板参数不存在", param);
            }
        }
    }
}
