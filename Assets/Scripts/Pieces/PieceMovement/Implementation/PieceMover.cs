﻿using UnityEngine;

/*
 * changes: Move signature change to Func<IBoardState, IBoardPos, IBoardPos, ITile[]> 
 * Don't update pieces on move but produce new board with moved piece types
 * remove undo move from this class as state will just be restored by refering to previous state
 * remove Boardstate from constructor, and pass as argument so it can be applied to arbitrary boards
 * 
 */

public class PieceMover : IPieceMover
{
    private static IBoardState _boardState;

    public PieceMover(IBoardState boardState)
    {
        _boardState = boardState;
    }

    public void Move(GameObject piece, IBoardPosition toDestination)
    {
        // move game object
        UpdatePiecesOnMove(piece, toDestination);
        UpdateBoardOnMove(piece, toDestination);
    }

    private void UpdateBoardOnMove(GameObject piece, IBoardPosition destination)
    {
        var vacatedTile = _boardState.GetTileAt(piece.GetComponent<Piece>().BoardPosition);
        var destinationTile = _boardState.GetTileAt(destination);

        vacatedTile.CurrentPiece = null;
        destinationTile.CurrentPiece = piece;
    }

    // must be called before UpdateBoardOnMove
    private void UpdatePiecesOnMove(GameObject piece, IBoardPosition destination)
    {
        piece.transform.position = destination.Vector;
        piece.GetComponent<Piece>().BoardPosition = destination;
        var displacedPiece = _boardState.GetTileAt(destination).CurrentPiece;
        if (displacedPiece != null & displacedPiece?.gameObject != piece.gameObject)
        {
            displacedPiece.gameObject.SetActive(false);
        }
    }

    public void UndoMove(IMoveData moveData)
    {
        UpdatePiecesOnUndo(moveData);
        UpdateBoardOnUndo(moveData);
    }

    private void UpdatePiecesOnUndo(IMoveData moveData)
    {
        // activate displaced piece
        moveData.DisplacedPiece?.SetActive(true);

        // activate moved piece and move back to original position
        moveData.MovedPiece.SetActive(true);
        moveData.MovedPiece.transform.position = moveData.InitialBoardPosition.Vector;
        moveData.MovedPieceComponent.BoardPosition = moveData.InitialBoardPosition;
    }

    private void UpdateBoardOnUndo(IMoveData moveData)
    {
        var vacatedTile = _boardState.GetTileAt(moveData.InitialBoardPosition);
        var destinationTile = _boardState.GetTileAt(moveData.DestinationBoardPosition);

        vacatedTile.CurrentPiece = moveData.MovedPiece;
        destinationTile.CurrentPiece = moveData.DisplacedPiece;
    }
}