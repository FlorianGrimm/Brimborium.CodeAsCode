using Brimborium.CodeAsCode.Tests.Sample1;

namespace Brimborium.CodeAsCode.Tests;

public class CascUtilityTests {
    [Test]
    public async Task GetListPropertyTest() {
        var cappRoot = new Sample1.CappRoot();
        var list = CascUtility.EnumPropertyOf<ICappUIPage>(cappRoot.Pages).ToList();
        await Assert.That(list.Count).IsEqualTo(2);
        var list2 = list.Where(p => p is Sample1.ICappUIPage).ToList();
        //await Assert.That(list[0]).Is.SameAs(sut.PageA);
        //await Assert.That(list[1]).Is.SameAs(sut.PageB);
    }
}
