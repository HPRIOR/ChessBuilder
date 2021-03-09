using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IMoveData
{
    IBoardPosition InitialBoardPosition { get; }
    IBoardPosition DestinationBoardPosition { get; }
    GameObject DisplacedPiece { get; }
    GameObject MovedPiece { get; }
    IPiece MovedPieceComponent { get; }

    
}
