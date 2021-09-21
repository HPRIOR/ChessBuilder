﻿using System.Linq;
using Models.Services.AI.Interfaces;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.AI.Implementations
{
    public class StaticEvaluator : IStaticEvaluator
    {
        public BoardScore Evaluate(GameState gameState)
        {
            var blackPoints = 0;
            var whitePoints = 0;
            var activePieces = gameState.BoardState.ActivePieces;
            foreach (var position in activePieces)
            {
                ref var tile = ref gameState.BoardState.GetTileAt(position);
                var currentPiece = tile.CurrentPiece;
                var multiplier = 100;
                if (currentPiece.Colour == PieceColour.Black)
                    blackPoints += currentPiece.Type.Value() * multiplier;
                else
                    whitePoints += currentPiece.Type.Value() * multiplier;
            }

            return new BoardScore(blackPoints, whitePoints);
        }
    }
}