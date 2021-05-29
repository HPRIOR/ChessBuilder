using System;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public enum ScannerType
    {
        Normal,
        Unrestricted
    }

    public class BoardScannerFactory : IBoardScannerFactory
    {
        private readonly BoardScanner.Factory _boardScannerFactory;
        private readonly UnrestrictedBoardScanner.Factory _unrestrictedBoardScannerFactory;

        public BoardScannerFactory(BoardScanner.Factory boardScannerFactory,
            UnrestrictedBoardScanner.Factory unrestrictedBoardScannerFactory)
        {
            _boardScannerFactory = boardScannerFactory;
            _unrestrictedBoardScannerFactory = unrestrictedBoardScannerFactory;
        }

        public IBoardScanner Create(PieceColour pieceColour, ScannerType scannerType)
        {
            return scannerType switch
            {
                ScannerType.Normal => _boardScannerFactory.Create(pieceColour),
                ScannerType.Unrestricted => _unrestrictedBoardScannerFactory.Create(pieceColour),
                _ => throw new ArgumentOutOfRangeException(nameof(scannerType), scannerType, null)
            };
        }
    }
}