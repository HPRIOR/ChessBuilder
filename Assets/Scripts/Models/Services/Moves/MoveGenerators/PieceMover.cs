using System;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Moves.MoveGenerators
{
    public class PieceMover : IPieceMover
    {
        public void ModifyBoardState(BoardState boardState, Position from,
            Position destination)
        {
            ModifyActivePieces(boardState, from, destination);

            // modify board state
            var destinationTile = boardState.Board[destination.X, destination.Y];
            var fromTile = boardState.Board[from.X, from.Y];

            // swap pieces
            destinationTile.CurrentPiece = fromTile.CurrentPiece;
            fromTile.CurrentPiece = new Piece(PieceType.NullPiece);

            // return nothing 
        }

        private static void ModifyActivePieces(BoardState boardState, Position from, Position destination)
        {
            // modify active pieces 
            boardState.ActivePieces.Remove(from);
            boardState.ActivePieces.Add(destination);

            var currentPiece = boardState.Board[from.X, from.Y].CurrentPiece.Type;

            if (TileContainsPieceAt(destination, boardState))
                RemovePosFromActivePieceOfColour(destination, currentPiece.Colour().NextTurn(), boardState);

            if (currentPiece != PieceType.NullPiece)
            {
                var currentMoveColour = currentPiece.Colour();
                switch (currentMoveColour)
                {
                    case PieceColour.Black:
                        boardState.ActiveBlackPieces.Remove(from);
                        boardState.ActiveBlackPieces.Add(destination);

                        break;
                    case PieceColour.White:
                        boardState.ActiveWhitePieces.Remove(from);
                        boardState.ActiveWhitePieces.Add(destination);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void RemovePosFromActivePieceOfColour(Position position, PieceColour takenPieceColour,
            BoardState boardState)
        {
            if (takenPieceColour == PieceColour.Black)
                boardState.ActiveBlackPieces.Remove(position);
            else
                boardState.ActiveWhitePieces.Remove(position);
        }

        private static bool TileContainsPieceAt(Position position, BoardState boardState) =>
            boardState.Board[position.X, position.Y].CurrentPiece.Type != PieceType.NullPiece;
    }
}