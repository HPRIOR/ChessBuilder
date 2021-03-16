﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IMoveValidator
{
    bool ValidateMove(GameObject piece, IBoardPosition destination);
    bool ValidateMove(IBoardPosition origin, IBoardPosition destination);
}
