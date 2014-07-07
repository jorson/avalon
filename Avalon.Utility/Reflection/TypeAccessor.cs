using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Compilation;

namespace Avalon.Utility
{
    public class TypeAccessor
    {
        static Dictionary<Type, TypeAccessor> accessors = new Dictionary<Type, TypeAccessor>();

        Type type;
        PropertyInfo[] properties;
        FieldInfo[] fields;

        Dictionary<string, MemberAccess<PropertyInfo>> propertyAccessDic;
        Dictionary<string, MemberAccess<FieldInfo>> fieldAccessDic;

        IList<PropertyInfo> readWriteProperties;
        Action<object, object, Func<object, object>> cloneFieldsHandler;

        private TypeAccessor(Type type)
        {
            this.type = type;
            properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(o => o.GetIndexParameters().Length == 0).ToArray();
            readWriteProperties = properties.Where(o => o.CanRead && o.CanWrite).ToList();
            fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToArray();

            propertyAccessDic = properties.ToDictionary(o => o.Name, o => CreateMemberAccess(o));
            fieldAccessDic = fields.ToDictionary(o => o.Name, o => CreateMemberAccess(o));

            cloneFieldsHandler = (source, target, valueProcessHandler) =>
            {
                foreach (var entry in fieldAccessDic)
                {
                    var v = valueProcessHandler(entry.Value.Getter(source));
                    entry.Value.Setter(target, v);
                }
            };
        }

        public void CloneByFields(object source, object target, Func<object, object> valueProcessHandler)
        {
            cloneFieldsHandler(source, target, valueProcessHandler);
        }

        public static TypeAccessor GetAccessor(Type type)
        {
            TypeAccessor accessor;
            if (!accessors.TryGetValue(type, out accessor))
            {
                lock (accessors)
                {
                    if (!accessors.TryGetValue(type, out accessor))
                    {
                        accessor = new TypeAccessor(type);
                        accessors.Add(type, accessor);
                    }
                }
            }
            return accessor;
        }

        public Type Type
        {
            get { return type; }
        }

        public PropertyInfo[] Properties
        {
            get { return properties; }
        }

        public FieldInfo[] Fields
        {
            get { return fields; }
        }

        public Dictionary<string, MemberAccess<PropertyInfo>> PropertyAccessDic
        {
            get { return propertyAccessDic; }
        }

        public Dictionary<string, MemberAccess<FieldInfo>> FieldAccessDic
        {
            get { return fieldAccessDic; }
        }

        public object Create()
        {
            return FastActivator.Create(type);
        }

        public Action<object, object> GetPropertyClone(string propertyName)
        {
            return propertyAccessDic.TryGetValue(propertyName).GetOrDefault(o => o.Cloner);
        }

        public Action<object, object> GetFieldClone(string fieldName)
        {
            return fieldAccessDic.TryGetValue(fieldName).GetOrDefault(o => o.Cloner);
        }

        public Func<object, object> GetPropertyGetter(string propertyName)
        {
            return propertyAccessDic.TryGetValue(propertyName).GetOrDefault(o => o.Getter);
        }

        public Action<object, object> GetPropertySetter(string propertyName)
        {
            return propertyAccessDic.TryGetValue(propertyName).GetOrDefault(o => o.Setter);
        }

        public Func<object, object> GetFieldGetter(string fieldName)
        {
            return fieldAccessDic.TryGetValue(fieldName).GetOrDefault(o => o.Getter);
        }

        public Action<object, object> GetFieldSetter(string fieldName)
        {
            return fieldAccessDic.TryGetValue(fieldName).GetOrDefault(o => o.Setter);
        }

        public object GetProperty(string propertyName, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Func<object, object> getter = GetPropertyGetter(propertyName);
            if (getter == null)
                throw new ArgumentOutOfRangeException("propertyName", String.Format("对象 {0} 没有命名为 {1} 的属性", instance.GetType().FullName, propertyName));

            return getter(instance);
        }

        public void SetProperty(string propertyName, object instance, object value)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Action<object, object> setter = GetPropertySetter(propertyName);
            if (setter == null)
                throw new ArgumentOutOfRangeException("propertyName", String.Format("对象 {0} 没有命名为 {1} 的属性", instance.GetType().FullName, propertyName));

            setter(instance, value);
        }

        public object GetField(string fieldName, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Func<object, object> getter = GetFieldGetter(fieldName);
            if (getter == null)
                throw new ArgumentOutOfRangeException("fieldName", String.Format("对象 {0} 没有命名为 {1} 的字段", instance.GetType().FullName, fieldName));

            return getter(instance);
        }

        public void SetField(string fieldName, object instance, object value)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Action<object, object> setter = GetFieldSetter(fieldName);
            if (setter == null)
                throw new ArgumentOutOfRangeException("fieldName", String.Format("对象 {0} 没有命名为 {1} 的字段", instance.GetType().FullName, fieldName));

            setter(instance, value);
        }

        public IList<PropertyInfo> ReadWriteProperties
        {
            get { return readWriteProperties; }
        }

        public object[] GetReadWritePropertyValues(object entity)
        {
            return propertyAccessDic.Values.Where(o => o.CanClone).Select(o => o.Getter(entity)).ToArray();
        }

        public void SetReadWritePropertyValues(object entity, object[] values)
        {
            if (values.Length != readWriteProperties.Count)
                throw new ArgumentException(String.Format("给定的值个数 {0} 与属性的个数 {1} 不一致，对象类型为 {2}。", values.Length, readWriteProperties.Count, type.FullName));

            var setters = propertyAccessDic.Values.Where(o => o.CanClone).ToArray();
            for (var i = 0; i < values.Length; i++)
                setters[i].Setter(entity, values[i]);
        }

        static MemberAccess<PropertyInfo> CreateMemberAccess(PropertyInfo member)
        {
            var memberAccess = new MemberAccess<PropertyInfo>()
            {
                Member = member,
                CanRead = member.CanRead,
                CanWrite = member.CanWrite
            };
            if (memberAccess.CanRead)
                memberAccess.Getter = DelegateAccessor.CreatePropertyGetter(member);
            if (memberAccess.CanWrite)
                memberAccess.Setter = DelegateAccessor.CreatePropertySetter(member);
            if (memberAccess.CanClone)
                memberAccess.Cloner = DelegateAccessor.CreatePropertyCloner(member);

            return memberAccess;
        }

        static MemberAccess<FieldInfo> CreateMemberAccess(FieldInfo member)
        {
            var memberAccess = new MemberAccess<FieldInfo>()
            {
                Member = member,
                CanRead = true,
                CanWrite = true
            };
            if (memberAccess.CanRead)
                memberAccess.Getter = DelegateAccessor.CreateFieldGetter(member);
            if (memberAccess.CanWrite)
                memberAccess.Setter = DelegateAccessor.CreateFieldSetter(member);
            if (memberAccess.CanClone)
                memberAccess.Cloner = DelegateAccessor.CreateFieldCloner(member);

            return memberAccess;
        }

        public class MemberAccess<T> where T : MemberInfo
        {
            public T Member { get; internal set; }

            public bool CanRead { get; internal set; }
            public bool CanWrite { get; internal set; }

            public bool CanClone
            {
                get { return CanRead && CanWrite; }
            }

            public Func<object, object> Getter { get; internal set; }

            public Action<object, object> Setter { get; internal set; }

            public Action<object, object> Cloner { get; internal set; }
        }
    }
}
