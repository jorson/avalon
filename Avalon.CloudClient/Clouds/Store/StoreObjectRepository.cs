using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public interface IStoreObjectRepository : IHttpRepository<StoreObject>
    {
        StoreObject Get(int id);

        IList<StoreObject> List(IEnumerable<int> ids);

        StoreObjectUrl GetUrl(int id);
    }

    public class StoreObjectRepository : AbstractHttpRepository<StoreObject>, IStoreObjectRepository
    {
        public override string BasePath
        {
            get { return "store/object"; }
        }

        StoreObject IStoreObjectRepository.Get(int id)
        {
            var url = GetRequestUri("get");
            var nvs = new NameValueCollection { { "id", id.ToString()} };
            return HttpPost<StoreObject>(url, nvs);
        }

        IList<StoreObject> IStoreObjectRepository.List(IEnumerable<int> ids)
        {
            var url = GetRequestUri("list");
            var nvs = new NameValueCollection();
            foreach (var id in ids)
            {
                nvs.Add("ids", id.ToString());
            }

            return HttpPost<IList<StoreObject>>(url, nvs);
        }

        StoreObjectUrl IStoreObjectRepository.GetUrl(int id)
        {
            var url = GetRequestUri("Url");
            var nvs = new NameValueCollection { { "id", id.ToString() } };
            return HttpPost<StoreObjectUrl>(url, nvs);
        }
    }
}
