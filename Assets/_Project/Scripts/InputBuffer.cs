using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    private List<InputBufferAction> _inputBuffer = new List<InputBufferAction>();
    private bool _actionAllowed = true;


    void Update()
    {
        CheckInput();
        if (_actionAllowed)
        {
            TryBufferedAction();
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _inputBuffer.Add(new InputBufferAction(InputBufferAction.InputAction.Jump, Time.time));
        }
    }

    private void TryBufferedAction()
    {
        if (_inputBuffer.Count > 0)
        {
            foreach (InputBufferAction ai in _inputBuffer.ToArray())
            {
                _inputBuffer.Remove(ai);
                if (ai.CheckIfValid())
                {
                    DoAction(ai);
                    break;
                }
            }
        }
    }

    private void DoAction(InputBufferAction ai)
    {
        Debug.Log("s");
        //_actionAllowed = false;
    }
}
