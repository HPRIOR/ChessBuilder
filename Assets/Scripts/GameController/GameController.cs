using System.Collections;
using System.Linq;
using Zenject;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PieceColour Turn { get; private set; } = PieceColour.White;
    // will need to reference the board controller do evaluate draw/win 
    // checkmate not check mate 
    public void EvaluateGame()
    {

    }
    public void ChangeTurn() =>
        _ = Turn == PieceColour.White ? Turn = PieceColour.Black : Turn = PieceColour.White;

    
}
