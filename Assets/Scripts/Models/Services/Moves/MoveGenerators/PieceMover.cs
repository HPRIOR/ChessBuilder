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
            ref var destinationTile = ref boardState.GetTileAt(destination);
            ref var fromTile = ref boardState.GetTileAt(from);

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

        }

    

        private static bool TileContainsPieceAt(Position position, BoardState boardState) =>
            boardState.Board[position.X][position.Y].CurrentPiece.Type != PieceType.NullPiece;
    }
}