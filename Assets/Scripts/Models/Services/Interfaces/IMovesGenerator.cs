﻿using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IMovesGenerator
    {
        IDictionary<Position, HashSet<Position>> GetPossibleMoves(BoardState boardState, PieceColour turn);
    }
}