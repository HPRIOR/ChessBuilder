using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;
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

        public bool NoPieceIn(Tile tile) => tile.CurrentPiece == PieceType.NullPiece;

        public bool FriendlyPieceIn(Tile tile) => !(tile.CurrentPiece is PieceType.NullPiece) &&
                                                      tile.CurrentPiece.Colour() == _pieceColour;

        public bool OpposingPieceIn(Tile tile) => !(tile.CurrentPiece is PieceType.NullPiece) &&
                                                      tile.CurrentPiece.Colour() != _pieceColour;

        public sealed class Factory : PlaceholderFactory<PieceColour, TileEvaluator>
        {
        }
    }
}