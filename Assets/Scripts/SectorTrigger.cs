using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorTrigger : MonoBehaviour
{
    [SerializeField] int sectorNumber;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HovercraftMovement>())
        {
            TimeTrialController.instance.StartSector(sectorNumber);
        }
    }
}
