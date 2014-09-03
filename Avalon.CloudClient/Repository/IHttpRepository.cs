using Avalon.Framework;
using Avalon.Framework.Querys;
using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon.CloudClient
{
    public interface IHttpRepository<TEntity> : IRepository<TEntity>
    {
        /// <summary>
        /// 基本路径
        /// </summary>
        string BasePath { get; }

        TEntity Get(object id);

        IList<TEntity> GetList(IEnumerable ids);

        IList<TResult> QueryList<TResult>(IQuerySpecification spec);

        PagingResult<TResult> QueryPaging<TResult>(IQuerySpecification spec);

        int QueryCount(IQuerySpecification spec);
    }
}
