using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public class StoreObjectService : IService
    {
        private readonly IStoreObjectRepository _storeObjectRepository;

        public StoreObjectService(IStoreObjectRepository storeObjectRepository)
        {
            _storeObjectRepository = storeObjectRepository;
        }

        public StoreObject Get(int id)
        {
            return _storeObjectRepository.Get(id);
        }

        public IList<StoreObject> List(IEnumerable<int> ids)
        {
            return _storeObjectRepository.List(ids);
        }

        public StoreObjectUrl GetUrl(int id)
        {
            return _storeObjectRepository.GetUrl(id);
        }
    }
}
