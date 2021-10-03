using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.Utils
{
    public sealed class TileEvaluator : ITileEvaluator
    {
        private readonly PieceColour _pieceColour;

        public TileEvaluator(PieceColour pieceColour)
        {
            _pieceColour = pieceColour;
        }

        public bool NoPieceIn(ref Tile tile) => tile.CurrentPiece.Type == PieceType.NullPiece;

        public bool FriendlyPieceIn(ref Tile tile) => !(tile.CurrentPiece.Type is PieceType.NullPiece) &&
                                                  tile.CurrentPiece.Colour == _pieceColour;

        public bool OpposingPieceIn(ref Tile tile) => !(tile.CurrentPiece.Type is PieceType.NullPiece) &&
                                                  tile.CurrentPiece.Colour != _pieceColour;

        public sealed class Factory : PlaceholderFactory<PieceColour, TileEvaluator>
        {
        }
    }
}