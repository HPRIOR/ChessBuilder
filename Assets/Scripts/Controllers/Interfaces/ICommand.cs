namespace Controllers.Interfaces
{
    public interface ICommand
    {
        void Execute();

        void Undo();

        bool IsValid(bool peak);
    }
}