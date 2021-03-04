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
        { PieceType.DarkKing, "Assets/Prefabs/Pieces/DarkKing.prefab"},
        { PieceType.DarkQueen, "Assets/Prefabs/Pieces/DarkQueen.prefab"},
        { PieceType.DarkRook, "Assets/Prefabs/Pieces/DarkRook.prefab"},
        { PieceType.DarkBishop, "Assets/Prefabs/Pieces/DarkBishop.prefab"},
        { PieceType.DarkKnight, "Assets/Prefabs/Pieces/DarKnight.prefab"},
        { PieceType.DarkPawn, "Assets/Prefabs/Pieces/DarkPawn.prefab"},
        { PieceType.LightKing, "Assets/Prefabs/Pieces/LightKing.prefab"},
        { PieceType.LightQueen, "Assets/Prefabs/Pieces/LightQueen.prefab"},
        { PieceType.LightRook, "Assets/Prefabs/Pieces/LightRook.prefab"},
        { PieceType.LightBishop, "Assets/Prefabs/Pieces/LightBishop.prefab"},
        { PieceType.LightKnight, "Assets/Prefabs/Pieces/LightKnight.prefab"},
        { PieceType.LightPawn, "Assets/Prefabs/Pieces/LightPawn.prefab"},

    };
    public void GeneratorPiece(ITile tile)
    {
        if (tile.CurrentPiece == PieceType.None) return;
        var prefab = AssetDatabase.LoadAssetAtPath(_piecePrefabLocations[tile.CurrentPiece], typeof(GameObject));
        var piece = (GameObject)GameObject.Instantiate(prefab);
        piece.transform.position = tile.BoardPosition.Position;
        piece.transform.parent = _pieceParent.transform;
        piece.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

   
}
