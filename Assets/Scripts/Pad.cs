using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    [SerializeField] float cooldown;

    protected bool onCooldown;

    void Start()
    {
        onCooldown = false;
    }

    protected IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
