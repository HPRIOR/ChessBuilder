using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    public IBoardPosition BoardPosition { get; set; }
    public IPieceInfo Info { get; private set; }
    private SpriteRenderer _spriteRenderer;

    [Inject]
    public void Construct(
            IPieceInfo pieceInfo,
            IBoardPosition boardPosition
            )
    {
        Info = pieceInfo;
        BoardPosition = boardPosition;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Info.SpriteAsset);
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces")?.transform;
        gameObject.transform.position = BoardPosition.Vector;
    }


    public class Factory : PlaceholderFactory<IPieceInfo, IBoardPosition, Piece> { }

    public override string ToString() => $"{Info}\n{BoardPosition}\n";
}