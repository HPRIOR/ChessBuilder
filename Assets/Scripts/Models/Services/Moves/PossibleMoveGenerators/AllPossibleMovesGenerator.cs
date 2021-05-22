using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Interfaces;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class AllPossibleMovesGenerator : IAllPossibleMovesGenerator
    {
        private readonly IPossibleMoveFactory _possibleMoveFactory;

        public AllPossibleMovesGenerator(IPossibleMoveFactory possibleMoveFactory)
        {
            _possibleMoveFactory = possibleMoveFactory;
        }

        // the logic is wrong here
        // Moves players possible moves are evaluated for whether they contain a checking move, however 
        // it should be checked that the opposite players moves contain a checking move
        // all moves need to be evaluated - if the opposing moves contain a check, then this would incur the IntersectOnCheck method
        // a better solution may be to flag a variable in the board state if one player moves the board into a checked state
        // this could be started as state in this class: IEnumerable<IBoardPosition> checkMoves
        // GeneratePossibleMove needs to take in the the moved piece of the previous turn so that this can be checked 
        public IDictionary<IBoardPosition, HashSet<IBoardPosition>> GetPossibleMoves(IBoardState boardState,
            PieceColour turn)
        {
            var result = new Dictionary<IBoardPosition, HashSet<IBoardPosition>>();
            var board = boardState.Board;

            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                if (currentPiece.Type != PieceType.NullPiece /*&& currentPiece.Colour == turn*/)
                {
                    var boardPos = tile.BoardPosition;
                    var possibleMoves = _possibleMoveFactory.GetPossibleMoveGenerator(currentPiece.Type)
                        .GetPossiblePieceMoves(boardPos, boardState);

                    result.Add(boardPos, new HashSet<IBoardPosition>(possibleMoves));
                }
            }

            return result;
        }


        private IDictionary<IBoardPosition, HashSet<IBoardPosition>> IntersectOnCheck(
            IDictionary<IBoardPosition, HashSet<IBoardPosition>> possibleMoves,
            ITile[,] board,
            IBoardPosition checkedKing, IBoardPosition checkingPiece)
        {
            var possibleNonKingMoves =
                new HashSet<IBoardPosition>(ScanPositionGenerator.GetPositionsBetween(checkedKing, checkingPiece));
            return possibleMoves.ToList().Select(t =>
            {
                var bp = t.Key;
                var set = t.Value;
                var currentPiece = board[bp.X, bp.Y].CurrentPiece;
                if (currentPiece.Type != PieceType.BlackKing || currentPiece.Type != PieceType.WhiteKing)
                    return (bp, set.Intersect(possibleNonKingMoves) as HashSet<IBoardPosition>);
                return (bp, set);
            }).ToDictionary(t => t.bp, t => t.Item2);
        }
    }
}