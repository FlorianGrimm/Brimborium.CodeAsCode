using System.Reflection;

namespace Brimborium.CodeAsCode;

public static class CascUtility {
    public static List<T> GetListProperty<T>(this T value, List<T>? target = null)
        where T : notnull {
        var result = target ?? new List<T> { };
        var valueType = value.GetType();
        var listtProperties = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var pi in listtProperties) {
            if (pi.PropertyType == typeof(T)) {
                if (pi.GetValue(value) is T propertyValue)
                    result.Add(propertyValue);
            }
        }
        return result;
    }
}
