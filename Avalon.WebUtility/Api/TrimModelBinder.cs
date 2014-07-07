using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string) && value != null && propertyDescriptor.Attributes.OfType<NoTrimAttribute>().Count() == 0)
                value = ((string)value).Trim();
            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
