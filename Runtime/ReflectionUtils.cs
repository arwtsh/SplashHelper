using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SplashHelper
{
    internal static class ReflectionUtils
    {
        internal static List<MethodInfo> FindMethodsWithAttribute<T>() where T : System.Attribute
        {
            return (
               from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
               from type in assembly.GetTypes()
               from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
               where method.GetCustomAttributes<T>().Count() > 0
               select method
               ).ToList();
        }
    }
}
