using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class TileEvaluatorFactory : ITileEvaluatorFactory
    {
        private readonly TileEvaluator.Factory _boardEvalFactory;

        public TileEvaluatorFactory(TileEvaluator.Factory boardEvalFactory)
        {
            _boardEvalFactory = boardEvalFactory;
        }

        public ITileEvaluator Create(PieceColour pieceColour)
        {
            return _boardEvalFactory.Create(pieceColour);
        }
    }
}