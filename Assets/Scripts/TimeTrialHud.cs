using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTrialHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentLap;
    [SerializeField] TextMeshProUGUI bestLap;
    [SerializeField] TextMeshProUGUI currentSectorTimes;
    [SerializeField] TextMeshProUGUI bestSectorTimes;

    void Update()
    {
        currentLap.text = "Current lap: " + (Mathf.Round(1000f * TimeTrialController.instance.currentTime) / 1000f).ToString(); 
        bestLap.text = "Best lap: " + (Mathf.Round(1000f * TimeTrialController.instance.bestTime) / 1000f).ToString();

        string currSectorTimes = "Sectors: ";
        for(int i = 0; i < TimeTrialController.instance.sectorTimes.Length; i++)
        {
            float time = TimeTrialController.instance.sectorTimes[i];
            if (time != 0)
                currSectorTimes += "Sector " + i.ToString() + ": "  + (Mathf.Round(1000f * time) / 1000f).ToString() + " ";
        }
        currentSectorTimes.text = currSectorTimes;

        string bstSectorTimes = "Best sectors: ";
        for (int i = 0; i < TimeTrialController.instance.bestSectorTimes.Length; i++)
        {
            float time = TimeTrialController.instance.bestSectorTimes[i];
            if (time != 0)
                bstSectorTimes += "Sector " + i.ToString() + ": " + (Mathf.Round(1000f * time) / 1000f).ToString() + " ";
        }
        bestSectorTimes.text = bstSectorTimes;
    }
}
