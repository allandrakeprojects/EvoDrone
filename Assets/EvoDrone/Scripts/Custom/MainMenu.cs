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

    public Text FirerateLvl;
    public Button FirerateButton;
    public Text FirerateCoin;

    public Text FirepowerLvl;
    public Button FirepowerButton;
    public Text FirepowerCoin;

    void Start()
    {
        ShowCoin();
        ShowBest();
        CheckCoin();
        Powerups();

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

    public void AddMinusCoin(int num, int type)
    {
        int coin = PlayerPrefs.GetInt("coin");
        if (type == 0)
        {
            coin += num;
        }
        else
        {
            coin -= num;
        }
        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.Save();
        Coin.text = FormatCoin(coin);
        
        checkPowerups();
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
        AddMinusCoin(100, 0);
        PlayerPrefs.SetString("waitMinutes", System.DateTime.Now.AddMinutes(5).ToString());
        PlayerPrefs.Save();
        detectStartCountdown = true;
    }

    public void CheckCoin()
    {
        try
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
        catch (Exception err)
        {
            // leave blank
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


    public void UpgradeFirerate()
    {
        int lvl = PlayerPrefs.GetInt("firerate_lvl");
        lvl += 1;
        PlayerPrefs.SetInt("firerate_lvl", lvl);
        PlayerPrefs.Save();
        
        AddMinusCoin( int.Parse(DecryptCoin(FirerateCoin.text)), 1);
        Powerups();
    }
    public void UpgradeFirepower()
    {
        int lvl = PlayerPrefs.GetInt("firepower_lvl");
        lvl += 1;
        PlayerPrefs.SetInt("firepower_lvl", lvl);
        PlayerPrefs.Save();

        AddMinusCoin(int.Parse(DecryptCoin(FirepowerCoin.text)), 1);
        Powerups();
    }




    public void Powerups()
    {
        firerate();
        firepower();
        checkPowerups();
    }

    public void firerate()
    {
        try
        {
            float DEFAULT_VAL = 3.3f;
            float LOOP_VAL = .2f;
            int DEFAULT_COIN = 500;
            int LOOP_COIN = 500;
            if (PlayerPrefs.GetInt("firerate_lvl", 1) == 1)
            {

                PlayerPrefs.SetInt("firerate_lvl", 1);
                PlayerPrefs.Save();
            }
            else
            {
                int lvl = PlayerPrefs.GetInt("firerate_lvl");
                float new_default_val = (lvl * LOOP_VAL) + DEFAULT_VAL;
                int new_default_coin = (lvl * LOOP_COIN) + DEFAULT_COIN;

                FirerateLvl.text = "FIRERATE [LV" + lvl + "]";
                FirerateCoin.text = FormatCoin(new_default_coin);
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    public void firepower()
    {
        try
        {
            int DEFAULT_VAL = 0;
            int LOOP_VAL = 1;
            int DEFAULT_COIN = 0;
            int LOOP_COIN = 7500;
            if (PlayerPrefs.GetInt("firepower_lvl", 1) == 1)
            {

                PlayerPrefs.SetInt("firepower_lvl", 1);
                PlayerPrefs.Save();
            }
            else
            {
                int lvl = PlayerPrefs.GetInt("firepower_lvl");
                if (lvl == 3)
                {
                    FirepowerLvl.text = "FIREPOWER [LV3]";
                    FirepowerButton.interactable = false;
                    FirepowerCoin.text = "MAX";
                }
                else
                {
                    int new_default_val = (lvl * LOOP_VAL) + DEFAULT_VAL;
                    int new_default_coin = (lvl * LOOP_COIN) + DEFAULT_COIN;

                    Debug.Log(new_default_val);
                    Debug.Log(new_default_coin);

                    FirepowerLvl.text = "FIREPOWER [LV" + lvl + "]";
                    FirepowerCoin.text = FormatCoin(new_default_coin);
                }
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    public void checkPowerups()
    {
        try
        {
            float coin = float.Parse(DecryptCoin(Coin.text));

            float firerateCoin = float.Parse(DecryptCoin(FirerateCoin.text));
            if (firerateCoin > coin)
            {
                FirerateButton.interactable = false;
            }
            else
            {
                FirerateButton.interactable = true;
            }

            if (FirepowerCoin.text == "MAX")
            {
                return;
            }
            
            float firepowerCoin = float.Parse(DecryptCoin(FirepowerCoin.text));
            if (firepowerCoin > coin)
            {
                FirepowerButton.interactable = false;
            }
            else
            {
                FirepowerButton.interactable = true;
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }


    static string DecryptCoin(string num)
    {
        num = num.ToUpper();

        if (num.Contains("K") && num.Contains("."))
        {
            num = num.Replace(".", "").Replace("K", "00");
        }
        else if (num.Contains("K"))
        {
            num = num.Replace("K", "000");
        }

        return num;
    }
}
