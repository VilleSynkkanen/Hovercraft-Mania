using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    [SerializeField] GameObject[] selectedButtons;
    [SerializeField] GameObject[] selectedButtonsMainMenu;
    [SerializeField] ChosenTrack track;
    [SerializeField] EventSystem eventSystem;

    int activeMenu;

    void Start()
    {
        activeMenu = 0;
        Cursor.visible = true;
    }
    
    public void ActivateMenu(int i)
    {
        int previousMenu = activeMenu;
        menus[activeMenu].SetActive(false);
        activeMenu = i;
        menus[i].SetActive(true);
        if(i != 0)
            eventSystem.SetSelectedGameObject(selectedButtons[i]);
        else
            eventSystem.SetSelectedGameObject(selectedButtonsMainMenu[previousMenu - 1]);
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