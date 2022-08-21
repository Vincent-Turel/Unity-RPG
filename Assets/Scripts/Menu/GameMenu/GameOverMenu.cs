using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenu;
    public PlayerContol PlayerContol;
    public bool isShowing = false;
    private void Start()
    {
        gameOverMenu.SetActive(false);
    }

    public void GameOverShow()
    {
        isShowing = true;
        gameOverMenu.SetActive(true);
    }

    public void GameOverUnShow()
    {
        isShowing = false;
        gameOverMenu.SetActive(false);
    }

    public void Respawn()
    {
        PlayerContol.playerRespawn();
    }
}
