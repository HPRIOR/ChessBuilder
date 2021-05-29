using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class BoardScannerFactory : IBoardScannerFactory
    {
        private readonly BoardScanner.Factory _boardScannerFactory;

        public BoardScannerFactory(BoardScanner.Factory boardScannerFactory)
        {
            _boardScannerFactory = boardScannerFactory;
        }

        public IBoardScanner Create(PieceColour pieceColour)
        {
            return _boardScannerFactory.Create(pieceColour);
        }
    }
}