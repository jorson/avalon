using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 描述API响应码的信息，用于接口文档的生成
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ResponseCodeAttribute : Attribute
    {
        public ResponseCodeAttribute(Type codeType)
        {
            CodeType = codeType;
        }

        public Type CodeType
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 描述给定响应码时的信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ResponseCodeItemAttribute : Attribute
    {
        public ResponseCodeItemAttribute(Type dataType)
        {
            DataType = dataType;
        }

        public ResponseCodeItemAttribute(string message, Type dataType = null)
        {
            Message = message;
            DataType = dataType;
        }

        public string Message { get; private set; }

        public Type DataType { get; private set; }
    }
}
