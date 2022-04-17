using System.Collections.Generic;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Models.Services.Game.Implementations;
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
            var pieces = new List<(PieceType piece, Position boardPosition)>
            {
                (PieceType.BlackQueen, new Position(1, 2)),
                (PieceType.BlackKnight, new Position(3, 3)),
                (PieceType.WhiteQueen, new Position(7, 7)),
                (PieceType.WhitePawn, new Position(5, 5)),
                (PieceType.WhiteKnight, new Position(7, 0))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            Assert.That(boardState.GetTileAt(1,2).CurrentPiece, Is.EqualTo(PieceType.BlackQueen));
            Assert.That(boardState.GetTileAt(3,3).CurrentPiece, Is.EqualTo(PieceType.BlackKnight));
            Assert.That(boardState.GetTileAt(7,7).CurrentPiece, Is.EqualTo(PieceType.WhiteQueen));
            Assert.That(boardState.GetTileAt(5,5).CurrentPiece, Is.EqualTo(PieceType.WhitePawn));
            Assert.That(boardState.GetTileAt(7,0).CurrentPiece, Is.EqualTo(PieceType.WhiteKnight));
        }
    }
}