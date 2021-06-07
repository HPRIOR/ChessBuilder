using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class TileEvaluatorFactory : ITileEvaluatorFactory
    {
        private readonly ReversedTileEvaluator.Factory _reversedTileEvalFactory;
        private readonly TileEvaluator.Factory _tileEvalFactory;

        public TileEvaluatorFactory(TileEvaluator.Factory tileEvalFactory,
            ReversedTileEvaluator.Factory reversedTileEvalFactory)
        {
            _tileEvalFactory = tileEvalFactory;
            _reversedTileEvalFactory = reversedTileEvalFactory;
        }

        public ITileEvaluator Create(PieceColour pieceColour, bool reversed)
        {
            if (reversed) return _reversedTileEvalFactory.Create(pieceColour);
            return _tileEvalFactory.Create(pieceColour);
        }
    }
}