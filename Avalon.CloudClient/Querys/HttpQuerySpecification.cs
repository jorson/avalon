using Avalon.Framework.Querys;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.CloudClient
{
    public interface IHttpQuerySpecification : IQuerySpecification
    {
        string Url { get; set; }

        NameValueCollection Datas { get; }
    }

    internal class HttpQuerySpecification<T> : IHttpQuerySpecification, IQuerySpecification<T>, IQueryOrderedSpecification<T>
    {
        NameValueCollection datas = null;

        public HttpQuerySpecification(IQuerySpecificationProvider provider, Expression expression = null)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (!(provider is HttpQuerySpecificationProvider))
                throw new ArgumentNullException("给定的 provider 非 HttpQuerySpecificationProvider 类型");

            Provider = provider;
            if (expression == null)
                expression = Expression.Constant(this);
            Expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get;
            private set;
        }

        public IQuerySpecificationProvider Provider
        {
            get;
            private set;
        }

        public NameValueCollection Datas
        {
            get
            {
                if (datas == null)
                    datas = new NameValueCollection();
                return datas;
            }
        }

        public string Url
        {
            get;
            set;
        }
    }
}
