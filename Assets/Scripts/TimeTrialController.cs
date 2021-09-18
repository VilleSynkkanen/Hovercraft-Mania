using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialController : MonoBehaviour
{
    public static TimeTrialController instance { get; private set; }

    [SerializeField] int numberOfSectors;

    public float[] sectorTimes; //{ get; private set; }
    public float[] bestSectorTimes; //{ get; private set; }
    public float[] bestLapSectorTimes; //{ get; private set; }
    public float currentTime; //{ get; private set; }
    public float currentSectorTime; //{ get; private set; }
    public float bestTime; //{ get; private set; }
    public int currentSector; //{ get; private set; }
    public bool onLap; //{ get; private set; }
    public bool validLap; //{ get; private set; }

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
                    if (bestSectorTimes.Length > 0)
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

                        for (int i = 0; i < numberOfSectors; i++)
                        {
                            bestSectorTimes[i] = sectorTimes[i];
                            bestLapSectorTimes[i] = sectorTimes[i];
                        }

                        bestTime = currentTime;
                    }

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
                }
                else
                {
                    // Invalid lap
                    validLap = false;
                }
            }
        }
    }
}
