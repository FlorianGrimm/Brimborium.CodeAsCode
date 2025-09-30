using Brimborium.CodeAsCode.Tests.Sample1;

namespace Brimborium.CodeAsCode.Tests;

public class CascConfigurationTests {
    [Test]
    public async Task ConfigureTest() {
        var sut = new CascConfiguration();
        var cappRoot = new Sample1.CappRoot();
        sut.Configure(cappRoot, static (cappRoot) => cappRoot.Configure);
        await Assert.That(sut.IsConfigured(cappRoot)).IsTrue();
        await Assert.That(sut.IsConfigured(cappRoot.Pages)).IsFalse();
    }

    [Test]
    public async Task RootConfigureTest() {
        Sample1.CappRoot cappRoot = new();
        var sut = new CascConfiguration();
        sut.RootConfigure<CappRoot>(cappRoot);

        await Assert.That(cappRoot.Pages.PageB.GetDefinition<CappPageB>()).IsNotNull();
        await Assert.That(cappRoot.Pages.PageB.GetDefinition<CappPageB>()).IsSameReferenceAs(cappRoot.PageB);
    }

    [Test]
    public async Task CascConfiguredTest() {
        var sut = new CascConfigured<Sample1.CappRoot>();
        var cappRoot = sut.GetValue();

        await Assert.That(cappRoot).IsNotNull();
        await Assert.That(cappRoot.Pages.PageB.GetDefinition<CappPageB>()).IsNotNull();
        await Assert.That(cappRoot.Pages.PageB.GetDefinition<CappPageB>()).IsSameReferenceAs(cappRoot.PageB);
    }
}