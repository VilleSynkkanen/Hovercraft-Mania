using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    [SerializeField] ChosenTrack track;

    int activeMenu;

    void Start()
    {
        activeMenu = 0;
    }
    
    public void ActivateMenu(int i)
    {
        menus[activeMenu].SetActive(false);
        activeMenu = i;
        menus[i].SetActive(true);
    }

    public void Play(int trackIndex)
    {
        track.chosenTrack = trackIndex;
        SceneManager.LoadScene("TimeTrial");
    }

    public void Quit()
    {
        Application.Quit();
    }
}