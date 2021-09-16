using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : Pad
{
    [SerializeField] float boostForce;

    void OnTriggerEnter(Collider other)
    {
        if (!onCooldown)
        {
            HovercraftMovement movement = other.GetComponent<HovercraftMovement>();
            if (movement)
            {
                movement.rb.AddForce(movement.transform.forward * boostForce);
            }
        }
    }
}
