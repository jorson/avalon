using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public enum Version
    {
        v1,
        v2
    }

    public static partial class EnumHelper
    {
        public static string ToName(this Version version)
        {
            switch (version)
            {
                case Version.v2:
                    return "v2";
                default:
                    return "v1";
            }
        }
    }
}
