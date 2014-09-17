using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Configuration;
using System.ComponentModel;
using System.Reflection;

namespace Avalon.Utility
{
    public static class FastActivator
    {
        static Dictionary<Type, Func<object>> factoryCache = new Dictionary<Type, Func<object>>();
        static Dictionary<Type, Func<int, Array>> arrayCache = new Dictionary<Type, Func<int, Array>>();

        public static T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        public static object Create(string type)
        {
            return Create(Type.GetType(type));
        }

        public static object Create(Type type)
        {
            if (type == null)
                throw new ArgumentException("type");

            Func<object> f;
            if (!factoryCache.TryGetValue(type, out f))
            {
                lock (factoryCache)
                {
                    if (!factoryCache.TryGetValue(type, out f))
                    {
                        f = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(type), typeof(object)), new ParameterExpression[0]).Compile();
                        factoryCache[type] = f;
                    }
                }
            }
            return f.Invoke();
        }

        public static Array CreateArray(Type type, int length)
        {
            if (type == null)
                throw new ArgumentException("type");

            Func<int, Array> f;
            if (!arrayCache.TryGetValue(type, out f))
            {
                lock (factoryCache)
                {
                    if (!arrayCache.TryGetValue(type, out f))
                    {
                        var lengthParam = Expression.Parameter(typeof(int), "length");
                        f = Expression.Lambda<Func<int, Array>>(Expression.Convert(Expression.NewArrayBounds(type, lengthParam), typeof(Array)), lengthParam).Compile();
                        arrayCache[type] = f;
                    }
                }
            }
            return f.Invoke(length);
        }
    }
}
