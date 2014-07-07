using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RoutePathAttribute : Attribute
    {
        public RoutePathAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}
