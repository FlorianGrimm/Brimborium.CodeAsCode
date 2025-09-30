namespace Brimborium.CodeAsCode;

public static class CascUtility {
    /*
    public static List<T> GetListProperty<T>(object value, List<T>? target = null)
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
    }*/

    public static IEnumerable<T> EnumPropertyOf<T>(object value)
        where T : notnull {
        var valueType = value.GetType();
        var listtProperties = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var pi in listtProperties) {
            if (typeof(T).IsAssignableFrom(pi.PropertyType)) {
                if (pi.GetValue(value) is T propertyValue) {
                    yield return propertyValue;
                }
            }
        }
    }

    public static IEnumerable<object> EnumPropertyOf<T1,T2>(object value)
        where T1 : notnull
        where T2 : notnull 
        {
        var valueType = value.GetType();
        var listtProperties = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var pi in listtProperties) {
            if (typeof(T1).IsAssignableFrom(pi.PropertyType)) {
                if (pi.GetValue(value) is T1 propertyValue) {
                    yield return propertyValue;
                }
                continue;
            }
            if (typeof(T2).IsAssignableFrom(pi.PropertyType)) {
                if (pi.GetValue(value) is T2 propertyValue) {
                    yield return propertyValue;
                }
                continue;
            }
        }
    }

}
