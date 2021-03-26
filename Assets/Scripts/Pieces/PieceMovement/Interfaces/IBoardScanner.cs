using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBoardScanner
{
    IEnumerable<IBoardPosition> ScanIn(Direction direction, IBoardPosition fromPosition);
}
