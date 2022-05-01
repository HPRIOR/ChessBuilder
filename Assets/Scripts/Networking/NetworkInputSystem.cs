using System.Collections;
using Controllers.Interfaces;
using Mirror;
using Zenject;

namespace Networking
{
    public class NetworkInputSystem: NetworkBehaviour
    {
        private ICommandInvoker _commandInvoker;
        
        [Inject]
        public void Construct(ICommandInvoker commandInvoker)
        {
            _commandInvoker = commandInvoker;
        }

        [Command]
        private void Update()
        {
            StartCoroutine(TryExecuteCommand());
        }

        private IEnumerator TryExecuteCommand()
        {
            _commandInvoker.ExecuteCommand();
           yield return null;
        }
    }
}