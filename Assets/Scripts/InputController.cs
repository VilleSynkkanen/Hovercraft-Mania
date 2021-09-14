using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float throttle { get; private set; }
    public float brake { get; private set; }
    public float steering { get; private set; }

    void Update()
    {
        throttle = Input.GetAxis("Throttle");
        brake = Input.GetAxis("Brake");
        steering = Input.GetAxis("Steering");
    }
}
