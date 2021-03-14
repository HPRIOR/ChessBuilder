using System;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Game : MonoBehaviour
{
    private IBoardState _boardState;
    private IPieceSpawner _pieceInjector;

    [Inject]
    public void Construct(IBoardState boardState, IPieceSpawner pieceInjector)
    {
        _boardState = boardState;
        _pieceInjector = pieceInjector;
    }

    public void Start()
    {
        _pieceInjector.CreatePieceOf(PieceType.BlackKing, new BoardPosition(0, 0));
        _pieceInjector.CreatePieceOf(PieceType.WhiteKing, new BoardPosition(7, 7));
    }
}
