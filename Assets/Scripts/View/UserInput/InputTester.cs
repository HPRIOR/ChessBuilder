using Controllers.Interfaces;
using UnityEngine;
using Zenject;

namespace View.UserInput
{
    public class InputTester : MonoBehaviour
    {
        private ICommandInvoker _commandInvoker;

        [Inject]
        public void Construct(ICommandInvoker commandInvoker)
        {
            _commandInvoker = commandInvoker;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) _commandInvoker.RollBackCommand();
        }
    }
}