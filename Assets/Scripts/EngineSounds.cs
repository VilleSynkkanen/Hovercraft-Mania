using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSounds : MonoBehaviour
{
    Rigidbody rb;
    
    [SerializeField] AudioSource engine;
    [SerializeField] AudioSource brake;
    [SerializeField] float pitchMultiplier;
    [SerializeField] float basePitch;
    [SerializeField] float maxPitch;

    bool brakesOn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        brakesOn = false;
    }
    
    void Update()
    {
        engine.pitch = Mathf.Clamp(basePitch + rb.velocity.magnitude * pitchMultiplier, 0, maxPitch);
    }

    public void PlayBrakingSound(bool braking)
    {
        if(braking)
        {
            if(!brakesOn)
            {
                brakesOn = true;
                brake.Play();
            }
        }
        else
        {
            brakesOn = false;
        }
    }
}