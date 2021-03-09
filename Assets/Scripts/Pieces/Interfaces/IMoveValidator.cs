﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IMoveValidator
{
    bool ValidateMove(GameObject gameObject, IBoardPosition destination);
}
