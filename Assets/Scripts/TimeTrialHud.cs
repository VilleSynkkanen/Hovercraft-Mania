using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTrialHud : MonoBehaviour
{
    public static TimeTrialHud instance { get; private set; }

    [SerializeField] TextMeshProUGUI currentLap;
    [SerializeField] TextMeshProUGUI bestLap;
    [SerializeField] TextMeshProUGUI currentSectorTimes;
    [SerializeField] TextMeshProUGUI bestSectorTimes;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        if(TimeTrialController.instance.onLap)
        {
            if(TimeTrialController.instance.validLap)
            {
                currentLap.text = "Current lap: " + (Mathf.Round(1000f * TimeTrialController.instance.currentTime) / 1000f).ToString() + " s";
            }
            else
            {
                currentLap.text = "Current lap: invalid";
            }
        }
        else
        {
            currentLap.text = "";
            currentSectorTimes.text = "";
        }
    }
    
    public void UpdateBestLap()
    {
        bestLap.text = "Best lap: " + (Mathf.Round(1000f * TimeTrialController.instance.bestTime) / 1000f).ToString() + " s";
        string bstSectorTimes = "Best sectors: ";
        for (int i = 0; i < TimeTrialController.instance.bestSectorTimes.Length; i++)
        {
            float time = TimeTrialController.instance.bestSectorTimes[i];
            if (time != 0)
                bstSectorTimes += "Sector " + (i + 1).ToString() + ": " + (Mathf.Round(1000f * time) / 1000f).ToString() + " s ";
        }
        bestSectorTimes.text = bstSectorTimes;
    }

    public void UpdateSectors()
    {
        string currSectorTimes = "Sectors: ";
        for (int i = 0; i < TimeTrialController.instance.sectorTimes.Length; i++)
        {
            float time = TimeTrialController.instance.sectorTimes[i];
            if (time != 0)
                currSectorTimes += "Sector " + (i + 1).ToString() + ": " + (Mathf.Round(1000f * time) / 1000f).ToString() + " s "; ;
        }
        currentSectorTimes.text = currSectorTimes;
    }
}
