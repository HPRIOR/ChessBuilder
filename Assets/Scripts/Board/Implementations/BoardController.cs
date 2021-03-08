using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class BoardController : MonoBehaviour
{
    public GameObject tilePrefab;
    IBoardGenerator _boardLogicGenerator;
    IPieceGanerator _pieceGenerator;
    public ITile[,] Board { get; private set; }
    public IDictionary<ITile, HashSet<ITile>> PossibleMoves { get; private set; }
   

    [Inject]
    private void Constructor(IBoardGenerator boardLogicGenerator, IPieceGanerator pieceGenerator)
    {
        _boardLogicGenerator = boardLogicGenerator;
        _pieceGenerator = pieceGenerator;
    }
    
    private void Start()
    {
        Board = _boardLogicGenerator.GenerateBoard();
        EvaluateBoardMoves();
       
    }

    public ITile GetTileAt(IBoardPosition boardPosition) =>
        Board[boardPosition.X, boardPosition.Y];
    

    public void EvaluateBoardMoves()
    {
        // update possible moves dict
    }

}
