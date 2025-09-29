
namespace Brimborium.CodeAsCode.Tests.Sample1;

public partial class CappRoot : CascRoot {
    public CappPages Pages { get; } = new CappPages();

    public CappPageB CappPageB { get; } = new CappPageB();

    public void Configure(CascConfiguration configuration) {
        configuration.Configure(this.Pages, this, static (p) => p.Configure);
    }
}

public class CappPages : CascCollection {
    public CascElement PageA { get; } = CascElement();
    public CascElement PageB { get; } = CascElement();

    public void Configure(CascConfiguration configuration, CappRoot cappRoot) {
        this.PageA.HasDefinition<CappUIPage>();
        this.PageB.HasDefinition(cappRoot.CappPageB);
    }
}

public interface ICappUIPage : ICascDefinition {
}

public class CappUIPage : CascDefinition, ICappUIPage {
}

public class CappPageB : CappUIPage {
    public CascElement Child1 { get; } = CascElement();
    public CascElement Child2 { get; } = CascElement();
}

