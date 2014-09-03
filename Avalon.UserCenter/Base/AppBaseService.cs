using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.UserCenter.Models;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    public class AppBaseService : IService
    {
        /// <summary>
        /// 邮件模板类型发送者显示名称键名格式
        /// </summary>
        private const string EmailTplTypeSenderFmtKey = "EmailSenderTlp{0}";
        /// <summary>
        /// 邮件模板类型邮件标题键名格式
        /// </summary>
        private const string EmailTplTypeSubjectFmtKey = "EmailSubjectTlp{0}";
        /// <summary>
        /// 邮件模板类型正文键名格式
        /// </summary>
        private const string EmailTplTypeBodyFmtKey = "EmailBodyTlp{0}";

        /// <summary>
        /// 短信模板正文键名格式
        /// </summary>
        private const string SMS_TPLTYPE_FMTKEY = "SmsTlp{0}";

        /// <summary>
        /// 应用图片验证码显示格式的键名格式
        /// </summary>
        private const string PicVerifyCodeDisplayFormatKey = "app{0}_PicVerifyCodeDisplayFormat";
        /// <summary>
        /// 应用注册图片验证码出现规则的键名格式
        /// </summary>
        private const string RegisterVerifyCodeAppearRuleKey = "app{0}_RegisterVerifyCodeAppearRule";
        /// <summary>
        /// 应用登录图片验证码出现规则的键名格式
        /// </summary>
        private const string LoginVerifyCodeAppearRuleKey = "app{0}_LoginVerifyCodeAppearRule";
        /// <summary>
        /// 应用的某种第三方帐号方案类型的启用开关的键名格式
        /// </summary>
        private const string ThirdAccountEnableFmtKey = "app{1}_ThirdAccountEnable{0}";
        /// <summary>
        /// 应用的某种第三方帐号方案类型的第三方自身应用标识的键名格式
        /// </summary>
        private const string ThirdSettingAppKeyFmtKey = "app{1}_ThirdSettingAppKey{0}";
        /// <summary>
        /// 应用的某种第三方帐号方案类型的第三方自身应用密钥的键名格式
        /// </summary>
        private const string ThirdSettingAppSecretFmtKey = "app{1}_ThirdSettingAppSecret{0}";

        /// <summary>
        /// 通行证网站AccessToken缓存
        /// </summary>
        private const string WebSiteAccessTokenCacheKey = "cache:site:WebSiteAccesstokenCache";
        /// <summary>
        /// 通行证网站logo缓存
        /// </summary>
        private const string WebSiteLogoCacheKey = "cache:site:WebSiteLogoCache";

        private readonly IWebSiteBaseSettingRepository _webSiteBaseSettingRepository;
        private readonly IKeyValueRepository _keyValueRepository;
        private readonly IIpAddressCityRepository _ipAddressRepository;
        private readonly IIpCityRepository _ipCityRepository;
        private readonly IIpProvinceRepository _ipProvinceRepository;
        private CacheDomain<List<IpProvince>> provinceCacheDomain;
        private CacheDomain<List<IpCity>, int> cityCacheDomain; 

        public AppBaseService(IWebSiteBaseSettingRepository webSiteBaseSettingRepository
            , IKeyValueRepository keyValueRepository, IIpAddressCityRepository ipAddressRepository
            ,IIpCityRepository ipCityRepository,IIpProvinceRepository ipProvinceRepository)
        {
            _webSiteBaseSettingRepository = webSiteBaseSettingRepository;
            _keyValueRepository = keyValueRepository;
            _ipAddressRepository = ipAddressRepository;
            _ipCityRepository = ipCityRepository;
            _ipProvinceRepository = ipProvinceRepository;
            provinceCacheDomain = CacheDomain.CreateSingleton(
                GetProvinces,
                "provincelist",
                "provincelist");
            cityCacheDomain = CacheDomain.CreateSingleKey<List<IpCity>, int>(
                o => o[0].ProvinceId,
                GetCities,
                null,
                "citylist",
                "citylist:{0}");
        }

        public PicVerifyCodeDisplayFormat GetPicVerifyCodeDisplayFormat(int appId)
        {
            PicVerifyCodeDisplayFormat displayFormat;
            var value = _webSiteBaseSettingRepository.Get(string.Format(PicVerifyCodeDisplayFormatKey, appId)).GetOrDefault(kv => kv.Value);
            if (!Enum.TryParse(value, out displayFormat))
                displayFormat = PicVerifyCodeDisplayFormat.OnlyDigits;

            return displayFormat;
        }

        public VerifyCodeAppearRule GetRegisterVerifyCodeAppearRule(int appId)
        {
            VerifyCodeAppearRule verifyCodeRuleType;
            var value =_webSiteBaseSettingRepository.Get(string.Format(RegisterVerifyCodeAppearRuleKey, appId)).GetOrDefault(kv => kv.Value);
            if (!Enum.TryParse(value, out verifyCodeRuleType))
                verifyCodeRuleType = VerifyCodeAppearRule.Need;

            return verifyCodeRuleType;
        }

        public VerifyCodeAppearRule GetLoginVerifyCodeAppearRule(int appId)
        {
            VerifyCodeAppearRule verifyCodeRuleType;
            var value =
                _webSiteBaseSettingRepository.Get(string.Format(LoginVerifyCodeAppearRuleKey, appId)).GetOrDefault(kv => kv.Value);
            var isOk = Enum.TryParse(value, out verifyCodeRuleType);
            if (!isOk)
                verifyCodeRuleType = VerifyCodeAppearRule.Judge;

            return verifyCodeRuleType;
        }

        /// <summary>
        /// 获取华渔通行证网站邮件模板
        /// </summary>
        /// <param name="templateType"></param>
        /// <returns></returns>
        public WebSiteEmailTemplate GetWebSiteEmailTemplate(TemplateType templateType)
        {
            var rt = new WebSiteEmailTemplate
            {
                SenderDispalyName =
                    _webSiteBaseSettingRepository.Get(string.Format(EmailTplTypeSenderFmtKey, (int) templateType))
                        .GetOrDefault(kv => kv.Value),
                Subject =
                    _webSiteBaseSettingRepository.Get(string.Format(EmailTplTypeSubjectFmtKey, (int) templateType))
                        .GetOrDefault(kv => kv.Value),
                BodyTemplate =
                    _webSiteBaseSettingRepository.Get(string.Format(EmailTplTypeBodyFmtKey, (int) templateType))
                        .GetOrDefault(kv => kv.Value)
            };
            return rt;
        }

        /// <summary>
        /// 获取华渔通行证网站短信模板
        /// </summary>
        /// <param name="templateType"></param>
        /// <returns></returns>
        public string GetWebSiteSmsTemplate(TemplateType templateType)
        {
            return _webSiteBaseSettingRepository.Get(string.Format(SMS_TPLTYPE_FMTKEY, (int)templateType))
                .GetOrDefault(kv => kv.Value);
        }

        /// <summary>
        /// 获取华渔通行证网站第三方帐号功能是否开启
        /// </summary>
        /// <param name="solutionType"></param>
        /// <param name="appId">应用标识</param>
        /// <returns></returns>
        public bool GetWebSiteThirdAccountEnable(SolutionType solutionType,int appId)
        {
            var str = _webSiteBaseSettingRepository.Get(string.Format(ThirdAccountEnableFmtKey, (int)solutionType, appId))
                .GetOrDefault(kv => kv.Value,"false");
            return bool.Parse(str);
        }

        /// <summary>
        /// 获取华渔通行证网站第三方帐号
        /// </summary>
        /// <param name="solutionType"></param>
        /// <param name="appId">应用标识</param>
        /// <returns></returns>
        public ThirdOAuthSetting GetWebSiteThirdSetting(SolutionType solutionType, int appId)
        {
            var rt = new ThirdOAuthSetting
            {
                AppKey =
                    _webSiteBaseSettingRepository.Get(string.Format(ThirdSettingAppKeyFmtKey, (int)solutionType, appId))
                        .GetOrDefault(kv => kv.Value),
                AppSecret =
                    _webSiteBaseSettingRepository.Get(string.Format(ThirdSettingAppSecretFmtKey, (int)solutionType, appId))
                        .GetOrDefault(kv => kv.Value),
            };
            return rt;
        }

        public string GetWebSiteAccessTokenCache()
        {
            var accesstoken=_keyValueRepository.GetValue<string>(WebSiteAccessTokenCacheKey);
            return accesstoken;
        }

        public void SetWebSiteAccesstokenCache(string accessToken, TimeSpan expire)
        {
            _keyValueRepository.SetKeyValue(WebSiteAccessTokenCacheKey, accessToken, expire);
        }

        public string GetWebSiteLogoCache()
        {
            var logoData = _keyValueRepository.GetValue<string>(WebSiteLogoCacheKey);
            return logoData;
        }

        public void SetWebSiteLogoCache(string logoData, TimeSpan expire)
        {
            _keyValueRepository.SetKeyValue(WebSiteLogoCacheKey, logoData, expire);
        }

        private List<IpProvince> GetProvinces()
        {
            return _ipProvinceRepository.FindAll(_ipProvinceRepository.CreateSpecification()).ToList();
        }

        private List<IpCity> GetCities(int provinceId)
        {
            var specification = _ipCityRepository.CreateSpecification().Where(c => c.ProvinceId == provinceId);
            return _ipCityRepository.FindAll(specification).ToList();
        }

        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        public IpProvince[] GetIpProvinces()
        {
           return provinceCacheDomain.GetItem().ToArray();
        }

        /// <summary>
        /// 获取相关省份的城市
        /// </summary>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public IpCity[] GetIpCitys(int provinceId)
        {
            return cityCacheDomain.GetItem(provinceId).ToArray();
        }

        /// <summary>
        /// 通过IP地址获取城市ID
        /// </summary>
        /// <param name="ipaddress">IP的整形表达方式</param>
        /// <returns></returns>
        public int GetCityIdByIp(long ipaddress)
        {
           var ipad = _ipAddressRepository.FindOne(
                _ipAddressRepository.CreateSpecification().Where(a => a.StartIp <= ipaddress && a.EndIp >= ipaddress));
            if (ipad == null)
                return 0;
            return ipad.CityId;
        }

        /// <summary>
        /// 根据ID查找省份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IpProvince GetIpProvinceById(int id)
        {
            return _ipProvinceRepository.Get(id);
        }

        /// <summary>
        /// 根据ID查找城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IpCity GetIpCityById(int id)
        {
            return _ipCityRepository.Get(id);
        }
    }
}
