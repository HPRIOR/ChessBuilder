using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    public IBoardPosition BoardPosition { get; set; }
    public IPieceInfo PieceInfo { get; private set; }
    public IPossibleMoveGenerator possibleMoveGenerator;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(PieceInfo.SpriteAsset);
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces").transform;
        gameObject.transform.position = BoardPosition.Vector;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves() => possibleMoveGenerator.GetPossiblePieceMoves(gameObject);

    [Inject]
    public void Construct(IPieceInfo pieceInfo, IBoardPosition boardPosition, IPieceMoveGeneratorFactory pieceMoveGeneratorFactory)
    {
        PieceInfo = pieceInfo;
        BoardPosition = boardPosition;
        possibleMoveGenerator = pieceMoveGeneratorFactory.GetPossibleMoveGenerator(PieceInfo.PieceType);
    }
    
    public class Factory : PlaceholderFactory<IPieceInfo, IBoardPosition, Piece> { }

}