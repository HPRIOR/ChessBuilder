public interface IBoardScannerFactory
{
    IBoardScanner Create(PieceColour pieceColour);
}