using Avalon.Framework;
using Avalon.Framework.Querys;
using Avalon.OAuthClient;
using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon.CloudClient
{
    public abstract class ReadonlyHttpRepository<TEntity> : AbstractHttpRepository<TEntity>
    {
        public override void Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class AbstractHttpRepository<TEntity> : IHttpRepository<TEntity>
    {
        OAuthHttpClient httpClient = new OAuthHttpClient(60000);

        public AbstractHttpRepository()
        {
            httpClient.Host = ClientContext.ApiHost;
        }

        public virtual string BasePath
        {
            get { return typeof(TEntity).Name.ToLower(); }
        }

        private Version _versionPath = Version.v1;
        public virtual Version VersionPath
        {
            set { _versionPath = value; }
            get { return _versionPath; }
        }

        protected virtual OAuthHttpClient HttpClient
        {
            get { return httpClient; }
        }

        public virtual void Create(TEntity entity)
        {
            var url = GetRequestUri("create");
            var data = EntityUtil.ToData(entity);
            OnCreate(entity, data);

            var newEntity = HttpPost<TEntity>(url, data);
            newEntity.DeepClone(entity);
        }

        protected virtual void OnCreate(TEntity entity, NameValueCollection data)
        {

        }

        public virtual void Update(TEntity entity)
        {
            var url = GetRequestUri("update");
            var data = EntityUtil.ToData(entity);
            data.Add(GetIdName<TEntity>(), Avalon.Utility.EntityUtil.GetId(entity).ToString());
            OnUpdate(entity, data);

            var newEntity = HttpPost<TEntity>(url, data);
            newEntity.DeepClone(entity);
        }

        protected virtual void OnUpdate(TEntity entity, NameValueCollection data)
        {

        }

        public virtual void Delete(TEntity entity)
        {
            var id = Avalon.Utility.EntityUtil.GetId(entity);
            var url = GetRequestUri("delete");
            NameValueCollection data = new NameValueCollection();
            data.Add(GetIdName<TEntity>(), id.ToString());
            OnDelete(entity, data);

            HttpGet<TEntity>(url, data);
        }

        protected virtual void OnDelete(TEntity entity, NameValueCollection data)
        {

        }

        public virtual TEntity Get(object id)
        {
            var url = GetRequestUri("get");
            url = UriPath.AppendArguments(url, GetIdName<TEntity>(), id);
            return HttpGet<TEntity>(url);
        }

        public virtual IList<TEntity> GetList(IEnumerable ids)
        {
            return GetList(ShardParams.Empty, ids);
        }

        public virtual IList<TEntity> GetList(ShardParams shardParams, IEnumerable ids)
        {
            var url = GetRequestUri("list");
            var idsName = GetIdsName<TEntity>();

            var data = new NameValueCollection();
            foreach (var id in ids)
            {
                data.Add(idsName, id.ToString());
            }
            if (data.Count == 0)
                return new List<TEntity>();

            return HttpPost<List<TEntity>>(url, data);
        }

        protected virtual void ProcessQuerySpecification(IHttpQuerySpecification spec)
        {
            spec.Url = GetRequestUri("search");
        }

        public IList<TResult> QueryList<TResult>(IQuerySpecification spec)
        {
            ProcessQuerySpecification((IHttpQuerySpecification)spec);
            return spec.ToList<TResult>();
        }

        public PagingResult<TResult> QueryPaging<TResult>(IQuerySpecification spec)
        {
            ProcessQuerySpecification((IHttpQuerySpecification)spec);
            return spec.ToPaging<TResult>();
        }

        public int QueryCount(IQuerySpecification spec)
        {
            ProcessQuerySpecification((IHttpQuerySpecification)spec);
            return spec.Count();
        }

        protected virtual T HttpGet<T>(string url)
        {
            return httpClient.HttpGet<T>(url);
        }

        protected virtual OpenApiResult<T> HttpGetForResult<T>(string url)
        {
            return httpClient.HttpGetForResult<T>(url);
        }

        protected virtual OpenApiResult<T> HttpPostForResult<T>(string url, NameValueCollection data)
        {
            return httpClient.HttpPostForResult<T>(url, data);
        }

        protected virtual OpenApiResult<T> HttpPostForResult<T>(string url, object values)
        {
            return httpClient.HttpPostForResult<T>(url, values);
        }

        protected virtual T HttpGet<T>(string url, NameValueCollection data)
        {
            return httpClient.HttpGet<T>(url, data);
        }

        protected virtual T HttpPost<T>(string url, NameValueCollection data)
        {
            return httpClient.HttpPost<T>(url, data);
        }

        protected virtual T HttpPost<T>(string url, object values)
        {
            return httpClient.HttpPost<T>(url, values);
        }

        protected virtual string GetIdName<T>()
        {
            var name = typeof(T).Name;
            if (Char.IsUpper(name[0]))
                return name[0].ToString().ToLower() + name.Substring(1) + "Id";
            return name + "Id";
        }

        protected virtual string GetIdsName<T>()
        {
            var name = typeof(T).Name;
            var os = name.Where(o => Char.IsUpper(o));
            return new string(os.ToArray()) + "ids";
        }

        protected virtual string GetRequestUri(params string[] args)
        {
            var frames = new List<string>() { string.Format("{0}/{1}", ClientContext.ApiPath, Version.v1.ToName()), BasePath };
            frames.AddRange(args);
            return UriPath.Combine(frames.ToArray());
        }

        protected virtual string GetRequestUriV2(params string[] args)
        {
            var frames = new List<string>() { string.Format("{0}/{1}", ClientContext.ApiPath, Version.v2.ToName()), BasePath };
            frames.AddRange(args);
            return UriPath.Combine(frames.ToArray());
        }

        ISpecification<TEntity> IRepository<TEntity>.CreateSpecification()
        {
            throw new NotImplementedException();
        }

        ISpecification<TEntity> IRepository<TEntity>.CreateSpecification(ShardParams shardParams)
        {
            throw new NotImplementedException();
        }

        void IRepository<TEntity>.Create(TEntity entity)
        {
            Create(entity);
        }

        void IRepository<TEntity>.Create(TEntity entity, object id)
        {
            throw new NotImplementedException();
        }

        void IRepository<TEntity>.Update(TEntity entity)
        {
            Update(entity);
        }

        void IRepository<TEntity>.Delete(TEntity entity)
        {
            Delete(entity);
        }

        void IRepository<TEntity>.SessionEvict(TEntity entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<TEntity>.SessionContains(TEntity entity)
        {
            throw new NotImplementedException();
        }

        TEntity IRepository<TEntity>.Get(ShardParams shardParams, object id)
        {
            return Get(id);
        }

        TEntity IRepository<TEntity>.FindOne(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        IList<TEntity> IRepository<TEntity>.FindAll(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        PagingResult<TEntity> IRepository<TEntity>.FindPaging(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        int IRepository<TEntity>.Count(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }
    }
}
