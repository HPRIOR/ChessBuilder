using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Tests.UnitTests.PossibleMoves.PossibleMoves.Utils;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
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
        private IAllPossibleMovesGenerator _allPossibleMovesGenerator;

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _allPossibleMovesGenerator = Container.Resolve<IAllPossibleMovesGenerator>();
        }

        [Test]
        public void WithNoPieces_NoPossibleMoves()
        {
            var board = new BoardState(_boardGenerator);
            var possibleMoves = _allPossibleMovesGenerator.GetPossibleMoves(board, PieceColour.White);
            Assert.AreEqual(0, possibleMoves.Count());
        }
    }
}