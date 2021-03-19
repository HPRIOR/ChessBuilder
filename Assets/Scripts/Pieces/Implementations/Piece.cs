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
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(PieceInfo.SpriteAsset);
        gameObject.transform.position = BoardPosition.Position;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves() => possibleMoveGenerator.GetPossiblePieceMoves(gameObject);

    [Inject]
    public void Construct(IPieceInfo pieceInfo, IBoardPosition boardPosition, IPossibleMoveGeneratorFactory possibleMoveGeneratorFactory)
    {
        PieceInfo = pieceInfo;
        BoardPosition = boardPosition;
        possibleMoveGenerator = possibleMoveGeneratorFactory.GetPossibleMoveGenerator(PieceInfo.PieceType);
    }
    
    public class Factory : PlaceholderFactory<IPieceInfo, IBoardPosition, Piece> { }

}