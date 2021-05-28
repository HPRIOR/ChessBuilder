using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossiblePawnMovesFactory
    {
        private readonly PossiblePawnMoves.Factory _factory;

        public PossiblePawnMovesFactory(PossiblePawnMoves.Factory factory)
        {
            _factory = factory;
        }

        public PossiblePawnMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}