using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardController : MonoBehaviour
{
    public GameObject tilePrefab;
    IBoardGenerator _boardGenerator;
    IBoardRenderer _boardRenderer;
    IPieceGanerator _pieceGenerator;
    ITile[,] _board;

    [Inject]
    public void Constructor(IBoardRenderer boardRenderer, IBoardGenerator boardLogic, IPieceGanerator pieceGenerator)
    {
        _boardRenderer = boardRenderer;
        _boardGenerator = boardLogic;
        _pieceGenerator = pieceGenerator;
    }
    
    public void Start()
    {
        _board = _boardGenerator.GenerateBoard();
        _boardRenderer.RenderBoard(_board, tilePrefab);
        foreach (var tile in _board)
            _pieceGenerator.GeneratorPiece(tile);
    }

}
