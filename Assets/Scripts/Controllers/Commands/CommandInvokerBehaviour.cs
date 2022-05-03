using System.Collections;
using Controllers.Interfaces;
using UnityEngine;
using Zenject;

namespace Controllers.Commands
{
    public class CommandInvokerBehaviour : MonoBehaviour
    {
        private ICommandInvoker _commandInvoker;

        /*
         * Coroutine needed to ensure TryExecuteCommand invoked once per frame.
         * Invoking it more than once per frame causes issues with the renderer, which destroys existing
         * game objects on a rerender
         */
        private void Update()
        {
            StartCoroutine(TryExecuteCommand());
            if (Input.GetKeyDown(KeyCode.LeftArrow)) _commandInvoker.RollBackCommand();
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
    }
}