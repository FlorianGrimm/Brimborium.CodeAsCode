using Brimborium.CodeAsCode.Tests.Sample1;

namespace Brimborium.CodeAsCode.Tests;

public class CascConfigurationTests {
    [Test]
    public async Task ConfigureTest() {
        var sut = new CascConfiguration();
        var cappRoot = new Sample1.CappRoot();
        sut.Configure(cappRoot, static (cappRoot) => cappRoot.Configure);
        await Assert.That(sut.IsConfigured(cappRoot.Pages)).IsTrue();
        //await Assert.That(cappRoot.Pages.PageA.ListDefinition).Contains((item)=>item is CappUIPage);

    }
}