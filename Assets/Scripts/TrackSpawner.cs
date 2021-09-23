using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    [SerializeField] ChosenTrack track;
    [SerializeField] GameObject[] tracks;

    void Start()
    {
        Instantiate(tracks[track.chosenTrack]);
        
    }
}
