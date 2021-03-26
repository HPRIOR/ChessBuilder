using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPositionTranslator
{
    IBoardPosition GetRelativePosition(IBoardPosition originalPosition);
    ITile GetRelativeTileAt(IBoardPosition boardPosition);
}
