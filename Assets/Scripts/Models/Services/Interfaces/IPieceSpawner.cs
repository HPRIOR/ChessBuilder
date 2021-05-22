using Models.State.Interfaces;
using Models.State.PieceState;
using View.Renderers;

namespace Models.Services.Interfaces
{
    public interface IPieceSpawner
    {
        PieceMono CreatePiece(PieceType pieceType, IBoardPosition BoardPosition);
    }
}