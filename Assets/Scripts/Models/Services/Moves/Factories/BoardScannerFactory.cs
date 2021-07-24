using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.Services.Moves.Utils.Scanners;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class BoardScannerFactory : IBoardScannerFactory
    {
        private readonly BoardScanner.Factory _boardScannerFactory;
        private readonly NonTurnBoardScanner.Factory _nonTurnBoardScannerFactory;

        public BoardScannerFactory(BoardScanner.Factory boardScannerFactory,
            NonTurnBoardScanner.Factory nonTurnBoardScannerFactory)
        {
            _boardScannerFactory = boardScannerFactory;
            _nonTurnBoardScannerFactory = nonTurnBoardScannerFactory;
        }

        public IBoardScanner Create(PieceColour pieceColour, Turn turnType)
        {
            if (turnType == Turn.NonTurn) return _nonTurnBoardScannerFactory.Create(pieceColour);
            return _boardScannerFactory.Create(pieceColour);
        }
    }
}