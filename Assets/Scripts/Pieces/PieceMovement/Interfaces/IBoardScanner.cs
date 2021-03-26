using System.Collections.Generic;

public interface IBoardScanner
{
    IEnumerable<IBoardPosition> ScanIn(Direction direction, IBoardPosition fromPosition);
}