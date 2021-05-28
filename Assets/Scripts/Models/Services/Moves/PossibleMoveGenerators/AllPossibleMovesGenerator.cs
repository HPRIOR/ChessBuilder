﻿using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;
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


        public IDictionary<BoardPosition, HashSet<BoardPosition>> GetPossibleMoves(BoardState boardState,
            PieceColour turn, BoardPosition previousMove)
        {
            var result = new Dictionary<BoardPosition, HashSet<BoardPosition>>();
            var board = boardState.Board;

            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                if (currentPiece.Type != PieceType.NullPiece && currentPiece.Colour == turn)
                {
                    var boardPos = tile.BoardPosition;
                    var possibleMoves = _possibleMoveFactory.GetPossibleMoveGenerator(currentPiece)
                        .GetPossiblePieceMoves(boardPos, boardState);

                    result.Add(boardPos, new HashSet<BoardPosition>(possibleMoves));
                }
            }

            return result;
        }

        private IEnumerable<BoardPosition> GetCheckedBoardPositions(BoardState boardState, BoardPosition previousMove)
        {
            var possibleMoves =
                _possibleMoveFactory.GetPossibleMoveGenerator(boardState.Board[previousMove.X, previousMove.Y]
                    .CurrentPiece);
            return new List<BoardPosition>();
        }
    }
}