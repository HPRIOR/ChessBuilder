public interface ICommandInvoker
{
    void AddCommand(ICommand command);

    void RollBackCommand();

    void UndoCommand();
}