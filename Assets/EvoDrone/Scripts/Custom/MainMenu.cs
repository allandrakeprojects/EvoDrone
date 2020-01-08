using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private LevelManager levelManager;
    public Text Coin;
    public Text Best;
    public Button ClaimCoinButton;
    public Text ClaimWaitMinutes;
    bool detectStartCountdown = false;

    void Start()
    {
        ShowCoin();
        ShowBest();
        CheckCoin();
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

    public void ShowBest()
    {
        try
        {
            if (PlayerPrefs.GetInt("highscore", 0) == 0)
            {
                Best.text = "BEST: 0";
            }
            else
            {
                int highscore = PlayerPrefs.GetInt("highscore");
                Best.text = "BEST: " + highscore;
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    public void ShowCoin()
    {
        try
        {
            if (PlayerPrefs.GetInt("coin", -1) == -1)
            {
                Coin.text = "1k";
                PlayerPrefs.SetInt("coin", 1000);
                PlayerPrefs.Save();

                int coin = PlayerPrefs.GetInt("coin");
                Coin.text = FormatCoin(coin);
            }
            else
            {
                int coin = PlayerPrefs.GetInt("coin");
                Coin.text = FormatCoin(coin);
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    public void AddCoin(int num)
    {
        int coin = PlayerPrefs.GetInt("coin");
        coin += num;
        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.Save();
        Coin.text = FormatCoin(coin);
    }

    static string FormatCoin(int num)
    {
        if (num >= 100000)
        {
            return FormatCoin(num / 1000) + "K";
        }
        else if (num >= 1000)
        {
            return (num / 1000D).ToString("0.#") + "K";
        }

        return num.ToString("#,0");
    }

    public void ClaimCoin()
    {
        ClaimCoinButton.interactable = false;
        AddCoin(100);
        PlayerPrefs.SetString("waitMinutes", System.DateTime.Now.AddMinutes(5).ToString());
        PlayerPrefs.Save();
        detectStartCountdown = true;
    }

    public void CheckCoin()
    {
        DateTime datevalue1 = DateTime.Parse(PlayerPrefs.GetString("waitMinutes"));
        DateTime datevalue2 = DateTime.Now;
        TimeSpan timeDifference = datevalue1 - datevalue2;
        string time = new DateTime(timeDifference.Ticks).ToString("mm:ss");
        ClaimWaitMinutes.text = time;

        if (int.Parse(time.Replace(":", "")) <= 0000)
        {
            ClaimCoinButton.interactable = true;
            detectStartCountdown = false;
            ClaimWaitMinutes.text = "Collect";
        }
        else
        {
            detectStartCountdown = true;
        }
    }

    void Update()
    {
        try
        {
            if (detectStartCountdown)
            {
                DateTime datevalue1 = DateTime.Parse(PlayerPrefs.GetString("waitMinutes"));
                DateTime datevalue2 = DateTime.Now;
                TimeSpan timeDifference = datevalue1 - datevalue2;
                string time = new DateTime(timeDifference.Ticks).ToString("mm:ss");
                ClaimWaitMinutes.text = time;

                if (int.Parse(time.Replace(":", "")) <= 0000)
                {
                    ClaimCoinButton.interactable = true;
                    detectStartCountdown = false;
                    ClaimWaitMinutes.text = "Collect";
                }
                else
                {
                    ClaimCoinButton.interactable = false;
                }
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }
}
