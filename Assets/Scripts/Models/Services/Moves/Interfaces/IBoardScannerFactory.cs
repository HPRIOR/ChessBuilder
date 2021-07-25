﻿using Models.Services.Moves.Utils;
using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface IBoardScannerFactory
    {
        IBoardScanner Create(PieceColour pieceColour, Turn turnType);
    }
}