using System;
using System.Collections.Generic;
using System.Linq;
using Bindings.Installers.AIInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Models.Services.AI.Implementations;
using Models.Services.AI.Interfaces;
using Models.Services.Board;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.AI
{
    [TestFixture]
    public class MoveOrderingTests : ZenjectUnitTestFixture
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

        private IBoardGenerator _boardGenerator;
        private IMoveOrderer _moveOrderer;
        private readonly Action<PieceColour, IGameStateUpdater> _actionStub = (colour, updater) => { };

        private void InstallBindings()
        {
            MoveOrdererInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _moveOrderer = Container.Resolve<IMoveOrderer>();
        }

        [Test]
        public void MovesAreOrderedInDescendingOrder()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0][0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1][1].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[2][2].CurrentPiece = new Piece(PieceType.BlackQueen);
            board[5][5].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[6][6].CurrentPiece = new Piece(PieceType.BlackRook);
            var boardState = new BoardState(board);

            var highestMove = new AiMove(MoveType.Move, new Position(1, 1), new Position(2, 2),
                PieceType.NullPiece);
            var midMove = new AiMove(MoveType.Move, new Position(5, 5), new Position(6, 6),
                PieceType.NullPiece);
            var lowestMove = new AiMove(MoveType.Move, new Position(0, 0), new Position(0, 1),
                PieceType.NullPiece);
            var actionList = new List<AiMove>
            {
                midMove,
                lowestMove,
                highestMove
            };

            var sut = _moveOrderer.OrderMoves(actionList, boardState).ToList();
            Assert.That(sut[0].From, Is.EqualTo(highestMove.From));
            Assert.That(sut[1].From, Is.EqualTo(midMove.From));
            Assert.That(sut[2].From, Is.EqualTo(lowestMove.From));
        }

        [Test]
        public void NonTakeMovesScoreZero()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0][0].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[1][1].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[2][2].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[5][5].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[6][6].CurrentPiece = new Piece(PieceType.BlackPawn);
            var boardState = new BoardState(board);

            var highestMove = new AiMove(MoveType.Move, new Position(1, 1), new Position(2, 2),
                PieceType.NullPiece);
            var midMove = new AiMove(MoveType.Move, new Position(5, 5), new Position(6, 6),
                PieceType.NullPiece);
            var lowestMove = new AiMove(MoveType.Move, new Position(0, 0), new Position(0, 1),
                PieceType.NullPiece);
            var actionList = new List<AiMove>
            {
                midMove,
                lowestMove,
                highestMove
            };

            var sut = _moveOrderer.OrderMoves(actionList, boardState).ToList();
            Assert.That(sut[0].From, Is.EqualTo(highestMove.From));
            Assert.That(sut[1].From, Is.EqualTo(midMove.From));
            Assert.That(sut[2].From, Is.EqualTo(lowestMove.From));
        }
    }
}