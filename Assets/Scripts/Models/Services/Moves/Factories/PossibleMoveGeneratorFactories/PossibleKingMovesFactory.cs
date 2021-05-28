using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleKingMovesFactory
    {
        private readonly PossibleKingMoves.Factory _factory;

        public PossibleKingMovesFactory(PossibleKingMoves.Factory factory)
        {
            _factory = factory;
        }

        public PossibleKingMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}