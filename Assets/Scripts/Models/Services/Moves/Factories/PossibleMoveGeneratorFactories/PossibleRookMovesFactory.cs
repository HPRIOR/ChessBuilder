using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleRookMovesFactory
    {
        private readonly PossibleRookMoves.Factory _factory;

        public PossibleRookMovesFactory(PossibleRookMoves.Factory factory)
        {
            _factory = factory;
        }

        public PossibleRookMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}