using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    [SerializeField] GameObject[] selectedButtons;
    [SerializeField] ChosenTrack track;
    [SerializeField] EventSystem eventSystem;

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
        eventSystem.SetSelectedGameObject(selectedButtons[i]);
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