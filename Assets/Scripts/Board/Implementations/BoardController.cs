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
    IBoardRenderer _boardRenderer;
    IPieceGanerator _pieceGenerator;
    public ITile[,] Board { get; private set; }
    public IDictionary<PieceType, IBoardPosition[]> PossibleMoves { get; private set; }

    [Inject]
    private void Constructor(IBoardRenderer boardRenderer, IBoardGenerator boardLogicGenerator, IPieceGanerator pieceGenerator)
    {
        _boardRenderer = boardRenderer;
        _boardLogicGenerator = boardLogicGenerator;
        _pieceGenerator = pieceGenerator;
    }
    
    private void Start()
    {
        Board = _boardLogicGenerator.GenerateBoard();
        //PossibleMoves = EvaluateBoardMoves();
        _boardRenderer.RenderBoard(Board, tilePrefab);
       
        foreach (var tile in Board)
            _pieceGenerator.GeneratorPiece(tile);
    }

    // will be called onces per successful move and creates a dictionary indicating all legal moves 
    public IDictionary<PieceType, IBoardPosition[]> EvaluateBoardMoves()
    {
        throw new NotImplementedException();
    }

    public ITile[,] UpdateBoardState(PieceType piece, IBoardPosition previousBoardPosition, IBoardPosition newBoardPosition)
    {
        throw new NotImplementedException();
    }



}
