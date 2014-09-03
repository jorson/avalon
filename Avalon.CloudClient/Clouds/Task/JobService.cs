using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public class JobService : IService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

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
        public Job CreateEmail(IList<string> toAddresses
            , string body
            , string subject = null
            , string smtp = null
            , int? port = null
            , string fromAddress = null
            , string displayName = null
            , string passWord = null
            , bool isBodyHtml = false
            , bool enableSsl = false
            )
        {
            return _jobRepository.CreateEmail(toAddresses, body, subject, smtp, port, fromAddress, displayName, passWord,
                isBodyHtml, enableSsl);
        }

        /// <summary>
        /// 创建短信Job
        /// </summary>
        /// <param name="mobile">号码</param>
        /// <param name="content">短信内容</param>
        /// <param name="remark">备注</param>
        /// <returns>job</returns>
        public Job CreateSms(string mobile, string content, string remark = null)
        {
            return _jobRepository.CreateSms(mobile, content, remark);
        }
    }
}
