using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IBoardScannerFactory
    {
        IBoardScanner Create(PieceColour pieceColour);
    }
}