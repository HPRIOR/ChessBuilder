using System;

namespace Models.Utils.ExtensionMethods.BoardPosExt
{
    public class DirectionException : Exception
    {
        public DirectionException(string message) : base(message)
        {
        }
    }
}