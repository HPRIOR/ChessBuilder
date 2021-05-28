using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleBishopMovesFactory
    {
        private readonly PossibleBishopMoves.Factory _factory;

        public PossibleBishopMovesFactory(PossibleBishopMoves.Factory factory)
        {
            _factory = factory;
        }

        public PossibleBishopMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}