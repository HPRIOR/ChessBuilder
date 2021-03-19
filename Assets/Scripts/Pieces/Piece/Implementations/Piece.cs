using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    public IBoardPosition BoardPosition { get; set; }
    public IPieceInfo Info { get; private set; }
    private IPieceMoveGenerator _possibleMoveGenerator;
    private SpriteRenderer _spriteRenderer;

    [Inject]
    public void Construct(
            IPieceInfo pieceInfo,
            IBoardPosition boardPosition,
            IPieceMoveGeneratorFactory pieceMoveGeneratorFactory
            )
    {
        Info = pieceInfo;
        BoardPosition = boardPosition;
        _possibleMoveGenerator = pieceMoveGeneratorFactory.GetPossibleMoveGenerator(Info.PieceType);
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Info.SpriteAsset);
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces")?.transform;
        gameObject.transform.position = BoardPosition.Vector;
    }

    public IEnumerable<IBoardPosition> GetPossibleMoves() => _possibleMoveGenerator.GetPossiblePieceMoves(gameObject);

    public class Factory : PlaceholderFactory<IPieceInfo, IBoardPosition, Piece> { }

}