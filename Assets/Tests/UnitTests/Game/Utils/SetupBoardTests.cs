﻿using System.Collections.Generic;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Game.Implementations;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Game.Utils
{
    [TestFixture]
    public class SetupBoardTests : ZenjectUnitTestFixture
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

        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            BoardSetupInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void BoardIsSetup()
        {
            var pieces = new List<(PieceType piece, BoardPosition boardPosition)>
            {
                (PieceType.BlackQueen, new BoardPosition(1, 2)),
                (PieceType.BlackKnight, new BoardPosition(3, 3)),
                (PieceType.WhiteQueen, new BoardPosition(7, 7)),
                (PieceType.WhitePawn, new BoardPosition(5, 5)),
                (PieceType.WhiteKnight, new BoardPosition(7, 0))
            };

            var boardState = _boardSetup.SetupBoard(pieces);

            Assert.That(boardState.Board[1, 2].CurrentPiece.Type, Is.EqualTo(PieceType.BlackQueen));
            Assert.That(boardState.Board[3, 3].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKnight));
            Assert.That(boardState.Board[7, 7].CurrentPiece.Type, Is.EqualTo(PieceType.WhiteQueen));
            Assert.That(boardState.Board[5, 5].CurrentPiece.Type, Is.EqualTo(PieceType.WhitePawn));
            Assert.That(boardState.Board[7, 0].CurrentPiece.Type, Is.EqualTo(PieceType.WhiteKnight));
        }
    }
}