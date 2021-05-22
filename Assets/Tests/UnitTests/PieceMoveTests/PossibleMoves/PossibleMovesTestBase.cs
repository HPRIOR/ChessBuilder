using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PieceMoveTests.PossibleMoves
{
    public class PossibleMovesTestBase : ZenjectUnitTestFixture
    {
        private IBoardGenerator _boardGenerator;
        private IPieceSpawner _pieceSpawner;
        private IPossibleMoveFactory _possibleMoveFactory;
        private PieceColour TestedPieceColour { get; set; } = PieceColour.White;

        [SetUp]
        public void Init()
        {
            PossibleMovesBinder.InstallBindings(Container);
            ResolveContainer();
        }

        [TearDown]
        public void TearDown()
        {
            Container.UnbindAll();
            _possibleMoveFactory = null;
            _pieceSpawner = null;
        }

        public void ResolveContainer()
        {
            _pieceSpawner = Container.Resolve<IPieceSpawner>();
            _possibleMoveFactory = Container.Resolve<IPossibleMoveFactory>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        protected IBoardState SetUpBoardWith(
            IEnumerable<(PieceType piece, IBoardPosition boardPosition)> piecesAtPositions)
        {
            var boardState = new BoardState(_boardGenerator.GenerateBoard());
            var board = boardState.Board;
            piecesAtPositions.ToList().ForEach(tup =>
                board[tup.boardPosition.X, tup.boardPosition.Y].CurrentPiece = new Piece(tup.piece));
            return boardState;
        }


        protected PieceType GetPieceTypeAtPosition(int x, int y, IBoardState inBoardState)
        {
            return TestedPieceColour == PieceColour.White
                ? inBoardState.GetTileAt(new BoardPosition(x, y)).CurrentPiece.Type
                : inBoardState.GetMirroredTileAt(new BoardPosition(x, y)).CurrentPiece.Type;
        }

        protected IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType)
        {
            return _possibleMoveFactory.GetPossibleMoveGenerator(pieceType);
        }

        protected PieceType GetOppositePieceType(PieceType pieceType)
        {
            var pieceTypeString = pieceType.ToString();
            if (pieceTypeString.StartsWith("White"))
                return (PieceType) Enum.Parse(typeof(PieceType), "Black" + pieceTypeString.Substring(5));
            return (PieceType) Enum.Parse(typeof(PieceType), "White" + pieceTypeString.Substring(5));
        }

        /// <summary>
        ///     Gets the position relative to the current tested piece (white or black)
        /// </summary>
        /// <param name="boardPosition"></param>
        /// <returns></returns>
        protected IBoardPosition RelativePositionToTestedPiece(IBoardPosition boardPosition)
        {
            return TestedPieceColour == PieceColour.White ? boardPosition : GetMirroredBoardPosition(boardPosition);
        }


        private IBoardPosition GetMirroredBoardPosition(IBoardPosition boardPosition)
        {
            return new BoardPosition(Math.Abs(boardPosition.X - 7), Math.Abs(boardPosition.Y - 7));
        }


        protected void SetTestedPieceColourWith(PieceType currentPieceType)
        {
            _ = TestedPieceColour = GetPieceColourFrom(currentPieceType);
        }

        private PieceColour GetPieceColourFrom(PieceType pieceType)
        {
            return pieceType.ToString().StartsWith("White") ? PieceColour.White : PieceColour.Black;
        }

        protected IEnumerable<IBoardPosition> GetPositionsIncludingAndPassed(IBoardPosition boardPosition,
            Direction direction)
        {
            if (boardPosition.X > 7 || boardPosition.X < 0 || boardPosition.Y > 7 || boardPosition.Y < 0)
                return new List<IBoardPosition>();
            var nextBoardPosition =
                new BoardPosition(boardPosition.X + Move.In(direction).X, boardPosition.Y + Move.In(direction).Y);
            return GetPositionsIncludingAndPassed(nextBoardPosition, direction)
                .Concat(new List<IBoardPosition> {boardPosition});
        }
    }
}