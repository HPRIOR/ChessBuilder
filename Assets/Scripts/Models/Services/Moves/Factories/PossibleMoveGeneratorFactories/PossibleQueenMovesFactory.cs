using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleQueenMovesFactory
    {
        private readonly PossibleQueenMoves.Factory _factory;

        public PossibleQueenMovesFactory(PossibleQueenMoves.Factory factory)
        {
            _factory = factory;
        }

        public PossibleQueenMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}