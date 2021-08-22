﻿using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Utils;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.Utils
{
    public class PinnedPieceFilter
    {
        private readonly HashSet<PieceType> _scanningPieces = new HashSet<PieceType>
        {
            PieceType.BlackBishop,
            PieceType.BlackQueen,
            PieceType.BlackRook,
            PieceType.WhiteBishop,
            PieceType.WhiteQueen,
            PieceType.WhiteRook
        };


        public void FilterMoves(IBoardInfo boardInfo, BoardState boardState)
        {
            var turnMoves = boardInfo.TurnMoves;
            var enemyScanningMoves = GetScanningPiecesMoves(boardInfo.EnemyMoves, boardState);
            var kingPosition = boardInfo.KingPosition;
            var turnPiecePosition = new HashSet<Position>(turnMoves.Keys);
            turnPiecePosition.Remove(kingPosition);
            foreach (var enemyMoves in enemyScanningMoves)
            {
                var turnPiecesPositionWhichCanBeTaken =
                    enemyMoves.Value.Intersect(turnPiecePosition).ToList();
                if (!turnPiecesPositionWhichCanBeTaken.Any()) continue;
                foreach (var turnPiece in turnPiecesPositionWhichCanBeTaken)
                    if (DirectionOfPinPointsToKing(kingPosition, turnPiece, enemyMoves.Key))
                    {
                        if (TheNextPieceIsKing(enemyMoves.Key, turnPiece, kingPosition, boardState))
                            turnMoves[turnPiece]
                                .IntersectWith(PossibleEscapeMoves(kingPosition, turnPiece, enemyMoves.Key));
                        return;
                    }
            }
        }

        private static HashSet<Position> PossibleEscapeMoves(
            Position kingPosition, Position pinnedPiecePosition, Position pinningPiecePosition)
        {
            var positionsBetweenPinAndKing = new HashSet<Position>(ScanMap.ScanTo(kingPosition, pinningPiecePosition));
            positionsBetweenPinAndKing.Remove(pinnedPiecePosition);

            return positionsBetweenPinAndKing;
        }

        private static bool DirectionOfPinPointsToKing(Position kingPosition, Position pinnedPosition,
            Position pinningPosition)
        {
            var pinDirection = DirectionMap.DirectionFrom(pinningPosition, pinnedPosition);
            var pinnedToKingDirection = DirectionMap.DirectionFrom(pinnedPosition, kingPosition);
            return pinDirection == pinnedToKingDirection;
        }

        private static bool ContainsNonKingPiece(BoardState boardState, Position kingPosition,
            Position targetPosition)
        {
            var (x, y) = (targetPosition.X, targetPosition.Y);
            return targetPosition != kingPosition &&
                   boardState.Board[x, y].CurrentPiece.Type != PieceType.NullPiece;
        }


        /*
         * The order of scanned board positions is wrong, so the logic is incorrect 
         */
        private static bool TheNextPieceIsKing(Position enemyPosition, Position turnPiecePosition,
            Position kingPosition, BoardState boardState)
        {
            var scannedBoardPositions =
                ScanMap.Scan(turnPiecePosition, DirectionMap.DirectionFrom(enemyPosition, kingPosition));
            foreach (var position in scannedBoardPositions)
            {
                if (position == kingPosition) return true;

                if (ContainsNonKingPiece(boardState, kingPosition,
                    position)) // escape early if piece obstructs possible king 
                    return false;
            }

            return false;
        }

        private bool PieceIsScanner(KeyValuePair<Position, HashSet<Position>> pieceMoves,
            BoardState boardState)
        {
            var pieceAtBoardPosition = boardState.Board[pieceMoves.Key.X, pieceMoves.Key.Y].CurrentPiece.Type;
            return _scanningPieces.Contains(pieceAtBoardPosition);
        }

        private IDictionary<Position, HashSet<Position>> GetScanningPiecesMoves(
            IDictionary<Position, HashSet<Position>> moves, BoardState boardState) =>
            moves
                .Where(keyVal => PieceIsScanner(keyVal, boardState))
                .ToDictionary(keyVal => keyVal.Key, keyVal => keyVal.Value);
    }
}