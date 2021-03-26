using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardPositionTranslatorFactory : IPositionTranslatorFactory
{
    private readonly BoardPositionTranslator.Factory _boardPositionTranslatorFactory;

    public BoardPositionTranslatorFactory(BoardPositionTranslator.Factory boardPositionTranslatorFactory)
    {
        _boardPositionTranslatorFactory = boardPositionTranslatorFactory;
    }

    public IBoardPositionTranslator Create(PieceColour pieceColour) =>
        _boardPositionTranslatorFactory.Create(pieceColour);

}
