﻿using System;
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

    public Text ScoreText;

    public GameObject gameMode;
    public GameObject storyMode;
    public GameObject droneSelection;
    public GameObject about;

    public GameObject sidekick;
    public GameObject sidekickFireEffect;

    public GameObject pauseButton, pauseMenu;

    public GameObject player_, sidekick_;
    public GameObject[] playerScore;
    public GameObject[] gameOver;

    public Button Level_01;
    public Button Level_02;
    public Button Level_03;
    public Button Level_04;
    public Button Level_05;

    public Image player;
    public GameObject playerGame;
    public Image[] menuShipIcons;

    public Sprite[] menuShipIconsSelected;

    public Sprite[] menuShipIconsNotSelected;

    public Sprite[] menuShipIconsMainMenu;

    void Start()
    {
        ShowCoin();
        ShowBest();
        CheckCoin();
        Powerups();
        SetSelectedDrone();

        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("current_level", 1);
        PlayerPrefs.SetInt("IS_SIDEKICK_ALIVE", 1);
        PlayerPrefs.SetInt("IS_CONTINUE", 0);
        PlayerPrefs.SetInt("IS_PLAYER_ALIVE", 1);
        PlayerPrefs.SetInt("IS_BOSS", 0);
        PlayerPrefs.Save();

        levelManager = GameObject.FindObjectOfType<LevelManager>();

        ShowHideSideKick();
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

    public void ShowHideSideKick()
    {
        try
        {
            if (PlayerPrefs.GetInt("level") >= 2)
            {
                sidekick.SetActive(true);
                sidekickFireEffect.SetActive(true);
            }
            else
            {
                sidekick.SetActive(false);
                sidekickFireEffect.SetActive(false);
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    public void OpenAboutMenu()
    {
        about.SetActive(true);
    }

    public void CloseAboutMenu()
    {
        about.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        StopTheGame();
    }

    void StopTheGame()
    {
        Time.timeScale = 0;
        PlayerMoving.instance.controlIsActive = false;
        pauseButton.SetActive(false);
    }

    public void Continue()
    {
        int new_coin = PlayerPrefs.GetInt("coin");
        new_coin -= 500 + int.Parse(ScoreText.text);
        PlayerPrefs.SetInt("coin", new_coin);
        PlayerPrefs.SetInt("IS_CONTINUE", 1);
        PlayerPrefs.SetInt("IS_SIDEKICK_ALIVE", 1);
        PlayerPrefs.SetInt("IS_PLAYER_ALIVE", 1);
        PlayerPrefs.Save();

        playerScore[0].SetActive(true);
        playerScore[1].SetActive(true);

        gameOver[0].SetActive(false);
        gameOver[1].SetActive(false);

        pauseButton.SetActive(true);

        player_.SetActive(true);
        if (PlayerPrefs.GetInt("level") >= 2)
        {
            sidekick_.SetActive(true);
        }
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        PlayerMoving.instance.controlIsActive = true;
        Time.timeScale = 1;
        pauseButton.SetActive(true);
    }

    public void StoryModeLevel()
    {
        if (PlayerPrefs.GetInt("level", 1) == 1)
        {
            // level 1
            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.Save();

            LockLevel(Level_02);
            LockLevel(Level_03);
            LockLevel(Level_04);
            LockLevel(Level_05);
        }
        else
        {
            int level = PlayerPrefs.GetInt("level");

            if (level == 2)
            {
                UnlockLevel(Level_02);

                LockLevel(Level_03);
                LockLevel(Level_04);
                LockLevel(Level_05);
            }
            else if (level == 3)
            {
                UnlockLevel(Level_02);
                UnlockLevel(Level_03);

                LockLevel(Level_04);
                LockLevel(Level_05);
            }
            else if (level == 4)
            {
                UnlockLevel(Level_02);
                UnlockLevel(Level_03);
                UnlockLevel(Level_04);

                LockLevel(Level_05);
            }
            else if (level == 5)
            {
                UnlockLevel(Level_02);
                UnlockLevel(Level_03);
                UnlockLevel(Level_04);
                UnlockLevel(Level_05);
            }
        }
    }

    private void UnlockLevel(Button level)
    {
        GameObject lock_ = level.transform.GetChild(1).gameObject;
        GameObject unlock_ = level.transform.GetChild(2).gameObject;

        lock_.SetActive(false);
        unlock_.SetActive(true);
    }

    private void LockLevel(Button level)
    {
        GameObject lock_ = level.transform.GetChild(1).gameObject;
        GameObject unlock_ = level.transform.GetChild(2).gameObject;

        lock_.SetActive(true);
        unlock_.SetActive(false);
    }

    public void viewGameMode()
    {
        StoryModeLevel();
        gameMode.SetActive(true);
    }

    public void hideGameMode()
    {
        if (storyMode.active)
        {
            storyMode.SetActive(false);
        }
        else
        {
            gameMode.SetActive(false);
        }
    }

    public void viewStoryMode()
    {
        storyMode.SetActive(true);
    }

    public void hideStoryMode()
    {
        storyMode.SetActive(false);
    }

    public void PLAYGAME_ENDLESS()
    {
        PlayerPrefs.SetString("playmode", "endless");
        PlayerPrefs.Save();

        //levelManager.LoadGameAfterDelay();
    }

    public void PLAYGAME_STORY_01()
    {
        PlayerPrefs.SetString("playmode", "story");
        PlayerPrefs.SetInt("selected_level", 01);
        PlayerPrefs.Save();

        //levelManager.LoadGameAfterDelay();
    }

    public void PLAYGAME_STORY_02()
    {
        PlayerPrefs.SetString("playmode", "story");
        PlayerPrefs.SetInt("selected_level", 02);
        PlayerPrefs.Save();
    }

    public void PLAYGAME_STORY_03()
    {
        PlayerPrefs.SetString("playmode", "story");
        PlayerPrefs.SetInt("selected_level", 03);
        PlayerPrefs.Save();
    }

    public void PLAYGAME_STORY_04()
    {
        PlayerPrefs.SetString("playmode", "story");
        PlayerPrefs.SetInt("selected_level", 04);
        PlayerPrefs.Save();
    }

    public void PLAYGAME_STORY_05()
    {
        PlayerPrefs.SetString("playmode", "story");
        PlayerPrefs.SetInt("selected_level", 05);
        PlayerPrefs.Save();
    }

    public void OpenMainMenu()
    {
        //levelManager.LoadMainMenuAfterDelay();
    }

    public void OpenDroneSelection()
    {
        droneSelection.SetActive(true);
    }

    public void CloseDroneSelection()
    {
        droneSelection.SetActive(false);
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
            int DEFAULT_COIN = 0;
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
            int LOOP_COIN = 2000;
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

    public void SetStartSkin(string mode)
    {
        try
        {
            switch (mode)
            {
                case "short_lazer":
                    //PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Short_Lazer;
                    PlayerPrefs.SetInt("SELECTED_DRONE", 1);
                    DisableMenuIcons();
                    menuShipIcons[0].GetComponent<Image>().sprite = menuShipIconsSelected[0];
                    player.GetComponent<Image>().sprite = menuShipIconsMainMenu[0];
                    break;
                case "rocket":
                    //PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Rocket;
                    PlayerPrefs.SetInt("SELECTED_DRONE", 2);
                    DisableMenuIcons();
                    menuShipIcons[1].GetComponent<Image>().sprite = menuShipIconsSelected[1];
                    player.GetComponent<Image>().sprite = menuShipIconsMainMenu[1];
                    break;
                case "swirling":
                    //PlayerShooting.instance.activeShootingMode = ActiveShootingMode.Swirling;
                    PlayerPrefs.SetInt("SELECTED_DRONE", 3);
                    DisableMenuIcons();
                    menuShipIcons[2].GetComponent<Image>().sprite = menuShipIconsSelected[2];
                    player.GetComponent<Image>().sprite = menuShipIconsMainMenu[2];
                    break;
            }

            PlayerPrefs.Save();
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    void DisableMenuIcons()
    {
        try
        {
            menuShipIcons[0].GetComponent<Image>().sprite = menuShipIconsNotSelected[0];
            menuShipIcons[1].GetComponent<Image>().sprite = menuShipIconsNotSelected[1];
            menuShipIcons[2].GetComponent<Image>().sprite = menuShipIconsNotSelected[2];
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    void SetSelectedDrone()
    {
        try
        {
            int selectedDrone = PlayerPrefs.GetInt("SELECTED_DRONE", 1);

            if (selectedDrone == 1)
            {
                DisableMenuIcons();
                menuShipIcons[0].GetComponent<Image>().sprite = menuShipIconsSelected[0];
                player.GetComponent<Image>().sprite = menuShipIconsMainMenu[0];
            }
            else if (selectedDrone == 2)
            {
                DisableMenuIcons();
                menuShipIcons[1].GetComponent<Image>().sprite = menuShipIconsSelected[1];
                player.GetComponent<Image>().sprite = menuShipIconsMainMenu[1];
            }
            else if (selectedDrone == 3)
            {
                DisableMenuIcons();
                menuShipIcons[2].GetComponent<Image>().sprite = menuShipIconsSelected[2];
                player.GetComponent<Image>().sprite = menuShipIconsMainMenu[2];
            }
        }
        catch (Exception err)
        {
            int selectedDrone = PlayerPrefs.GetInt("SELECTED_DRONE", 1);

            if (selectedDrone == 1)
            {
                playerGame.GetComponent<SpriteRenderer>().sprite = menuShipIconsMainMenu[0];
            }
            else if (selectedDrone == 2)
            {
                playerGame.GetComponent<SpriteRenderer>().sprite = menuShipIconsMainMenu[1];
            }
            else if (selectedDrone == 3)
            {
                playerGame.GetComponent<SpriteRenderer>().sprite = menuShipIconsMainMenu[2];
            }
        }
    }
}
