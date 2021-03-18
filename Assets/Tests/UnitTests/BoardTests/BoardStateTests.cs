using Zenject;
using NUnit.Framework;

[TestFixture]
public class BoardStateTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void Init()
    {
        BoardStateInstaller.Install(Container);
    }

    [Test]
    public void TestBinding()
    {
        IBoardState boardState = Container.Resolve<IBoardState>();

        Assert.NotNull(boardState);
    }

    [Test]
    public void TestResolveToClass()
    {
        BoardState boardState = Container.Resolve<IBoardState>() as BoardState;

        Assert.IsNotNull(boardState);
    }


    A
    public void MyTestMethod()
    {

    }
}