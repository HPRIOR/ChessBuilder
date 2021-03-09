using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour
{
    CommandInvoker _commandInvoker;
    // Start is called before the first frame update
    void Start()
    {
        _commandInvoker = GetComponent<CommandInvoker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) _commandInvoker.RollBackCommand();
    }
}
