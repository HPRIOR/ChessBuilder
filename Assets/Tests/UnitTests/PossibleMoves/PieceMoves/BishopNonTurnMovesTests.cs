﻿using System.Collections.Generic;
using System.Linq;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Game.Implementations;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    [TestFixture]
    public class BishopNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _whiteBishopNonTurnMoves;
        private IPieceMoveGenerator _blackBishopNonTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleMovesFactoryInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var possibleMovesFactory = Container.Resolve<PossibleMovesFactory>();
            _whiteBishopNonTurnMoves = possibleMovesFactory.Create(PieceType.WhiteBishop, false);
            _blackBishopNonTurnMoves = possibleMovesFactory.Create(PieceType.BlackBishop, false);
            _boardSetup = Container.Resolve<BoardSetup>();
        }


        [Test]
        public void WithFriendlyPiece_White_BishopCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteBishop, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(7, 7)));
        }


        [Test]
        public void WithFriendlyPiece_Black_BishopCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackBishop, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(7, 7)));
        }


        [Test]
        public void WithOpposingPiece_White_BishopIsNotBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteBishop, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(7, 7)));
        }


        [Test]
        public void WithOpposingPiece_Black_BishopIsNotBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackBishop, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(7, 7)));
        }
    }
}