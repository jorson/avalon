using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Security.Cryptography
{
    /// <summary>
    /// Indicates that the value of marked element could be <c>null</c> sometimes, so the check for <c>null</c> is necessary before its usage.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    internal sealed class CanBeNullAttribute : Attribute
    {
    }
}
