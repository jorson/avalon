using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class DescriptionExtend
    {
        static ConcurrentDictionary<Type, Dictionary<string, string>> enumDics = new ConcurrentDictionary<Type, Dictionary<string, string>>();

        public static string GetDescription<T>(this T enumValue) where T : struct
        {
            Type type = enumValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumValue must be of Enum type", "enumValue");
            }

            var dic = GetEnumDic(type);
            return dic.TryGetValue(enumValue.ToString());
        }

        static Dictionary<string, string> GetEnumDic(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type of Enum type", "type");

            var dic = enumDics.GetOrAdd(type, t =>
            {
                return t.GetFields().ToDictionary(o => o.Name, o =>
                {
                    var attr = (DescriptionAttribute)o.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                    if (attr != null)
                        return attr.Description;

                    return o.Name;
                });
            });

            return dic;
        }
    }
}
