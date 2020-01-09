using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadMainMenuAfterDelay()
    {
        Invoke("MainMenu", 1.0f);
    }

    public void LoadGameAfterDelay()
    {
        Invoke("Game", 1.0f);
    }

    public void LoadQuitAfterDelay()
    {
        Invoke("Quit", 1.0f);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void Game()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
