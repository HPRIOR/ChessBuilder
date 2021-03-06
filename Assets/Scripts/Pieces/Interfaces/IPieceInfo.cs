using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPieceInfo  
{
    PieceType PieceType { get; set; }
    IBoardPosition boardPosition { get; set;} 

}
