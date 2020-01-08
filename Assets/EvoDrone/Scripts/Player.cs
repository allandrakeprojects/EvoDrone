﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    public GameObject destructionFX;
    public GameObject Score_Count;
    public GameObject Score_Text;

    public GameObject GameOver_Header;
    public GameObject GameOver;

    public static Player instance;
    private static Text score;
    public Text scoreText;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)
    {
        Destruction();
        ShowDeathScreen();
    }

    public void ShowDeathScreen()
    {
        score = GameObject.Find("Score_Count").GetComponentInChildren<Text>();
        int highscore = PlayerPrefs.GetInt("highscore");
        int getCurrentScore = PlayerPrefs.GetInt("score");
        if (getCurrentScore > highscore)
        {
            scoreText.text = "NEW BEST";
            PlayerPrefs.SetInt("highscore", getCurrentScore);
            PlayerPrefs.Save();
        }
        Score_Count.SetActive(false);
        Score_Text.SetActive(false);

        Text scoreGameOver = GameOver.GetComponentInChildren<Text>();
        scoreGameOver.text = score.text;
        GameOver_Header.SetActive(true);
        GameOver.SetActive(true);

        //StartCoroutine(ShowGameOverCoroutine());
    }
    IEnumerator ShowGameOverCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
    }

    //'Player's' destruction procedure
    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect and destroying the 'Player' object
        Destroy(gameObject);
    }
}
















