using Zenject;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMoveTestBase : ZenjectUnitTestFixture
{
    private IPieceSpawner _pieceSpawner;
    private IPieceMoveGeneratorFactory _pieceMoveGeneratorFactory;
    private IBoardState _boardState;

    [SetUp]
    public void Init()
    {
        InstallBindings();
        ResolveContainer();
    }

    [TearDown]
    public void TearDown()
    {
        _boardState = null;
        _pieceMoveGeneratorFactory = null;
        _pieceSpawner = null;
    }

    public void ResolveContainer()
    {
        _pieceSpawner = Container.Resolve<IPieceSpawner>();
        _pieceMoveGeneratorFactory = Container.Resolve<IPieceMoveGeneratorFactory>();
        _boardState = Container.Resolve<IBoardState>();
    }

    private void InstallBindings()
    {
        BoardStateInstaller.Install(Container);
        PieceSpawnerInstaller.Install(Container);
        PieceMoveGeneratorFactoryInstaller.Install(Container);
        CommandInvokerInstaller.Install(Container);
        DragAndDropCommandInstaller.Install(Container);
    }

    protected void SetUpBoardWith(IEnumerable<(PieceType piece, IBoardPosition boardPosition)> piecesAtPositions) =>
        piecesAtPositions.ToList().ForEach(item => _pieceSpawner.CreatePieceOf(item.piece, item.boardPosition));
    protected GameObject GetGameObjectAtPosition(int x, int y) => _boardState.GetTileAt(new BoardPosition(x, y)).CurrentPiece;
    protected IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType) => _pieceMoveGeneratorFactory.GetPossibleMoveGenerator(pieceType);

}