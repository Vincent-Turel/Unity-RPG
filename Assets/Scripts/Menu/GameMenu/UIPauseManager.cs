using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
    
public class UIPauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool isShowing = false;
    private void Start()
    {
        PauseMenu.SetActive(false);
    }

    public void PauseShow()
    {
        isShowing = true;
        PauseMenu.SetActive(true);
    }

    public void PauseUnShow()
    {
        isShowing = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
