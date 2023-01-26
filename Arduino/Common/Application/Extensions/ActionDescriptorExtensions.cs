using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Arduino.Common.Application.Extensions;

public static class ActionDescriptorExtensions
{
    public static IEnumerable<T> GetCustomAttributes<T>(this ActionDescriptor actionDescriptor)
        where T : Attribute =>
        actionDescriptor is ControllerActionDescriptor descriptor
            ? descriptor.MethodInfo.GetCustomAttributes<T>()
            : Enumerable.Empty<T>();
}
