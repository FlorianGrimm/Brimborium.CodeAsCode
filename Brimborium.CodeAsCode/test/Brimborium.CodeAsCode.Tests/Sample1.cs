using System.Runtime.CompilerServices;

namespace Brimborium.CodeAsCode.Tests.Sample1;

public partial class CappRoot : CascRoot, ICascConfigurable {
    public CappRoot() {
        this.Pages = new();
        this.PageB = new();
    }
    public CappPages Pages { get; }

    public CappPageB PageB { get; }

    public void Configure(CascConfiguration configuration) {
        //configuration.ConfigureProperties(this);
    }
}

public class CappPages : CascCollection, ICascConfigurable<CappRoot> {
    public CascElement PageA { get; } = CascElement();
    public CascElement PageB { get; } = CascElement();

    public void Configure(CascConfiguration configuration, CappRoot cappRoot) {
        this.PageA.AddDefinition<CappUIPage>();
        this.PageB.AddDefinition(cappRoot.PageB);
    }

    public static string GetSourceCodeFilePath() => _GetSourceCodeFilePath();
    private static string _GetSourceCodeFilePath([CallerFilePath] string callerFilePath = "") => callerFilePath;
    public static int GetSourceCodeLineNumber() => _GetSourceCodeLineNumber();
    private static int _GetSourceCodeLineNumber([CallerLineNumber] int callerLineNumber = 0) => callerLineNumber;
}

public interface ICappUIPage : ICascDefinition {
}

public class CappUIPage : CascDefinition, ICappUIPage {
}

public class CappPageB : CappUIPage, ICascConfigurable<CappRoot> {
    public CascElement Child1 { get; } = CascElement();
    public CascElement Child2 { get; } = CascElement();

    public void Configure(CascConfiguration cascConfiguration, CappRoot arg) {
        this.Child2.AddDefinition<CappUIPage>();
    }

    public static int GetSourceCodeLineNumber([CallerLineNumber] int callerLineNumber = 0) => callerLineNumber;
}