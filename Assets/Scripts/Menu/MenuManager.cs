using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public InventoryUI InventoryUI;
    public UIPauseManager PauseMenuUI;
    public GameOverMenu GameOverMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenuUI.isShowing)
            {
                PauseMenuUI.PauseUnShow();
                unPause();
            }
            else if (InventoryUI.isShowing)
            {
                InventoryUI.InventoryUnshow();
                unPause();
            }
            else
            {
                PauseMenuUI.PauseShow();
                pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.I) && !PlayerContol.playerDead)
        {
            if (InventoryUI.isShowing)
            {
                InventoryUI.InventoryUnshow();
                unPause();
            }
            else if (!InventoryUI.isShowing)
            {
                InventoryUI.InventoryShow();
                pause();
            }
        }

        if (PlayerContol.playerDead)
        {
            InventoryUI.InventoryUnshow();
            unPause();
            GameOverMenu.GameOverShow();
        }
        else
        {
            GameOverMenu.GameOverUnShow();
        }
    }

    private void pause()
    {
        Time.timeScale = 0;
    }

    private void unPause()
    {
        Time.timeScale = 1;
    }
}
