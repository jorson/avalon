using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Avalon.Web.Test
{
    public static class AutofacServiceLocatorExtends
    {
        public static void RegisterMvc(this AutofacServiceLocator locator)
        {
            locator.Builder.RegisterControllers(Assembly.GetExecutingAssembly());
            locator.Builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            locator.Builder.RegisterModelBinderProvider();
        }

        public static void BuilderMvc(this AutofacServiceLocator locator)
        {
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(locator.Container));
        }
    }
}