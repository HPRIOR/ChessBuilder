using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    public IBoardPosition BoardPosition { get; set; }
    public PieceColour PieceColour { get; }
    public PieceType PieceType { get; set; }

    public IPossibleMoveGenerator possibleMoveGenerator;

    private static IDictionary<PieceType, string> _spriteAssetMap = PieceSpriteAssetManager.PieceSpriteMap; 
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(_spriteAssetMap[PieceType]);
        gameObject.transform.position = BoardPosition.Position;
    }

    [Inject]
    public void Construct(PieceType pieceType, IBoardPosition boardPosition, IPossibleMoveGeneratorFactory possibleMoveGeneratorFactory)
    {
        PieceType = pieceType;
        BoardPosition = boardPosition;
        possibleMoveGenerator = possibleMoveGeneratorFactory.GetPossibleMoveGenerator(PieceType);
    }
    
    public class Factory : PlaceholderFactory<PieceType, IBoardPosition, Piece> { }

}