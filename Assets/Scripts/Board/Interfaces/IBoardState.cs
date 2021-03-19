using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBoardState
{

    ITile GetTileAt(IBoardPosition boardPosition);
    ITile GetMirroredTileAt(IBoardPosition boardPosition);
}
