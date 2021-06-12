using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class TileEvaluatorFactory : ITileEvaluatorFactory
    {
        private readonly TileEvaluator.Factory _tileEvalFactory;

        public TileEvaluatorFactory(TileEvaluator.Factory tileEvalFactory)
        {
            _tileEvalFactory = tileEvalFactory;
        }

        public ITileEvaluator Create(PieceColour pieceColour) => _tileEvalFactory.Create(pieceColour);
    }
}