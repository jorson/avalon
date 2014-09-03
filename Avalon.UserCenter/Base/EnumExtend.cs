using Avalon.Utility;
using System;
using System.Linq;

namespace Avalon.UserCenter
{
    public static class EnumExtend
    {
        public static bool IsDefined<TEnum>(this TEnum e) where TEnum : struct
        {
            var t = typeof(TEnum);
            if (t.IsEnum)
            {
                return Enum.IsDefined(t, e);
            }
            return false;
        }

        public static void CheckEnum<TEnum>(this TEnum e,string msg=null) where TEnum : struct
        {
            var t = typeof(TEnum);
            if (!t.IsEnum) return;
            if (!Enum.IsDefined(t, e))
            {
                if (msg.IsNullOrWhiteSpace())
                {
                    msg = "无效的枚举数值";
                }
                throw new AvalonException(msg);
            }
        }
    }
}