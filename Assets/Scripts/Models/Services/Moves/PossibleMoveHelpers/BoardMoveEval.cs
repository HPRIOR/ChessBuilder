using Models.Services.Interfaces;
using Models.State.Interfaces;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class BoardMoveEval : IBoardMoveEval
    {
        private readonly PieceColour _pieceColour;

        public BoardMoveEval(PieceColour pieceColour)
        {
            _pieceColour = pieceColour;
        }

        public bool NoPieceIn(ITile tile)
        {
            return tile.CurrentPiece.Type == PieceType.NullPiece;
        }

        public bool FriendlyPieceIn(ITile tile)
        {
            return !(tile.CurrentPiece.Type is PieceType.NullPiece) && tile.CurrentPiece.Colour == _pieceColour;
        }

        public bool OpposingPieceIn(ITile tile)
        {
            return !(tile.CurrentPiece.Type is PieceType.NullPiece) && tile.CurrentPiece.Colour != _pieceColour;
        }

        public class Factory : PlaceholderFactory<PieceColour, BoardMoveEval>
        {
        }
    }
}