using UnityEngine;

public interface IPieceMover
{
    void Move(GameObject piece, IBoardPosition toDestination);

    void UndoMove(IMoveData moveData);
}