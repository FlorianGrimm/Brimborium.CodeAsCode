namespace Brimborium.CodeAsCode;

public sealed class CascConfigured<T>
    where T : class, ICascConfigurable, new() {
    private T? _Value;

    public CascConfigured() {
    }

    public T GetValue() {
        if (this._Value is { } result) { return result; }
        {
            this._Value = result = new T();
            CascConfiguration cascConfiguration = new();
            cascConfiguration.RootConfigure<T>(result);
            return result;
        }
    }
}
