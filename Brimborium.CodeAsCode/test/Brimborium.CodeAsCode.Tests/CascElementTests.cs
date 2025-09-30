namespace Brimborium.CodeAsCode.Tests;

public class CascElementTests {
    [Test]
    public async Task NameTest() {
        Sample1.CappPages sut = new();
        await Assert.That(sut.PageA.Name).IsEqualTo("PageA");
        await Assert.That(sut.PageB.Name).IsEqualTo("PageB");
    }

    [Test]
    public async Task SourceCodeFilePathTest() {
        var callerFilePath = Sample1.CappPages.GetSourceCodeFilePath();
        Sample1.CappPages sut = new();
        await Assert.That(sut.PageA.SourceCodeFilePath).IsEqualTo(callerFilePath);
        await Assert.That(sut.PageB.SourceCodeFilePath).IsEqualTo(callerFilePath);
    }

    [Test]
    public async Task SourceCodeLineNumberTest() {
        Sample1.CappPages sut = new();
        await Assert.That(sut.PageA.SourceCodeLineNumber).IsLessThan(sut.PageB.SourceCodeLineNumber);
    }
}
