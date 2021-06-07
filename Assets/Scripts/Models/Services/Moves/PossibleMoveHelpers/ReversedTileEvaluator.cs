using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class ReversedTileEvaluator : ITileEvaluator
    {
        private readonly PieceColour _pieceColour;

        public ReversedTileEvaluator(PieceColour pieceColour)
        {
            _pieceColour = pieceColour;
        }

        public bool NoPieceIn(Tile tile)
        {
            return tile.CurrentPiece.Type == PieceType.NullPiece;
        }

        public bool FriendlyPieceIn(Tile tile)
        {
            return !(tile.CurrentPiece.Type is PieceType.NullPiece) && tile.CurrentPiece.Colour != _pieceColour;
        }

        public bool OpposingPieceIn(Tile tile)
        {
            return !(tile.CurrentPiece.Type is PieceType.NullPiece) && tile.CurrentPiece.Colour == _pieceColour;
        }

        public class Factory : PlaceholderFactory<PieceColour, ReversedTileEvaluator>
        {
        }
    }
}