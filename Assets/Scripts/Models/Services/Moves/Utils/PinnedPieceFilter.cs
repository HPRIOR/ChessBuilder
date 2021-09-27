using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

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

        private static readonly HashSet<Direction> BishopDirections = new HashSet<Direction>(new DirectionComparer())
        {
            Direction.NE, Direction.NW, Direction.SE, Direction.SW
        };

        private static readonly HashSet<Direction> RookDirections = new HashSet<Direction>(new DirectionComparer())
        {
            Direction.N, Direction.E, Direction.S, Direction.W
        };

        private static readonly HashSet<Direction> QueenDirections = new HashSet<Direction>(new DirectionComparer())
        {
            Direction.N, Direction.E, Direction.S, Direction.W,
            Direction.NE, Direction.NW, Direction.SE, Direction.SW
        };

        private static bool DirectionIsInPieceMoveRepetitious(Direction direction, Position piecePosition,
            BoardState boardState)
        {
            var pieceType = boardState.GetTileAt(piecePosition).CurrentPiece.Type;
            switch (pieceType)
            {
                case PieceType.BlackBishop:
                case PieceType.WhiteBishop:
                    return BishopDirections.Contains(direction);
                case PieceType.BlackRook:
                case PieceType.WhiteRook:
                    return RookDirections.Contains(direction);
                case PieceType.WhiteQueen:
                case PieceType.BlackQueen:
                    return QueenDirections.Contains(direction);
                default:
                    return false;
            }
        }

        private static bool PieceCanMoveToKing(Position piecePosition, Position kingPosition,
            BoardState boardState)
        {
            var potentialDirectionToKing = DirectionCache.DirectionFrom(piecePosition, kingPosition);
            var directionExists = potentialDirectionToKing != Direction.Null;
            return directionExists && DirectionIsInPieceMoveRepetitious(potentialDirectionToKing, piecePosition, boardState);
        }

        private (bool, Position, int) PinExists(IEnumerable<Position> positions, BoardState boardState,
            PieceColour kingColour, Position kingPosition)
        {
            var potentialPinHasBeenFound = false;
            var pinnedPiece = new Position();
            var index = 0;
            foreach (var position in positions)
            {
                ref var tile = ref boardState.GetTileAt(position);

                var pieceType = tile.CurrentPiece.Type;
                if (pieceType == PieceType.NullPiece)
                {
                    index++;
                    continue;
                }

                var pieceColour = pieceType.Colour(); // piece is 

                var pieceIsNotTurnColour = !potentialPinHasBeenFound && pieceColour != kingColour;
                if (pieceIsNotTurnColour)
                    return (false, new Position(), index); // no pin can occur if enemy blocked enemy

                var firstPieceWhichCouldBePinned = !potentialPinHasBeenFound && pieceColour == kingColour;
                if (firstPieceWhichCouldBePinned)
                {
                    potentialPinHasBeenFound = true;
                    pinnedPiece = position;
                    index++;
                    continue;
                }

                var kingIsNextPiece = potentialPinHasBeenFound && position == kingPosition;
                if (kingIsNextPiece)
                {
                    return (true, pinnedPiece, index);
                }

                if (potentialPinHasBeenFound) return (false, new Position(), index); // piece has blocked pin
            }

            return (false, new Position(), 0);
        }

        public void FilterMovesTwo(IBoardInfo boardInfo, BoardState boardState)
        {
            var kingPosition = boardInfo.KingPosition;
            if (kingPosition == new Position(8, 8))
                return;
            var kingColour = boardState.GetTileAt(kingPosition).CurrentPiece.Type.Colour();
            var enemyScanningMoves = GetScanningPiecesMoves(boardInfo.EnemyMoves, boardState);
            foreach (var enemyScanningMove in enemyScanningMoves)
            {
                var pieceCanMoveToKing = PieceCanMoveToKing(enemyScanningMove.Key, kingPosition, boardState);
                if (pieceCanMoveToKing)
                {
                    var kingThreatMoves = ScanCache.ScanTo(enemyScanningMove.Key, kingPosition).ToList();
                    var (pinExists, pinnedPiecePosition, pinnedPieceIndex) =
                        PinExists(kingThreatMoves, boardState, kingColour, kingPosition);
                    if (pinExists) // this should return the pinned piece
                    {
                        kingThreatMoves.RemoveAt(pinnedPieceIndex);
                        kingThreatMoves.Add(enemyScanningMove.Key);
                        boardInfo.TurnMoves[pinnedPiecePosition] = boardInfo.TurnMoves[pinnedPiecePosition]
                            .Intersect(kingThreatMoves).ToList(); // need to remove position of pinned piece
                    }
                }
            }
        }


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
            var positionsBetweenPinAndKing =
                new HashSet<Position>(ScanCache.ScanTo(kingPosition, pinningPiecePosition));
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

        private static bool TheNextPieceIsKingTwo(IEnumerable<Position> scannedBoardPositions, Position kingPosition,
            BoardState boardState)
        {
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