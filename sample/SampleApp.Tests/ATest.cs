namespace SampleApp.Tests;

public class ATest
{
    [Test]
    public async Task A()
    {
        int i = 1;
        await Assert.That(i).IsEqualTo(1);
    }
}
