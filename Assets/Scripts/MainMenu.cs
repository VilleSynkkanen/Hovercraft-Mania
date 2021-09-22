using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    
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

    public void Play(string trackName)
    {
        SceneManager.LoadScene(trackName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}