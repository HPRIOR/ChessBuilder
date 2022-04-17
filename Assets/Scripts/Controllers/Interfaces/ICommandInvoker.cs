namespace Controllers.Interfaces
{
    public interface ICommandInvoker
    {
        void AddCommand(ICommand command);
        void ExecuteCommand();

        void RollBackCommand();

        void UndoCommand();
    }
}