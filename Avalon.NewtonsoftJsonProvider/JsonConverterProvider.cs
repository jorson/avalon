using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.NewtonsoftJsonProvider
{
    public class JsonConverterProvider : JsonConverter.IJsonConverterProvider
    {
        /// <summary>
        /// 
        /// </summary>
        static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            ContractResolver = new PublicContractResolver()
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string ToJson(object entity)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.None, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T FromJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object FromJson(string json, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type, settings);
        }
    }
}
