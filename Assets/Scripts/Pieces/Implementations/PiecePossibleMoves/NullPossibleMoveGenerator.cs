using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NullPossibleMoveGenerator : IPossibleMoveGenerator
{
    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece) => new List<IBoardPosition>();
}

