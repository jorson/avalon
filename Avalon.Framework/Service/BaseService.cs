using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class BaseService<T> : IService where T : class
    {
        public IRepository<T> repository;

        protected BaseService(IRepository<T> repository)
        {
            this.repository = repository;
        }
        protected void ValidRead(IEnumerable<T> entities)
        {
            if (entities != null)
                entities.ForEach(o => ValidRead(o));
        }
        protected virtual void ValidWrite(T entity)
        {
            if (entity != null)
            {
                var appid = AppIdGetter(entity);
                OauxOAuthContext.Current.ValidWrite(appid);

                //var targetAppId = CloudOAuthContext.Current.TargetAppId;
                //if (targetAppId != appid)
                //    throw new PlatformException("当期的应用标识为 {0} ,但尝试编辑的对象的应用标识为 {1}", targetAppId, appid);
            }
        }
        protected void ValidWrite(IEnumerable<T> entities)
        {
            if (entities != null)
                entities.ForEach(o => ValidWrite(o));
        }
        protected T GetEntity(int id)
        {
            var item = repository.Get(ShardParams.Empty, id);
            ValidRead(item);
            return item;
        }
        protected IList<T> GetEntityList(IEnumerable<int> ids)
        {
            var items = repository.GetList(ShardParams.Empty, ids);
            ValidRead(items);
            return items;
        }
        protected void CreateEntity(T entity)
        {
            ValidWrite(entity);
            repository.Create(entity);
        }

        protected void UpdateEntity(T entity)
        {
            ValidWrite(entity);
            repository.Update(entity);
        }

        protected void DeleteEntity(T entity)
        {
            ValidWrite(entity);
            repository.Delete(entity);
        }
        protected virtual ISpecification<T> GetSpecification()
        {
            var spec = repository.CreateSpecification();
            return spec;
        }
    }
}
