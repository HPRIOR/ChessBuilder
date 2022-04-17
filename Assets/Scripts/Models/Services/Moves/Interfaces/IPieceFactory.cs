using Models.State.Board;
using Models.State.PieceState;
using View.Prefab.Interfaces;

namespace Models.Services.Moves.Interfaces
{
    public interface IPieceFactory
    {
        IPieceSpawner CreatePiece(PieceType pieceType, Position position);
    }
}