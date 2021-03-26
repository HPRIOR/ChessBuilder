using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardPositionTranslatorFactory : IPositionTranslatorFactory
{
    private readonly PositionTranslator.Factory _boardPositionTranslatorFactory;

    public BoardPositionTranslatorFactory(PositionTranslator.Factory boardPositionTranslatorFactory)
    {
        _boardPositionTranslatorFactory = boardPositionTranslatorFactory;
    }

    public IPositionTranslator Create(PieceColour pieceColour) =>
        _boardPositionTranslatorFactory.Create(pieceColour);

}
