using Models.State.Board;
using Models.State.PieceState;
using View.Utils;

namespace Models.Services.Interfaces
{
    public interface IPieceFactory
    {
        PieceSpawner CreatePiece(PieceType pieceType, Position position);
    }
}