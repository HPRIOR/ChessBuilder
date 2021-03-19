using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    public IBoardPosition BoardPosition { get; set; }
    public PieceColour PieceColour { get; }
    public PieceType PieceType { get; set; }
    public IPieceInfo PieceInfo { get; private set; }
    public IPossibleMoveGenerator possibleMoveGenerator;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(PieceSpriteAssetManager.GetSpriteAsset(PieceType));
        gameObject.transform.position = BoardPosition.Position;
        Debug.Log(PieceColour);
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves() => possibleMoveGenerator.GetPossiblePieceMoves(gameObject);

    [Inject]
    public void Construct(PieceType pieceType, IPieceInfo pieceInfo, IBoardPosition boardPosition, IPossibleMoveGeneratorFactory possibleMoveGeneratorFactory)
    {
        PieceInfo = pieceInfo;
        PieceType = pieceType;
        BoardPosition = boardPosition;
        possibleMoveGenerator = possibleMoveGeneratorFactory.GetPossibleMoveGenerator(PieceType);
    }
    
    public class Factory : PlaceholderFactory<PieceType, IPieceInfo, IBoardPosition, Piece> { }

}