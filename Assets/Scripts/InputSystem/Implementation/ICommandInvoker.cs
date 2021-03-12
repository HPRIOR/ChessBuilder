using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ICommandInvoker
{
    void AddCommand(ICommand command);
    void RollBackCommand();
    void UndoCommand();
}
