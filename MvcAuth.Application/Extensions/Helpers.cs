using System.Reflection;

namespace MvcAuth.Application.Extensions;
public static class Helpers
{

    public static List<Type> GetTypesOf(this Assembly assembly, Type baseType) => assembly
      .GetTypes()
      .Where(type =>
          type.BaseType != null
          && (
              (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == baseType))
              || (baseType.IsAssignableFrom(type) && !type.IsAbstract)
          )
      .ToList();

}
