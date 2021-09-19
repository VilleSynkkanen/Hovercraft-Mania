using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TimeTrialController : MonoBehaviour
{
    public static TimeTrialController instance { get; private set; }

    [SerializeField] int numberOfSectors;
    [SerializeField] string trackName;

    public float[] sectorTimes { get; private set; }
    public float[] bestSectorTimes { get; private set; }
    public float[] bestLapSectorTimes { get; private set; }
    public float currentTime { get; private set; }
    public float currentSectorTime { get; private set; }
    public float bestTime { get; private set; }
    public int currentSector { get; private set; }
    public bool onLap { get; private set; }
    public bool validLap { get; private set; }

    bool bestSectorsArrayGenerated;

    string username;
    LapData data;

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
        onLap = false;
        validLap = false;
        bestSectorsArrayGenerated = false;
        username = "Developer";         // PLACEHOLDER

        if(LoadLapInfo())
            TimeTrialHud.instance.UpdateBestLap();
    }

    void Update()
    {
        if(onLap)
        {
            currentSectorTime += Time.deltaTime;
            currentTime += Time.deltaTime;
        }
    }

    public void StartSector(int num)
    {  
        if(num == 0)
        {
            if(onLap)
            {
                // Check that current sector is the last one, otherwise lap is invalid
                if(currentSector + 1 == numberOfSectors && validLap)
                {
                    sectorTimes[sectorTimes.Length - 1] = currentSectorTime;

                    // Compare times and save them
                    if (bestSectorsArrayGenerated)
                    {
                        // Existing times

                        if(currentTime < bestTime)
                        {
                            // Save best time and best lap sector times
                            bestTime = currentTime;
                            for (int i = 0; i < numberOfSectors; i++)
                            {
                                bestLapSectorTimes[i] = sectorTimes[i];
                            }
                        }

                        for (int i = 0; i < numberOfSectors; i++)
                        {
                            if (sectorTimes[i] < bestSectorTimes[i])
                            {
                                bestSectorTimes[i] = sectorTimes[i];
                            }
                        }
                    }
                    else
                    {
                        // No existing times

                        bestSectorTimes = new float[numberOfSectors];
                        bestLapSectorTimes = new float[numberOfSectors];
                        bestSectorsArrayGenerated = true;

                        for (int i = 0; i < numberOfSectors; i++)
                        {
                            bestSectorTimes[i] = sectorTimes[i];
                            bestLapSectorTimes[i] = sectorTimes[i];
                        }

                        bestTime = currentTime;
                    }

                    TimeTrialHud.instance.UpdateBestLap();
                    SaveLapInfo();
                }
                else
                {
                    // Invalid lap
                    validLap = false;
                }
            }

            // Start lap
            onLap = true;
            validLap = true;
            sectorTimes = new float[numberOfSectors];
            currentSector = 0;
            currentTime = 0;
            currentSectorTime = 0;

            TimeTrialHud.instance.UpdateSectors();
        }
        else
        {
            if (onLap)
            {
                // Check that current sector is the previous one, otherwise lap is invalid
                if (currentSector == num - 1 && validLap)
                {
                    sectorTimes[num - 1] = currentSectorTime;
                    currentSectorTime = 0;
                    currentSector = num;

                    TimeTrialHud.instance.UpdateSectors();
                }
                else
                {
                    // Invalid lap
                    validLap = false;
                }
            }
        }
    }

    public void InvalidateLap()
    {
        if (onLap)
            validLap = false;
    }

    bool LoadLapInfo()
    {
        string fileName = "LapData_" + username + "_" + trackName;
        if(File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
            data = JsonUtility.FromJson<LapData>(json);
            bestTime = data.bestTime;

            bestSectorTimes = new float[data.bestSectorTimes.Length];
            for (int i = 0; i < bestSectorTimes.Length; i++)
            {
                bestSectorTimes[i] = data.bestSectorTimes[i];
            }

            bestLapSectorTimes = new float[data.bestLapSectorTimes.Length];
            for (int i = 0; i < bestLapSectorTimes.Length; i++)
            {
                bestLapSectorTimes[i] = data.bestLapSectorTimes[i];
            }

            return true;
        }

        return false;
    }

    void SaveLapInfo()
    {
        data = new LapData();
        data.username = username;
        data.trackName = trackName;
        data.bestTime = bestTime;
        data.bestSectorTimes = bestSectorTimes;
        data.bestLapSectorTimes = bestLapSectorTimes;

        string json = JsonUtility.ToJson(data);
        string fileName = "LapData_" + data.username + "_" + data.trackName;
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }
}
