using System;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public class DirectionException : Exception
    {
        public DirectionException(string message) : base(message)
        {
        }
    }
}