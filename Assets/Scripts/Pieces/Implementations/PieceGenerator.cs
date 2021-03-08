using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PieceGenerator : IPieceGanerator
{
    GameObject _pieceParent;
    public PieceGenerator()
    {
        _pieceParent = new GameObject("Pieces");
    }
    IDictionary<PieceType, string> _piecePrefabLocations = new Dictionary<PieceType, string>()
    {
        { PieceType.BlackKing, "Assets/Prefabs/Pieces/BlackKing.prefab"},
        { PieceType.BlackQueen, "Assets/Prefabs/Pieces/BlackQueen.prefab"},
        { PieceType.BlackRook, "Assets/Prefabs/Pieces/BlackRook.prefab"},
        { PieceType.BlackBishop, "Assets/Prefabs/Pieces/BlackBishop.prefab"},
        { PieceType.BlackKnight, "Assets/Prefabs/Pieces/BlackKnight.prefab"},
        { PieceType.BlackPawn, "Assets/Prefabs/Pieces/BlackPawn.prefab"},
        { PieceType.WhiteKing, "Assets/Prefabs/Pieces/WhiteKing.prefab"},
        { PieceType.WhiteQueen, "Assets/Prefabs/Pieces/WhiteQueen.prefab"},
        { PieceType.WhiteRook, "Assets/Prefabs/Pieces/WhiteRook.prefab"},
        { PieceType.WhiteBishop, "Assets/Prefabs/Pieces/WhiteBishop.prefab"},
        { PieceType.WhiteKnight, "Assets/Prefabs/Pieces/WhiteKnight.prefab"},
        { PieceType.WhitePawn, "Assets/Prefabs/Pieces/WhitePawn.prefab"},

    };
    public void GeneratorPiece(ITile tile)
    {
        if (tile.CurrentPiece == PieceType.None) return;
        var prefab = AssetDatabase.LoadAssetAtPath(_piecePrefabLocations[tile.CurrentPiece], typeof(GameObject));
        var pieceGameObject = (GameObject)GameObject.Instantiate(prefab);

        pieceGameObject.transform.position = tile.BoardPosition.Position;
        pieceGameObject.transform.parent = _pieceParent.transform;

        pieceGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        // generate piece info TODO replace me with IPiece
        pieceGameObject.AddComponent<Piece>();
        var pieceComponent = pieceGameObject.GetComponent<Piece>();
        pieceComponent.PieceTile = tile;
        
    }

   
}
