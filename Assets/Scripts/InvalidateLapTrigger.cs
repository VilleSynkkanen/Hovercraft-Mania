using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidateLapTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<HovercraftMovement>())
        {
            TimeTrialController.instance.InvalidateLap();
        }
    }
}
