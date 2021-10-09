using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.MoveGenerators
{
    public sealed class PieceMover : IPieceMover
    {
        public void ModifyBoardState(BoardState boardState, Position from,
            Position destination)
        {
            ref var destinationTile = ref boardState.GetTileAt(destination);
            ref var fromTile = ref boardState.GetTileAt(from);

            var isTakingMove = destinationTile.CurrentPiece != PieceType.NullPiece;

            ModifyActivePieces(boardState, from, destination, isTakingMove);

            // swap pieces
            destinationTile.CurrentPiece = fromTile.CurrentPiece;
            fromTile.CurrentPiece = PieceType.NullPiece;

            // return nothing 
        }

        private static void ModifyActivePieces(BoardState boardState, Position @from, Position destination,
            bool isTakingMove)
        {
            // modify active pieces 
            boardState.ActivePieces.Remove(from);
            if (!isTakingMove)
                boardState.ActivePieces.Add(destination);
        }
    }
}