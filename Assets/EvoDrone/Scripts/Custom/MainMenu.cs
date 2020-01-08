using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private LevelManager levelManager;

    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.Save();
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
