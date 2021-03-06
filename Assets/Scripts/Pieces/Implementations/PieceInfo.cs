using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInfo : MonoBehaviour, IPieceInfo
{
    public PieceType PieceType { get; set; }
    public IBoardPosition boardPosition { get; set;} 
}
