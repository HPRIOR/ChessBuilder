using UnityEngine;

public interface IPieceMover 
{
    void Move(GameObject piece, IBoardPosition toDestination);
    void Undo(IMoveData moveData);

}