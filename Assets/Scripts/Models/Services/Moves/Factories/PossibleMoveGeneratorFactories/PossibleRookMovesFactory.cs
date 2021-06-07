using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleRookMovesFactory
    {
        private readonly RookTurnMoves.Factory _factory;

        public PossibleRookMovesFactory(RookTurnMoves.Factory factory)
        {
            _factory = factory;
        }

        public RookTurnMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}