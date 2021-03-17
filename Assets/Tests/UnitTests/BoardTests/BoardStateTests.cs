using Zenject;
using NUnit.Framework;

[TestFixture]
public class BoardStateTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void Init()
    {
        BoardGeneratorInstaller.Install(Container);
    }
    [Test]
    public void TestBinding()
    {
        IBoardGenerator boardGenerator = Container.Resolve<IBoardGenerator>();

        Assert.NotNull(boardGenerator);
    }
}