using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPad : Pad
{
    [SerializeField] float slowForceMultiplier;
    
    void OnTriggerEnter(Collider other)
    {
        if(!onCooldown)
        {
            HovercraftMovement movement = other.GetComponent<HovercraftMovement>();
            if (movement)
            {
                movement.rb.AddForce(-movement.rb.velocity * slowForceMultiplier);
            }
        }
    }
}
