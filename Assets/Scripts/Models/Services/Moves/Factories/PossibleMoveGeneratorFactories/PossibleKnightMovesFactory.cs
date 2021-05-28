using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleKnightMovesFactory
    {
        private readonly PossibleKnightMoves.Factory _factory;

        public PossibleKnightMovesFactory(PossibleKnightMoves.Factory factory)
        {
            _factory = factory;
        }

        public PossibleKnightMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}