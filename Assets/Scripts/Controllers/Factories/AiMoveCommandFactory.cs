using Controllers.Commands;

namespace Controllers.Factories
{
    public class AiMoveCommandFactory
    {
        private readonly AiMoveCommand.Factory _aiMoveCommandFactory;

        public AiMoveCommandFactory(AiMoveCommand.Factory aiMoveCommandFactory)
        {
            _aiMoveCommandFactory = aiMoveCommandFactory;
        }

        public AiMoveCommand Create() =>
            _aiMoveCommandFactory.Create();
    }
}