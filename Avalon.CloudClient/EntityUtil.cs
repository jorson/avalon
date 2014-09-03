using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public static class EntityUtil
    {
        public static TEntity ProcessResult<TEntity>(string json)
        {
            if (json == "null")
                return default(TEntity);

            var result = JsonConverter.FromJson<OpenApiResult<TEntity>>(json);
            if (result.Code != 0)
                throw new AvalonException(result.Message) { Code = result.Code };

            return result.Data;
        }

        public static NameValueCollection ToData<TEntity>(TEntity entity)
        {
            NameValueCollection data = new NameValueCollection();
            if (entity != null)
            {
                TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TEntity));
                var properties = ta.ReadWriteProperties;
                var values = ta.GetReadWritePropertyValues(entity);
                for (var i = 0; i < properties.Count; i++)
                {
                    var v = values[i];
                    if (v != null)
                        data.Add(properties[i].Name, v.ToString());
                }
            }
            return data;
        }
    }
}
