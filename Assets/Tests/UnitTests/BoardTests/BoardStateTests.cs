using Zenject;
using NUnit.Framework;
using UnityEngine;

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

        Assert.IsNotNull(boardState.GetTileAt(new BoardPosition(0,0)));
    }

    [Test]
    public void BoardIsCorrectSize()
    {
        var boardState = GetBoardState();

        Assert.DoesNotThrow(() => boardState.GetTileAt(new BoardPosition(7,7)));

    }

    [Test]
    public void BoardContainsCorrectBoardPositions(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        var boardState = GetBoardState();
        Assert.AreEqual(new BoardPosition(x, y), boardState.GetTileAt(new BoardPosition(x, y)).BoardPosition);
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

    [Test]
    public void TilesAreAtCorrentPositions(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        var boardState = GetBoardState();
        Assert.AreEqual(boardState.GetTileAt(new BoardPosition(x, y)).BoardPosition.Position, new Vector2(x + 0.5f, y + 0.5f));
    }


    [Test]
    public void BoardRotatesCorrectly()
    {
        var boardState = GetBoardState();

        Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(0,0)), boardState.GetTileAt(new BoardPosition(7, 7)));
        Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(1,1)), boardState.GetTileAt(new BoardPosition(6, 6)));
        Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(4,6)), boardState.GetTileAt(new BoardPosition(3, 1)));
        Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(3,2)), boardState.GetTileAt(new BoardPosition(4, 5)));
    }
}