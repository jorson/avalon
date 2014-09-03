
using Avalon.Framework;
namespace Avalon.UserCenter
{
    public interface IWebSiteBaseSettingRepository : INoShardRepository<WebSiteBaseSetting>
    {
         
    }

    public interface IIpAddressCityRepository : INoShardRepository<IpAddressCity>
    {
        
    }

    public interface IIpCityRepository : INoShardRepository<IpCity>
    {
        
    }

    public interface IIpProvinceRepository : INoShardRepository<IpProvince>
    {
        
    }
}