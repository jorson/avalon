using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Avalon.Utility;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ApiAttribute : FilterAttribute, IExceptionFilter
    {
        ILog log = LogManager.GetLogger<ApiAttribute>();

        protected int GetResponseCode(ExceptionContext filterContext)
        {
            int code = ResultCode.InternalServerError;
            if (filterContext.Exception is ArgumentException)
                code = ResultCode.BadRequest;

            if (filterContext.Exception is AvalonException)
                code = ((AvalonException)filterContext.Exception).Code;

            if (filterContext.Exception is BusinessException)
                code = ((BusinessException)filterContext.Exception).Code;
            return code;
        }

        public virtual void OnException(ExceptionContext filterContext)
        {
            var svrIp = filterContext.HttpContext.Request.ServerVariables["Local_Addr"];
            var clientIp = IpAddress.GetIP();
            var ipInfoStr = string.Format(@"
服务器ip：{0}
客户端ip：{1}", svrIp, clientIp);
            var msg = filterContext.Exception.Message + ipInfoStr;

            if (filterContext.Exception is BusinessException)
            {
                if (log.IsInfoEnabled)
                {
                    log.Info(msg, filterContext.Exception);
                }
            }
            else
            {
                if (log.IsErrorEnabled)
                {
                    log.Error(msg, filterContext.Exception);
                }
            }
        }
    }
}