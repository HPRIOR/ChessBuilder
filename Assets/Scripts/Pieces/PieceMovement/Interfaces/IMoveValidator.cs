using UnityEngine;

public interface IMoveValidator
{
    bool ValidateMove(GameObject piece, IBoardPosition destination);

    bool ValidateMove(IBoardPosition origin, IBoardPosition destination);
}