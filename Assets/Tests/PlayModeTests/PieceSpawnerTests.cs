using Zenject;
using System.Collections;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;

public class PieceSpawnerTests : ZenjectIntegrationTestFixture
{

    private IPieceSpawner _pieceSpawner;
    private IBoardState _boardState;

    [Inject]
    public void Construct(IPieceSpawner pieceSpawner, IBoardState boardState)
    {
        _pieceSpawner = pieceSpawner;
        _boardState = boardState;
    }

    void CommonInstall()
    {
        PreInstall();

        PieceSpawnerInstaller.Install(Container);
        BoardStateInstaller.Install(Container);

        PostInstall();
    }

    [UnityTest]
    public IEnumerator PiecesSpawnInCorrectPosition(
        [Values] PieceType pieceType,
        [Values(2, 4, 6)] int x,
        [Values(1, 3, 5)] int y
        )
    {
        CommonInstall();
        //var piece = _pieceSpawner.CreatePieceOf(pieceType, new BoardPosition(x, y));
        yield return null;
        //Assert.AreEqual(new BoardPosition(x, y), piece.BoardPosition);
    }

    [UnityTest]
    public IEnumerator PiecesSpawnWithCorrectType(
        [Values] PieceType pieceType
        )
    {
        CommonInstall();
        //var piece = _pieceSpawner.CreatePieceOf(pieceType, new BoardPosition(0, 0));
        yield return null;
        //Assert.AreEqual(pieceType, piece.Info.PieceType);
    }

    [UnityTest]
    public IEnumerator PiecesChangeBoardStateOnSpawn(
        [Values] PieceType pieceType, [Values(1, 3, 5, 7)] int x, [Values(0, 2, 4, 6)] int y
        )
    {
        CommonInstall();
        //var piece = _pieceSpawner.CreatePieceOf(pieceType, new BoardPosition(x, y));
        yield return null;
        //Assert.AreEqual(piece, _boardState.GetTileAt(new BoardPosition(x, y)).CurrentPiece.GetComponent<Piece>());
    }

    [UnityTest]
    public IEnumerator PiecesSpawnWithPossibleMoveGenerator([Values] PieceType pieceType)
    {
        CommonInstall();
        //var piece = _pieceSpawner.CreatePieceOf(pieceType, new BoardPosition(0, 0));
        yield return null;
        //Assert.IsNotNull(piece.GetPossibleMoves());
    }

    [UnityTest]
    public IEnumerator PiecesSpawnAtCorrectVector(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        CommonInstall();
        //var piece = _pieceSpawner.CreatePieceOf(PieceType.WhitePawn, new BoardPosition(x, y));
        yield return null;
        //Assert.AreEqual(new Vector2(x + 0.5f, y + 0.5f), piece.BoardPosition.Vector);
    }
}