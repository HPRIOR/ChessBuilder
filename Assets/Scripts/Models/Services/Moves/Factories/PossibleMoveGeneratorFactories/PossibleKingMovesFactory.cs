using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleKingMovesFactory
    {
        private readonly KingTurnMoves.Factory _factory;

        public PossibleKingMovesFactory(KingTurnMoves.Factory factory)
        {
            _factory = factory;
        }

        public KingTurnMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}