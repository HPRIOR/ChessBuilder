using System;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Moves.MoveGenerators
{
    public sealed class PieceMover : IPieceMover
    {
        public void ModifyBoardState(BoardState boardState, Position from,
            Position destination)
        {
            
            ref var destinationTile = ref boardState.GetTileAt(destination);
            ref var fromTile = ref boardState.GetTileAt(from);

            var isTakingMove = destinationTile.CurrentPiece.Type != PieceType.NullPiece;
            
            ModifyActivePieces(boardState, from, destination, isTakingMove);

            // swap pieces
            destinationTile.CurrentPiece = fromTile.CurrentPiece;
            fromTile.CurrentPiece = new Piece(PieceType.NullPiece);

            // return nothing 
        }

        private static void ModifyActivePieces(BoardState boardState, Position @from, Position destination,
            bool isTakingMove)
        {
            // modify active pieces 
            boardState.ActivePieces.Remove(from);
            if(!isTakingMove)
                boardState.ActivePieces.Add(destination);

        }

    }
}