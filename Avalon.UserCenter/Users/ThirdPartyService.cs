using System;
using System.Linq.Expressions;
using Avalon.UserCenter.Models;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Avalon.Framework;
using Avalon.HttpClient;

namespace Avalon.UserCenter
{
    public class ThirdPartyService : IService
    {
        private readonly AppBaseService _appBaseService;


        public ThirdPartyService(AppBaseService appBaseService)
        {
            _appBaseService = appBaseService;
        }

        public ThirdOAuthClient GetAppThirdOAuthClient(Solution solution, int appId)
        {
            if (solution != null && solution.Type != SolutionType.Custom && solution.Status == SolutionStatus.Normal)
            {
                var isEnable = _appBaseService.GetWebSiteThirdAccountEnable(solution.Type, appId);
                if (isEnable)
                {
                    var setting = _appBaseService.GetWebSiteThirdSetting(solution.Type, appId);
                    switch (solution.Type)
                    {
                        case SolutionType.SinaWeibo:
                            return new SinaWeiboClient(setting);
                        case SolutionType.QQAccount:
                            return new QQAccountClient(setting);
                    }
                }
            }
            return null;
        }

        public ThirdTokenClient GetThirdTokenClient(Solution solution, string token)
        {
            if (solution != null && solution.Type != SolutionType.Custom && solution.Status == SolutionStatus.Normal)
            {

                switch (solution.Type)
                {
                    case SolutionType.SinaWeibo:
                        return new SinaWeiboTokenClient(token);
                    case SolutionType.QQAccount:
                        return new QQAccountTokenClient(token);
                    case SolutionType.Uap:
                        return new UapTokenClient(GetUapClient(solution),token);
                }
            }
            return null;
        }

        public UapClient GetUapClient(Solution solution)
        {
            if (solution != null && solution.Type == SolutionType.Uap && solution.Status == SolutionStatus.Normal)
            {

                switch (solution.Type)
                {
                    case SolutionType.Uap:
                        return new UapForNdClient(solution);
                }
            }
            return null;
        }

        public IdStarClient GetIdStarClient(Solution solution)
        {
            if (solution != null && solution.Type == SolutionType.IdStar && solution.Status == SolutionStatus.Normal)
            {
                return new IdStarClient(solution);
            }
            return null;
        }

    }

    public interface IThirdTokenInfo
    {
        /// <summary>
        /// 授权令牌
        /// </summary>
        string Token { get; }


        /// <summary>
        /// 帐号开放标识
        /// </summary>
        string OpenId { get; }
    }

    public interface IThirdAppKeyAndOpenId
    {
        /// <summary>
        /// 帐号开放标识
        /// </summary>
        string OpenId { get; }
        /// <summary>
        /// 应用标识
        /// </summary>
        string AppKey { get; }
    }

    public interface IThirdAccountInfo
    {
        string NickName { get; set; }
    }

    public class ThirdOAuthSetting
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }
    }

    /// <summary>
    /// 第三方客户端
    /// </summary>
    public abstract class ThirdOAuthClient
    {
        protected ThirdOAuthClient(ThirdOAuthSetting setting)
        {
            Setting = setting;
        }

        /// <summary>
        /// oauth2 设置
        /// </summary>
        public ThirdOAuthSetting Setting { get; protected set; }

        protected Avalon.HttpClient.HttpClient HttpClient = new Avalon.HttpClient.HttpClient();

        /// <summary>
        /// 获取强制登录授权页Url
        /// </summary>
        /// <param name="redirect_uri">返回uri</param>
        /// <returns>登录授权页Url</returns>
        public abstract string GetForceAuthorizeUrl(string redirect_uri);

        /// <summary>
        /// 获取登录授权页Url
        /// </summary>
        /// <param name="redirect_uri">返回uri</param>
        /// <returns>登录授权页Url</returns>
        public abstract string GetAuthorizeUrl(string redirect_uri);

        /// <summary>
        /// 通过授权码和返回uri获得第三方token与openId
        /// </summary>
        /// <param name="code">授权码</param>
        /// <param name="redirect_uri">返回uri</param>
        /// <returns></returns>
        public abstract IThirdTokenInfo GetThirdToken(string code, string redirect_uri);


        /// <summary>
        /// 通过第三方token与openId获取第三方帐号信息
        /// </summary>
        /// <param name="tokenAndOpenId">第三方token与openId</param>
        /// <returns></returns>
        public abstract IThirdAccountInfo GeThirdAccountInfo(IThirdTokenInfo tokenAndOpenId);


    }

    /// <summary>
    /// 第三方token客户端
    /// </summary>
    public abstract class ThirdTokenClient
    {
        protected ThirdTokenClient(string token)
        {
            Token = token;
        }

        protected Avalon.HttpClient.HttpClient HttpClient = new Avalon.HttpClient.HttpClient();

        public string Token { get; protected set; }

        /// <summary>
        /// 第三方应用标识与帐号开放标识
        /// </summary>
        /// <returns></returns>
        public abstract IThirdAppKeyAndOpenId AppKeyAndOpenId { get; }

        /// <summary>
        /// 第三方帐号信息
        /// </summary>
        /// <returns></returns>
        public abstract IThirdAccountInfo ThirdAccountInfo { get; }
    }

    

    

    /// <summary>
    /// 第三方信息
    /// </summary>
    public class ThirdInfo
    {
        /// <summary>
        /// 授权令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}