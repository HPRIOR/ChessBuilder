using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    CommandInvoker _commandInvoker;
    private void Start()
    {
        _commandInvoker = GameObject.FindGameObjectWithTag("GameController").GetComponent<CommandInvoker>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
