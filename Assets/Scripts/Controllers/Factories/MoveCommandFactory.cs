using Controllers.Commands;
using Models.State.Interfaces;

namespace Controllers.Factories
{
    public class MoveCommandFactory
    {
        private readonly MoveCommand.Factory _movePieceCommandFactory;

        public MoveCommandFactory(MoveCommand.Factory movePieceCommandFactory)
        {
            _movePieceCommandFactory = movePieceCommandFactory;
        }

        public MoveCommand Create(IBoardPosition from, IBoardPosition destination)
        {
            return _movePieceCommandFactory.Create(from, destination);
        }
    }
}