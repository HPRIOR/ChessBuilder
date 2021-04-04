public class BoardScannerFactory : IBoardScannerFactory
{
    private readonly BoardScanner.Factory _boardScannerFactory;

    public BoardScannerFactory(BoardScanner.Factory boardScannerFactory)
    {
        _boardScannerFactory = boardScannerFactory;
    }

    public IBoardScanner Create(PieceColour pieceColour) => _boardScannerFactory.Create(pieceColour);
}