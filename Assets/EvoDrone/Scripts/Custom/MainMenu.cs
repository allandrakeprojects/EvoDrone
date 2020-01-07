using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    public void PLAYGAME()
    {
        levelManager.LoadGameAfterDelay();
    }

    public void OpenMainMenu()
    {
        levelManager.LoadMainMenuAfterDelay();
    }

    public void Quit()
    {
        levelManager.LoadQuitAfterDelay();
    }
}
