using UnityEngine;

public class InputTester : MonoBehaviour
{
    private CommandInvoker _commandInvoker;

    // Start is called before the first frame update
    private void Start()
    {
        _commandInvoker = GetComponent<CommandInvoker>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) _commandInvoker.RollBackCommand();
    }
}