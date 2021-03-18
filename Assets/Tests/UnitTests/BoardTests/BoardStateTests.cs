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

    private IBoardState GetBoardState() => Container.Resolve<IBoardState>();

    [Test]
    public void BindsCorrectly()
    {
        IBoardState boardState = Container.Resolve<IBoardState>();

        Assert.NotNull(boardState);
    }

    [Test]
    public void ResolvesToClass()
    {
        BoardState boardState = Container.Resolve<IBoardState>() as BoardState;

        Assert.IsNotNull(boardState);
    }

    [Test]
    public void BoardIsGenerated()
    {
        IBoardState boardState = Container.Resolve<IBoardState>();

        Assert.IsNotNull(boardState.Board);
    }

    [Test]
    public void BoardIsCorrectSize()
    {
        var boardState = GetBoardState();

        Assert.AreEqual(64, boardState.Board.Length);

    }

    [Test]
    public void BoardContainsCorrectBoardPositions(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        var boardState = GetBoardState();
        Assert.AreEqual(new BoardPosition(x, y), boardState.Board[x, y].BoardPosition);
    }

    [Test]
    public void GetterRetrievesCorrectTile(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        var boardState = GetBoardState();
        Assert.AreEqual(boardState.GetTileAt(new BoardPosition(x, y)).BoardPosition, new BoardPosition(x, y));
    }

    [Test]
    public void BoardContainsNoPiecesOnInit(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        var boardState = GetBoardState();
        Assert.IsNull(boardState.GetTileAt(new BoardPosition(x, y)).CurrentPiece);
    }
}