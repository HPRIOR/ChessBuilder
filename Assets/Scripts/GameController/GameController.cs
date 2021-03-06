using System.Collections;
using System.Linq;
using Zenject;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PieceColour Turn { get; private set; } = PieceColour.White;

    public void ChangeTurn() =>
        _ = Turn == PieceColour.White ? Turn = PieceColour.Black : PieceColour.White;
    
    
}
