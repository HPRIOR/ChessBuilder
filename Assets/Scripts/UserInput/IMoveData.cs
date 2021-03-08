﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IMoveData
{
    ITile FromTile { get; }
    ITile ToTile { get;  }
    PieceType DisplacedPiece { get; }
    PieceType MovedPiece { get; }

}
