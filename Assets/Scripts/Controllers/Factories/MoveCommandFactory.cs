using Controllers.Commands;
using Models.State.Board;

namespace Controllers.Factories
{
    public class MoveCommandFactory
    {
        private readonly MoveCommand.Factory _movePieceCommandFactory;

        public MoveCommandFactory(MoveCommand.Factory movePieceCommandFactory)
        {
            _movePieceCommandFactory = movePieceCommandFactory;
        }

        public MoveCommand Create(BoardPosition from, BoardPosition destination) =>
            _movePieceCommandFactory.Create(from, destination);
    }
}