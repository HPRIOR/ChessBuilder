using System.Collections;
using Controllers.Interfaces;
using Mirror;
using Zenject;

namespace Networking
{
    public class Player : NetworkBehaviour
    {
        private ICommandInvoker _commandInvoker;
        // Update is called once per frame
        
        [Command]
        private void Update()
        {
            StartCoroutine(TryExecuteCommand());
        }
        

        [Inject]
        public void Construct(ICommandInvoker commandInvoker)
        {
            _commandInvoker = commandInvoker;
        }

        private IEnumerator TryExecuteCommand()
        {
            _commandInvoker.ExecuteCommand();
            yield return null;
        }
        
        public class Factory : PlaceholderFactory<Player>
        {
        }
    }
}