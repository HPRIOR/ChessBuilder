using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class TileEvaluator : ITileEvaluator
    {
        private readonly PieceColour _pieceColour;

        public TileEvaluator(PieceColour pieceColour)
        {
            _pieceColour = pieceColour;
        }

        public bool NoPieceIn(Tile tile) => tile.CurrentPiece.Type == PieceType.NullPiece;

        public bool FriendlyPieceIn(Tile tile) => !(tile.CurrentPiece.Type is PieceType.NullPiece) &&
                                                  tile.CurrentPiece.Colour == _pieceColour;

        public bool OpposingPieceIn(Tile tile) => !(tile.CurrentPiece.Type is PieceType.NullPiece) &&
                                                  tile.CurrentPiece.Colour != _pieceColour;

        public class Factory : PlaceholderFactory<PieceColour, TileEvaluator>
        {
        }
    }
}