﻿using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Tests.UnitTests.PossibleMoves.PieceMoves.Utils;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class BoardEvalTests : ZenjectUnitTestFixture
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

        private IBoardInfo _boardInfo;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            PossibleMovesBinder.InstallBindings(Container);
        }

        private void ResolveContainer()
        {
            _boardInfo = Container.Resolve<IBoardInfo>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void BoardEval_IsInstalled()
        {
            Assert.IsNotNull(_boardInfo);
        }

        [Test]
        public void OnBlackTurn_MovesAreDividedBetweenTurn(
        )
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardInfo.EvaluateBoard(boardState, PieceColour.Black);

            Assert.That(_boardInfo.TurnMoves, Contains.Key(new BoardPosition(7, 7)));
            Assert.That(_boardInfo.TurnMoves, Contains.Key(new BoardPosition(7, 6)));
            Assert.That(_boardInfo.NonTurnMoves, Contains.Key(new BoardPosition(1, 1)));
            Assert.That(_boardInfo.NonTurnMoves, Contains.Key(new BoardPosition(1, 2)));
            Assert.That(_boardInfo.TurnMoves.Count(), Is.EqualTo(2));
            Assert.That(_boardInfo.NonTurnMoves.Count(), Is.EqualTo(2));
        }

        [Test]
        public void OnWhiteTurn_MovesAreDividedBetweenTurn()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardInfo.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardInfo.TurnMoves, Contains.Key(new BoardPosition(1, 1)));
            Assert.That(_boardInfo.TurnMoves, Contains.Key(new BoardPosition(1, 2)));
            Assert.That(_boardInfo.NonTurnMoves, Contains.Key(new BoardPosition(7, 7)));
            Assert.That(_boardInfo.NonTurnMoves, Contains.Key(new BoardPosition(7, 6)));
            Assert.That(_boardInfo.TurnMoves.Count(), Is.EqualTo(2));
            Assert.That(_boardInfo.NonTurnMoves.Count(), Is.EqualTo(2));
        }

        [Test]
        public void OnBlackTurn_FriendlyKingIsFound()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardInfo.EvaluateBoard(boardState, PieceColour.Black);

            Assert.That(_boardInfo.KingPosition, Is.EqualTo(new BoardPosition(7, 7)));
        }


        [Test]
        public void OnWhiteTurn_FriendlyKingIsFound()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardInfo.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardInfo.KingPosition, Is.EqualTo(new BoardPosition(1, 1)));
        }

        [Test]
        public void KingPositionNullIsOutOfBound()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardInfo.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardInfo.KingPosition, Is.EqualTo(new BoardPosition(8, 8)));
        }
    }
}