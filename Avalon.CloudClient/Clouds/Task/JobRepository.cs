using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public interface IJobRepository : IHttpRepository<Job>
    {
        /// <summary>
        /// 创建邮件Job
        /// </summary>
        /// <param name="toAddresses">收件人的邮件地址集合</param>
        /// <param name="body">邮件正文</param>
        /// <param name="subject">邮件的主题</param>
        /// <param name="smtp">SMTP 服务器</param>
        /// <param name="port">SMTP 服务器端口</param>
        /// <param name="fromAddress">发信人邮件地址</param>
        /// <param name="displayName">发信人显示名</param>
        /// <param name="passWord">发信人密码</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="enableSsl">SMTP 是否使用安全套接字层 (SSL) 加密连接</param>
        /// <returns>job</returns>
        Job CreateEmail(IList<string> toAddresses
            ,string body
            ,string subject=null
            ,string smtp=null
            ,int? port=null
            ,string fromAddress=null
            ,string displayName=null
            ,string passWord=null
            ,bool isBodyHtml=false
            ,bool enableSsl=false
            );

        /// <summary>
        /// 创建短信Job
        /// </summary>
        /// <param name="mobile">号码</param>
        /// <param name="content">短信内容</param>
        /// <param name="remark">备注</param>
        /// <returns>job</returns>
        Job CreateSms(string mobile, string content, string remark = null);
    }

    public class JobRepository : ReadonlyHttpRepository<Job>, IJobRepository
    {
        public override string BasePath
        {
            get { return "task/job"; }
        }

        Job IJobRepository.CreateEmail(IList<string> toAddresses, string body, string subject, string smtp, int? port, string fromAddress, string displayName, string passWord, bool isBodyHtml, bool enableSsl)
        {
            var url = GetRequestUri("createemail");
            var nvs = new NameValueCollection();
            foreach (var toAddress in toAddresses)
            {
                nvs.Add("toAddresses", toAddress);
            }
            nvs.Add("body", body);

            if (subject != null)
                nvs.Add("subject", subject);

            if (smtp != null)
                nvs.Add("smtp", smtp);

            if (port != null)
                nvs.Add("port", port.Value.ToString());

            if (fromAddress != null)
                nvs.Add("fromAddress", fromAddress);

            if (displayName != null)
                nvs.Add("displayName", displayName);

            if (passWord != null)
                nvs.Add("passWord", passWord);

            if (passWord != null)
                nvs.Add("passWord", passWord);

            nvs.Add("isBodyHtml", isBodyHtml.ToString().ToLower());
            nvs.Add("enableSsl", enableSsl.ToString().ToLower());

            return HttpPost<Job>(url, nvs);
        }

        Job IJobRepository.CreateSms(string mobile, string content, string remark)
        {
            var url = GetRequestUri("createsms");
            var nvs = new NameValueCollection {{"mobile", mobile}, {"content", content}};
            if (remark != null)
            {
                nvs.Add("remark", remark);
            }
            return HttpPost<Job>(url, nvs);
        }

        
    }
}
