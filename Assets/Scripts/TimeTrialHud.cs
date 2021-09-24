using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeTrialHud : MonoBehaviour
{
    public static TimeTrialHud instance { get; private set; }

    [SerializeField] TextMeshProUGUI currentLap;
    [SerializeField] TextMeshProUGUI bestLap;
    [SerializeField] TextMeshProUGUI[] currentSectorTimes;
    [SerializeField] Image[] currentSectorTimesImages;
    [SerializeField] Color betterColor;
    [SerializeField] Color worseColor;
    [SerializeField] Color defaultColor;
    [SerializeField] TextMeshProUGUI[] bestSectorTimes;
    [SerializeField] Image[] bestSectorTimesImages;

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

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if(TimeTrialController.instance.onLap)
        {
            if(TimeTrialController.instance.validLap)
            {
                currentLap.text = "Current time:\n" + (Mathf.Round(1000f * TimeTrialController.instance.currentTime) / 1000f).ToString() + " s";
            }
            else
            {
                currentLap.text = "Current lap\ninvalid";
            }
        }
        else
        {
            currentLap.text = "";
        }
    }
    
    public void UpdateBestLap()
    {
        bestLap.text = "Best time:\n" + (Mathf.Round(1000f * TimeTrialController.instance.bestTime) / 1000f).ToString() + " s";
        for (int i = 0; i < TimeTrialController.instance.bestSectorTimes.Length; i++)
        {
            float time = TimeTrialController.instance.bestSectorTimes[i];
            if (time != 0)
            {
                bestSectorTimes[i].text = "S" + (i + 1).ToString() + ": " + (Mathf.Round(1000f * time) / 1000f).ToString() + " s";
                bestSectorTimesImages[i].color = Color.white;
            }
        }
    }

    public void UpdateSectors()
    {
        for (int i = 0; i < TimeTrialController.instance.sectorTimes.Length; i++)
        {
            float time = TimeTrialController.instance.sectorTimes[i];
            if (time != 0)
            {
                currentSectorTimes[i].text = "S" + (i + 1).ToString() + ": " + (Mathf.Round(1000f * time) / 1000f).ToString() + " s";
                if(!TimeTrialController.instance.bestSectorsArrayGenerated || time < TimeTrialController.instance.bestSectorTimes[i])
                {
                    currentSectorTimesImages[i].color = betterColor;
                }
                else
                {
                    currentSectorTimesImages[i].color = worseColor;
                }
            }    
        }
    }

    public void ResetSectors()
    {
        for (int i = 0; i < TimeTrialController.instance.sectorTimes.Length; i++)
        {
            currentSectorTimes[i].text = "";
            currentSectorTimesImages[i].color = defaultColor;
        }
    }
}
