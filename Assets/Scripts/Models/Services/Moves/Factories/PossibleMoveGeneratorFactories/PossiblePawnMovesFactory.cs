using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossiblePawnMovesFactory
    {
        private readonly PawnTurnMoves.Factory _factory;

        public PossiblePawnMovesFactory(PawnTurnMoves.Factory factory)
        {
            _factory = factory;
        }

        public PawnTurnMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}