using System;
using UnityEngine;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public class DirectionException : Exception
    {
        public DirectionException(string message) : base(message)
        {
            Debug.Log(message);
        }
    }
}