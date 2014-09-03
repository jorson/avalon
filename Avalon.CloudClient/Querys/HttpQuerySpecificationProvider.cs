using Avalon.Framework.Querys;
using Avalon.OAuthClient;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.CloudClient
{
    public class HttpQuerySpecificationProvider : IQuerySpecificationProvider
    {
        static HttpQuerySpecificationProvider instance;
        static MethodInfo countMethod, pagingMethod, listMethod;
        static OAuthHttpClient httpClient = new OAuthHttpClient(60000);

        static HttpQuerySpecificationProvider()
        {
            var type = typeof(QuerySpecificationExtend);
            countMethod = type.GetMethod("Count");
            pagingMethod = type.GetMethod("ToPaging");
            listMethod = type.GetMethod("ToList");

            httpClient.Host = ClientContext.ApiHost;
        }

        public static HttpQuerySpecificationProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new HttpQuerySpecificationProvider();
                return instance;
            }
        }

        public IQuerySpecification<T> CreateSpecification<T>(Expression expression)
        {
            return new HttpQuerySpecification<T>(this, expression);
        }

        public IQuerySpecification<T> CreateSpecification<T>()
        {
            return new HttpQuerySpecification<T>(this);
        }

        public object Execute(Type queryType, MethodInfo method, IQuerySpecification spec)
        {
            if (method == countMethod)
                return ExecuteCount(queryType, (IHttpQuerySpecification)spec);

            throw new NotSupportedException("不支持的方法 " + method.Name);
        }

        public object ExecuteItems<TItem>(Type queryType, MethodInfo method, IQuerySpecification spec)
        {
            if (method == listMethod)
                return ExecuteList<TItem>(queryType, (IHttpQuerySpecification)spec);

            if (method == pagingMethod)
                return ExecutePaging<TItem>(queryType, (IHttpQuerySpecification)spec);

            throw new NotSupportedException("不支持的方法 " + method.Name);
        }

        int ExecuteCount(Type queryType, IHttpQuerySpecification spec)
        {
            LinqToODataStringVisitor vistor = new LinqToODataStringVisitor(spec.Expression);
            var datas = vistor.ODataString.ToNameValueCollection();
            datas.Add("$count", "true");
            datas.Add(spec.Datas);

            return httpClient.HttpPost<int>(spec.Url, datas);
        }

        IList<TItem> ExecuteList<TItem>(Type queryType, IHttpQuerySpecification spec)
        {
            LinqToODataStringVisitor vistor = new LinqToODataStringVisitor(spec.Expression);
            var datas = vistor.ODataString.ToNameValueCollection();
            datas.Add(spec.Datas);

            return httpClient.HttpPost<IList<TItem>>(spec.Url, datas);
        }

        PagingResult<TItem> ExecutePaging<TItem>(Type queryType, IHttpQuerySpecification spec)
        {
            LinqToODataStringVisitor vistor = new LinqToODataStringVisitor(spec.Expression);
            var datas = vistor.ODataString.ToNameValueCollection();
            datas.Add("$inlinecount", "allpages");
            datas.Add(spec.Datas);

            return httpClient.HttpPost<PagingResult<TItem>>(spec.Url, datas);
        }
    }
}
