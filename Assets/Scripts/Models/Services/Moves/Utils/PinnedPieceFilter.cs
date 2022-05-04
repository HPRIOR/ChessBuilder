using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Moves.Utils
{
    public sealed class PinnedPieceFilter
    {
        private static readonly HashSet<PieceType> ScanningPieces = new(new PieceTypeComparer())
        {
            PieceType.BlackBishop,
            PieceType.BlackQueen,
            PieceType.BlackRook,
            PieceType.WhiteBishop,
            PieceType.WhiteQueen,
            PieceType.WhiteRook
        };

        private static readonly HashSet<Direction> BishopDirections = new(new DirectionComparer())
        {
            Direction.Ne, Direction.Nw, Direction.Se, Direction.Sw
        };

        private static readonly HashSet<Direction> RookDirections = new(new DirectionComparer())
        {
            Direction.N, Direction.E, Direction.S, Direction.W
        };

        private static readonly HashSet<Direction> QueenDirections = new(new DirectionComparer())
        {
            Direction.N, Direction.E, Direction.S, Direction.W,
            Direction.Ne, Direction.Nw, Direction.Se, Direction.Sw
        };

        private static bool PieceIsScanner(KeyValuePair<Position, List<Position>> pieceMoves,
            BoardState boardState)
        {
            var pieceAtBoardPosition = boardState.GetTileAt(pieceMoves.Key.X, pieceMoves.Key.Y).CurrentPiece;
            return ScanningPieces.Contains(pieceAtBoardPosition);
        }

        private static List<Position> GetScanningPiecesMoves(
            IDictionary<Position, List<Position>> moves, BoardState boardState)
        {
            var result = new List<Position>();
            foreach (var keyVal in moves)
                if (PieceIsScanner(keyVal, boardState))
                    result.Add(keyVal.Key);
            return result;
        }

        private static bool DirectionIsInPieceMoveRepetitious(Direction direction, Position piecePosition,
            BoardState boardState)
        {
            var pieceType = boardState.GetTileAt(piecePosition).CurrentPiece;
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
            return directionExists &&
                   DirectionIsInPieceMoveRepetitious(potentialDirectionToKing, piecePosition, boardState);
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

                if (index == 0)
                {
                    index++;
                    continue;
                }

                var pieceType = tile.CurrentPiece;
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
                if (kingIsNextPiece) return (true, pinnedPiece, index);

                if (potentialPinHasBeenFound) return (false, new Position(), index); // piece has blocked pin
            }

            return (false, new Position(), 0);
        }

        public void FilterMoves(IBoardInfo boardInfo, BoardState boardState)
        {
            var kingPosition = boardInfo.KingPosition;
            if (kingPosition == new Position(8, 8))
                return;
            var kingColour = boardState.GetTileAt(kingPosition).CurrentPiece.Colour();
            var enemyScanningMoves = GetScanningPiecesMoves(boardInfo.EnemyMoves, boardState);
            for (var index = 0; index < enemyScanningMoves.Count; index++)
            {
                var enemyScanningMove = enemyScanningMoves[index];
                var pieceCanMoveToKing = PieceCanMoveToKing(enemyScanningMove, kingPosition, boardState);
                if (pieceCanMoveToKing)
                {
                    // avoid allocations here somehow 
                    var kingThreatMoves = ScanCache.ScanInclusiveTo(enemyScanningMove, kingPosition).ToList();
                    var (pinExists, pinnedPiecePosition, pinnedPieceIndex) =
                        PinExists(kingThreatMoves, boardState, kingColour, kingPosition);
                    if (pinExists)
                    {
                        kingThreatMoves.RemoveAt(pinnedPieceIndex);
                        boardInfo.TurnMoves[pinnedPiecePosition] = boardInfo.TurnMoves[pinnedPiecePosition]
                            .Intersect(kingThreatMoves).ToList();
                    }
                }
            }
        }
    }
}