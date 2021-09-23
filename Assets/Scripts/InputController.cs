using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public float throttle { get; private set; }
    public float brake { get; private set; }
    public float steering { get; private set; }

    public event UnityAction Reset = delegate { };
    public event UnityAction Pause = delegate { };

    public void OnThrottle(InputAction.CallbackContext context)
    {
        if (context.performed)
            throttle = context.ReadValue<float>();
        else if(context.canceled)
            throttle = 0;
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        if (context.performed)
            brake = context.ReadValue<float>();
        else if (context.canceled)
            brake = 0;
    }

    public void OnSteering(InputAction.CallbackContext context)
    {
        if (context.performed)
            steering = context.ReadValue<float>();
        else if (context.canceled)
            steering = 0;
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.started)
            Reset();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
            Pause();
    }
}
