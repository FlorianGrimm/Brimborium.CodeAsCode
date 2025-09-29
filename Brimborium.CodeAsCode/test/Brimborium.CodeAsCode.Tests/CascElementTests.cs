using System.Runtime.CompilerServices;

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
        var callerFilePath = GetSourceCodeFilePath();
        Sample1.CappPages sut = new();
        await Assert.That(sut.PageA.SourceCodeFilePath).IsEqualTo(callerFilePath);
        await Assert.That(sut.PageB.SourceCodeFilePath).IsEqualTo(callerFilePath);
    }

    private static string GetSourceCodeFilePath([CallerFilePath] string callerFilePath = "") => callerFilePath;

    [Test]
    public async Task SourceCodeLineNumberTest() {
        var sourceCodeLineNumber = GetSourceCodeLineNumber();
        Sample1.CappPages sut = new();
        await Assert.That(sut.PageA.SourceCodeLineNumber).IsLessThan(sut.PageB.SourceCodeLineNumber);
        await Assert.That(sut.PageB.SourceCodeLineNumber).IsLessThan(sourceCodeLineNumber);
    }

    private static int GetSourceCodeLineNumber([CallerLineNumber] int callerLineNumber = 0) => callerLineNumber;

}
