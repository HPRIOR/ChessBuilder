using NUnit.Framework;
using Zenject;

[TestFixture]
public class GameStateTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void Init()
    {
        InstallBindings();
        ResolveContainer();
    }

    [TearDown]
    public void TearDown()
    {
        Container.UnbindAll();
    }

    private void InstallBindings()
    {
    }

    private void ResolveContainer()
    {
    }
}