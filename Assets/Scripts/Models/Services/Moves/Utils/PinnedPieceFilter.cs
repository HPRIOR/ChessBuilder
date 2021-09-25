using System.Collections.Generic;
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


        /*
         * New algo: fix direction map so incoherent directions produce Null Direction.
         * For each enemy scanning piece, if their direction to king != Null direction
         * generate the moves in that direction and check the board against them for
         * 'the next piece is king' if so then intersect the pinned piece moves from that
         * of the enemy pieces moves.
         * 
         */
        public void FilterMoves(IBoardInfo boardInfo, BoardState boardState)
        {
            var turnMoves = boardInfo.TurnMoves;
            var enemyScanningMoves = GetScanningPiecesMoves(boardInfo.EnemyMoves, boardState);
            var kingPosition = boardInfo.KingPosition;
            var turnPiecePosition = new HashSet<Position>(turnMoves.Keys);
            turnPiecePosition.Remove(kingPosition);
            foreach (var enemyMoves in enemyScanningMoves)
            {
                var potentialPinnedPieces =
                    enemyMoves.Value.Intersect(turnPiecePosition).ToList();
                if (!potentialPinnedPieces.Any()) continue;
                foreach (var turnPiece in potentialPinnedPieces)
                    // if (DirectionOfPinPointsToKing(kingPosition, turnPiece, enemyMoves.Key))
                    // {
                    if (TheNextPieceIsKing(enemyMoves.Key, turnPiece, kingPosition, boardState))
                    {
                        turnMoves[turnPiece] = turnMoves[turnPiece]
                            .Intersect(PossibleEscapeMoves(kingPosition, turnPiece, enemyMoves.Key))
                            .ToList();
                    }

                return;
                // }
            }
        }

        private static HashSet<Position> PossibleEscapeMoves(
            Position kingPosition, Position pinnedPiecePosition, Position pinningPiecePosition)
        {
            var positionsBetweenPinAndKing = new HashSet<Position>(ScanCache.ScanTo(kingPosition, pinningPiecePosition));
            positionsBetweenPinAndKing.Remove(pinnedPiecePosition);

            return positionsBetweenPinAndKing;
        }

        private static bool DirectionOfPinPointsToKing(Position kingPosition, Position pinnedPosition,
            Position pinningPosition)
        {
            var pinDirection = DirectionCache.DirectionFrom(pinningPosition, pinnedPosition);
            var pinnedToKingDirection = DirectionCache.DirectionFrom(pinnedPosition, kingPosition);
            return pinDirection == pinnedToKingDirection;
        }

        private static bool ContainsNonKingPiece(BoardState boardState, Position kingPosition,
            Position targetPosition)
        {
            var (x, y) = (targetPosition.X, targetPosition.Y);
            return targetPosition != kingPosition &&
                   boardState.Board[x][y].CurrentPiece.Type != PieceType.NullPiece;
        }


        /*
         * The order of scanned board positions is wrong, so the logic is incorrect 
         */
        private static bool TheNextPieceIsKing(Position enemyPosition, Position turnPiecePosition,
            Position kingPosition, BoardState boardState)
        {
            var scannedBoardPositions =
                ScanCache.Scan(turnPiecePosition, DirectionCache.DirectionFrom(enemyPosition, kingPosition));
            foreach (var position in scannedBoardPositions)
            {
                if (position == kingPosition) return true;

                if (ContainsNonKingPiece(boardState, kingPosition,
                    position)) // escape early if piece obstructs possible king 
                    return false;
            }

            return false;
        }

        private bool PieceIsScanner(KeyValuePair<Position, List<Position>> pieceMoves,
            BoardState boardState)
        {
            var pieceAtBoardPosition = boardState.Board[pieceMoves.Key.X][pieceMoves.Key.Y].CurrentPiece.Type;
            return _scanningPieces.Contains(pieceAtBoardPosition);
        }

        private IDictionary<Position, List<Position>> GetScanningPiecesMoves(
            IDictionary<Position, List<Position>> moves, BoardState boardState) =>
            moves
                .Where(keyVal => PieceIsScanner(keyVal, boardState))
                .ToDictionary(keyVal => keyVal.Key, keyVal => keyVal.Value);
    }
}