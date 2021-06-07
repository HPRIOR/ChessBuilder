using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleQueenMovesFactory
    {
        private readonly QueenTurnMoves.Factory _factory;

        public PossibleQueenMovesFactory(QueenTurnMoves.Factory factory)
        {
            _factory = factory;
        }

        public QueenTurnMoves Create(PieceColour pieceColour)
        {
            return _factory.Create(pieceColour);
        }
    }
}