using System;
using Controllers.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Controllers.Commands
{
    public class BuildCommand : ICommand
    {
        public BuildCommand(Position at, Piece piece)
        {
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool IsValid() => throw new NotImplementedException();
    }
}