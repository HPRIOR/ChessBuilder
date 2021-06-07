using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleKnightMovesFactory
    {
        private readonly KnightTurnMoves.Factory _factory;

        public PossibleKnightMovesFactory(KnightTurnMoves.Factory factory)
        {
            _factory = factory;
        }

        public KnightTurnMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}