using Models.Services.Interfaces;
using Models.State.Interfaces;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class BoardEval : IBoardEval
    {
        private readonly PieceColour _pieceColour;

        public BoardEval(PieceColour pieceColour)
        {
            _pieceColour = pieceColour;
        }

        public bool NoPieceIn(ITile tile) => tile.CurrentPiece.Type == PieceType.NullPiece;

        public bool FriendlyPieceIn(ITile tile) =>
            !(tile.CurrentPiece.Type is PieceType.NullPiece) && PieceColourFromType(tile.CurrentPiece.Type) == _pieceColour;

        public bool OpposingPieceIn(ITile tile) =>
            !(tile.CurrentPiece.Type is PieceType.NullPiece) && PieceColourFromType(tile.CurrentPiece.Type) != _pieceColour;

        private static PieceColour PieceColourFromType(PieceType pieceType) => pieceType.ToString().StartsWith("White") ? PieceColour.White : PieceColour.Black;

        public class Factory : PlaceholderFactory<PieceColour, BoardEval> { }
    }
}