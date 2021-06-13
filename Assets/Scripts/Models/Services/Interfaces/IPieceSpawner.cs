using Models.State.Board;
using Models.State.PieceState;
using View.Renderers;

namespace Models.Services.Interfaces
{
    public interface IPieceSpawner
    {
        PieceMono CreatePiece(PieceType pieceType, Position position);
    }
}