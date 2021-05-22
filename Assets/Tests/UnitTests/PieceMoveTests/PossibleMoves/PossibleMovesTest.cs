using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PieceMoveTests.PossibleMoves
{
    [TestFixture]
    public class PossibleMovesTest : ZenjectUnitTestFixture
    {
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
        }

        private IBoardGenerator _boardGenerator;
        private IPossibleMovesGenerator _possibleMovesGenerator;

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _possibleMovesGenerator = Container.Resolve<IPossibleMovesGenerator>();
        }

        [Test]
        public void WithNoPieces_NoPossibleMoves()
        {
            var board = new BoardState(_boardGenerator);
            var possibleMoves = _possibleMovesGenerator.GeneratePossibleMoves(board, PieceColour.White);
            Assert.AreEqual(0, possibleMoves.Count());
        }
    }
}