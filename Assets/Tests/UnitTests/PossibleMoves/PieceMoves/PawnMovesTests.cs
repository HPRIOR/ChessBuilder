﻿using System.Collections.Generic;
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
    public class PawnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _whitePawnTurnMoves;
        private IPieceMoveGenerator _blackPawnTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var possibleMoveFactory = Container.Resolve<PossibleMovesFactory>();
            _whitePawnTurnMoves = possibleMoveFactory.Create(PieceType.WhitePawn, true);
            _blackPawnTurnMoves = possibleMoveFactory.Create(PieceType.BlackPawn, true);

            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void White_PawnCanMoveForward()
        {
            var pieces = new List<(PieceType, Position )>
            {
                (PieceType.WhitePawn, new Position(4, 1))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whitePawnTurnMoves.GetPossiblePieceMoves(new Position(4, 1), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(4, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void Black_PawnCanMoveForward()
        {
            var pieces = new List<(PieceType, Position )>
            {
                (PieceType.BlackPawn, new Position(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackPawnTurnMoves.GetPossiblePieceMoves(new Position(4, 6), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(4, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void White_PawnCanTakeDiagonally()
        {
            var pieces = new List<(PieceType, Position )>
            {
                (PieceType.WhitePawn, new Position(4, 1)),
                (PieceType.BlackPawn, new Position(5, 2)),
                (PieceType.BlackPawn, new Position(3, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whitePawnTurnMoves.GetPossiblePieceMoves(new Position(4, 1), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(4, 2),
                new Position(5, 2),
                new Position(3, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void Black_PawnCanTakeDiagonally()
        {
            var pieces = new List<(PieceType, Position )>
            {
                (PieceType.BlackPawn, new Position(4, 6)),
                (PieceType.WhitePawn, new Position(3, 5)),
                (PieceType.WhitePawn, new Position(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackPawnTurnMoves.GetPossiblePieceMoves(new Position(4, 6), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(4, 5),
                new Position(5, 5),
                new Position(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void White_PawnIsBlockedByFriendlyPiece()
        {
            var pieces = new List<(PieceType, Position )>
            {
                (PieceType.WhitePawn, new Position(4, 1)),
                (PieceType.WhitePawn, new Position(4, 2))
            };


            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whitePawnTurnMoves.GetPossiblePieceMoves(new Position(4, 1), boardState);
            var expectedMoves = new List<Position>();

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void Black_PawnIsBlockedByFriendlyPiece()
        {
            var pieces = new List<(PieceType, Position )>
            {
                (PieceType.BlackPawn, new Position(4, 6)),
                (PieceType.BlackPawn, new Position(4, 5))
            };


            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackPawnTurnMoves.GetPossiblePieceMoves(new Position(4, 6), boardState);
            var expectedMoves = new List<Position>();

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}