using Avalon.Utility;
using ServiceStack.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Avalon.RedisProvider
{
    internal static class ObjectHashMappingUtil
    {
        static Type tupleType;
        const string NullValue = "$<NULL>$";
        const string ValueName = "$<VALUE>$";
        public const string TimeStampName = "$<TIMESTAMP>$";
        static ObjectHashMappingUtil()
        {
            tupleType = Type.GetType("System.ITuple");
        }
        static bool IsSingleValue(Type type)
        {
            //基元类型或集合
            return Type.GetTypeCode(type) != TypeCode.Object || typeof(IEnumerable).IsAssignableFrom(type);
        }
        public static Dictionary<string, string> ToHash(object instance, string keyPrefix = "")
        {
            if (instance == null)
                throw new ArgumentNullException("entity");

            Type type = instance.GetType();
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (instance is EmptyData)
            {
                dic.Add(keyPrefix + ValueName, NullValue);
            }
            else
            {
                //集合整个进行序列化
                if (IsSingleValue(type))
                {
                    dic.Add(keyPrefix + ValueName, ToString(instance, type));
                }
                // for tuple
                else if (tupleType.IsAssignableFrom(type))
                {
                    TypeAccessor ta = TypeAccessor.GetAccessor(type);
                    dic = ta.FieldAccessDic.ToDictionary(o => o.Key, o => ToString(o.Value.Getter(instance), o.Value.Member.FieldType));
                }
                else
                {
                    TypeAccessor ta = TypeAccessor.GetAccessor(type);
                    if (ta.ReadWriteProperties.Count == 0)
                        throw new NotSupportedException(String.Format("给定的对象 {0} 没有任何可读写的属性，无法进行序列化", type.FullName));

                    var values = ta.GetReadWritePropertyValues(instance);
                    int index = 0;
                    foreach (var property in ta.ReadWriteProperties)
                    {
                        dic.Add(keyPrefix + property.Name, ToString(values[index], property.PropertyType));
                        index++;
                    }
                }
            }
            return dic;
        }

        public static string[] GetKeys(Type type, string keyPrefix = "")
        {
            List<string> keys = new List<string>();
            if (IsSingleValue(type))
            {
                keys.Add(keyPrefix + ValueName);
            }
            // for tuple
            else if (tupleType.IsAssignableFrom(type))
            {
                keys.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Select(o => o.Name));
            }
            else
            {
                TypeAccessor ta = TypeAccessor.GetAccessor(type);
                keys.AddRange(ta.ReadWriteProperties.Select(o => keyPrefix + o.Name));
            }
            return keys.ToArray();
        }

        /// <summary>
        /// 将 HASH 数据转为对象，注意第一个指向的为 ValueName 的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stringValues"></param>
        /// <returns></returns>
        public static object ToObject(Type type, string[] stringValues)
        {
            if (stringValues == null || stringValues.Length == 0)
                throw new ArgumentException("stringValues");

            if (stringValues.All(o => String.IsNullOrEmpty(o)))
                return null;

            if (stringValues[0] == NullValue)
                return EmptyData.Value;

            if (IsSingleValue(type))
                return FromString(stringValues[0], type);

            TypeAccessor ta = TypeAccessor.GetAccessor(type);
            if (tupleType.IsAssignableFrom(type))
            {
                var tuple = FormatterServices.GetUninitializedObject(type);
                int i = 0;
                foreach (var entry in ta.FieldAccessDic)
                {
                    var v = FromString(stringValues[i], entry.Value.Member.FieldType);
                    entry.Value.Setter(tuple, v);
                    i++;
                }
                return tuple;
            }

            if (ta.ReadWriteProperties.Count == 0)
                throw new NotSupportedException(String.Format("给定的对象 {0} 没有任何可读写的属性，无法进行序列化", type.FullName));

            object[] values = new object[ta.ReadWriteProperties.Count];
            int index = 0;
            foreach (var property in ta.ReadWriteProperties)
            {
                values[index] = FromString(stringValues[index], property.PropertyType);
                index++;
            }

            object instance = ta.Create();
            ta.SetReadWritePropertyValues(instance, values);

            return instance;
        }


        static string ToString(object value, Type type)
        {
            var str = TypeSerializer.SerializeToString(value, type);
            if (str == null)
                str = NullValue;

            return str;
        }

        static object FromString(string value, Type type)
        {
            if (value == NullValue)
                return null;

            var v = TypeSerializer.DeserializeFromString(value, type);
            if (v != null && type == typeof(DateTime))
            {
                var dv = (DateTime)v;
                v = dv.ToLocalTime();
            }
            return v;
        }
    }
}
