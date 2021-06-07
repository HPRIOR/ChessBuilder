using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleBishopMovesFactory
    {
        private readonly BishopTurnMoves.Factory _factory;

        public PossibleBishopMovesFactory(BishopTurnMoves.Factory factory)
        {
            _factory = factory;
        }

        public BishopTurnMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}