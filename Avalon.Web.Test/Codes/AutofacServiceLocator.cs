using Autofac;
using Avalon.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avalon.Web.Test
{
    public class AutofacServiceLocator : IDependencyRegister, IDependencyResolver
    {
        private ContainerBuilder builder = new ContainerBuilder();
        private IContainer container;
        public IContainer Container
        {
            get { return container; }
        }

        public ContainerBuilder Builder
        {
            get { return builder; }
        }

        public void RegisterRepository()
        {
            //初始化配置
            RepositoryFramework.Configure(this);
        }

        public void Register(Type interfaceType, Type instanceType)
        {
            builder.RegisterType(instanceType).As(interfaceType).SingleInstance();
        }

        public void Register(Type interfaceType, object instance)
        {
            builder.RegisterInstance(instance).As(interfaceType);
        }

        public void RegisterType(Type implementationType)
        {
            builder.RegisterType(implementationType);
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        public T ResolveOptional<T>()
        {
            return (T)container.ResolveOptional(typeof(T));
        }

        public object ResolveOptional(Type type)
        {
            return container.ResolveOptional(type);
        }

        public void Build()
        {
            container = builder.Build();
            DependencyResolver.SetResolver(this);
        }
    }
}