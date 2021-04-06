using Zenject;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Models.Piece;

public class PossibleMovesTestBase : ZenjectUnitTestFixture
{
    private IBoardGenerator _boardGenerator;
    private IPieceSpawner _pieceSpawner;
    private IPieceMoveGeneratorFactory _pieceMoveGeneratorFactory;
    protected PieceColour TestedPieceColour { get; set; } = PieceColour.White;

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
        _pieceMoveGeneratorFactory = null;
        _pieceSpawner = null;
    }

    public void ResolveContainer()
    {
        _pieceSpawner = Container.Resolve<IPieceSpawner>();
        _pieceMoveGeneratorFactory = Container.Resolve<IPieceMoveGeneratorFactory>();
        _boardGenerator = Container.Resolve<IBoardGenerator>();
    }

    private void InstallBindings()
    {
        BoardStateInstaller.Install(Container);
        PieceSpawnerInstaller.Install(Container);
        PieceMoveGeneratorFactoryInstaller.Install(Container);
        CommandInvokerInstaller.Install(Container);
        MoveCommandInstaller.Install(Container);
        BoardEvalInstaller.Install(Container);
        BoardScannerInstaller.Install(Container);
        BoardPositionTranslatorInstaller.Install(Container);
    }

    protected IBoardState SetUpBoardWith(IEnumerable<(PieceType piece, IBoardPosition boardPosition)> piecesAtPositions)
    {
        var boardState = new BoardState(_boardGenerator.GenerateBoard());
        var board = boardState.Board;
        piecesAtPositions.ToList().ForEach(tup => board[tup.boardPosition.X, tup.boardPosition.Y].CurrentPiece = new Piece(tup.piece));
        return boardState;
    }


    protected PieceType GetPieceTypeAtPosition(int x, int y, IBoardState inBoardState) => TestedPieceColour == PieceColour.White 
        ? inBoardState.GetTileAt(new BoardPosition(x, y)).CurrentPiece.Type 
        : inBoardState.GetMirroredTileAt(new BoardPosition(x, y)).CurrentPiece.Type;

    protected IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType) => 
        _pieceMoveGeneratorFactory.GetPossibleMoveGenerator(pieceType);

    protected PieceType GetOppositePieceType(PieceType pieceType)
    {
        var pieceTypeString = pieceType.ToString();
        if (pieceTypeString.StartsWith("White"))
        {
            return (PieceType)Enum.Parse(typeof(PieceType), "Black" + pieceTypeString.Substring(5));
        }
        return (PieceType)Enum.Parse(typeof(PieceType), "White" + pieceTypeString.Substring(5));
    }

    /// <summary>
    /// Gets the position relative to the current tested piece (white or black)
    /// </summary>
    /// <param name="boardPosition"></param>
    /// <returns></returns>
    protected IBoardPosition RelativePositionToTestedPiece(IBoardPosition boardPosition) =>
        TestedPieceColour == PieceColour.White ? boardPosition : GetMirroredBoardPosition(boardPosition);

   
    private IBoardPosition GetMirroredBoardPosition(IBoardPosition boardPosition) =>
        new BoardPosition(Math.Abs(boardPosition.X - 7), Math.Abs(boardPosition.Y - 7));


    protected void SetTestedPieceColourWith(PieceType currentPieceType) =>
        _ = TestedPieceColour = GetPieceColourFrom(currentPieceType);

    private PieceColour GetPieceColourFrom(PieceType pieceType) => 
        pieceType.ToString().StartsWith("White") ? PieceColour.White : PieceColour.Black;

    protected IEnumerable<IBoardPosition> GetPositionsIncludingAndPassed(IBoardPosition boardPosition, Direction direction)
    {
        if (boardPosition.X > 7 || boardPosition.X < 0 || boardPosition.Y > 7 || boardPosition.Y < 0) return new List<IBoardPosition>();
        var nextBoardPosition =
            new BoardPosition(boardPosition.X + Move.In(direction).X, boardPosition.Y + Move.In(direction).Y);
        return GetPositionsIncludingAndPassed(nextBoardPosition, direction)
            .Concat(new List<IBoardPosition>() { boardPosition });
    }

}