using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Changes: tile will contain reference to piece type not game object. Get possible moves will refer to a different
 * mechanism that is give through the piece type. e.g. some factory which produces GetPieceTypeMove class
 * board state passed as argument instead of through constructor, produces dictionary instead of being a member of the 
 * class
 */
public class PossibleBoardMovesGenerator : IPossibleBoardMovesGenerator
{

    public IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState)
    {
        throw new System.NotImplementedException();
    }
}