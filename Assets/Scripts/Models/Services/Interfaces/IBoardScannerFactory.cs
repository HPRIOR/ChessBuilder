using Models.Services.Moves.Factories;
using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IBoardScannerFactory
    {
        IBoardScanner Create(PieceColour pieceColour, ScannerType scannerType);
    }
}