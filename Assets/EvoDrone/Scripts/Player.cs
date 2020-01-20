using System.Collections;
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

    public delegate void GainCoin();

    public event GainCoin OnGainCoin;

    public delegate void PlayerDied();

    public event PlayerDied OnPlayerDied;

    public static Player instance;
    private static Text score;
    public Text Score_Text_Header;
    public Text scoreText;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)
    {
        if (OnPlayerDied != null)
        {
            OnPlayerDied();
        }

        Destruction();
        ShowDeathScreen();
    }

    public void ShowDeathScreen()
    {
        score = GameObject.Find("Score_Count").GetComponentInChildren<Text>();
        int getCurrentScore = PlayerPrefs.GetInt("score");
        if (Score_Text_Header.text.ToLower() == "new best")
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

        int new_coin = PlayerPrefs.GetInt("coin");
        new_coin += int.Parse(score.text);
        PlayerPrefs.SetInt("coin", new_coin);
        PlayerPrefs.Save();
    }

    //'Player's' destruction procedure
    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect and destroying the 'Player' object
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Coin")
        {
            if (OnGainCoin != null)
            {
                OnGainCoin();
            }

            Destroy(other.gameObject);
        }
    }
}
















