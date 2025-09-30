using System.Runtime.CompilerServices;

namespace Brimborium.CodeAsCode;

public sealed class CascElement 
    : ICascDefinitionSourceCode
    , ICascVersion {
    private string _Name;
    private string _SourceCodeFilePath;
    private int _SourceCodeLineNumber;

    public CascElement(
        [CallerMemberName] string name = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0) {
        this._Name = name;
        this._SourceCodeFilePath = callerFilePath;
        this._SourceCodeLineNumber = callerLineNumber;
        this.CascVersion = CascVersionUtility.GetNextVersion();
        this.ListDefinition = new (this);
    }

    public string Name {
        get => this._Name;
        set {
            this._Name = value;
            this.CascVersion = CascVersionUtility.GetNextVersion();
        }
    }
    public string SourceCodeFilePath {
        get => this._SourceCodeFilePath;
        set {
            this._SourceCodeFilePath = value;
            this.CascVersion = CascVersionUtility.GetNextVersion();
        }
    }
    public int SourceCodeLineNumber {
        get => this._SourceCodeLineNumber;
        set {
            this._SourceCodeLineNumber = value;
            this.CascVersion = CascVersionUtility.GetNextVersion();
        }
    }

    public CascListOwned<ICascDefinition> ListDefinition { get; }

    public long CascVersion { get; set; }


    public CascElement AddDefinition<T>(
        [CallerMemberName] string name = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
        ) where T : ICascDefinition, new() {
        var cascDefinition = new T();
        if (cascDefinition is ICascDefinitionSourceCode cascDefinitionSourceCode) {
            cascDefinitionSourceCode.Name = name;
            cascDefinitionSourceCode.SourceCodeFilePath = callerFilePath;
            cascDefinitionSourceCode.SourceCodeLineNumber = callerLineNumber;
        }
        this.ListDefinition.Add(cascDefinition);
        return this;
    }

    public T AddDefinition<T>(
        T cascDefinition
        ) where T : ICascDefinition {
        this.ListDefinition.Add(cascDefinition);
        return cascDefinition;
    }

    public T? GetDefinition<T>() where T : notnull {
        foreach (var defintion in this.ListDefinition) {
            if (defintion is T result) {
                return result;
            }
        }
        return default;
    }

    public List<T> GetAllDefinition<T>() where T : notnull {
        List<T> result = [];
        foreach (var defintion in this.ListDefinition) {
            if (defintion is T item) {
                result.Add(item);
            }
        }
        return result;
    }
}
