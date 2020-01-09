using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Player player;
    [SerializeField]
    public Boss boss_01;
    [SerializeField]
    public Boss boss_02;
    [SerializeField]
    public Boss boss_03;
    [SerializeField]
    public Boss boss_04;
    [SerializeField]
    public Boss boss_05;
    [SerializeField]
    public Enemy enemy;
    [SerializeField]
    public GameObject wave;

    [SerializeField]
    static Text coinCountText;
    [SerializeField]
    static Text scoreText;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    private int coins = 0;

    private int monsterWaveCount = 1;
    private int wavesLeft;
    private float monsterSpeed = 2;

    void Start()
    {
        if (coinCountText == null)
        {
            coinCountText = GameObject.Find("Score_Count").GetComponentInChildren<Text>();
        }

        if (scoreText == null)
        {
            scoreText = GameObject.Find("Score").GetComponentInChildren<Text>();
        }

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        Player.instance.OnGainCoin += HandleGainCoin;
        Player.instance.OnPlayerDied += HandlePlayerDeath;



        wavesLeft = monsterWaveCount;

        StartEnemyGeneration();
    }

    private float timer = 0.0f;
    public float interpolationPeriod = 0.1f;

    public void Update()
    {
        //timer += Time.deltaTime;

        //string minutes = Mathf.Floor(timer / 60).ToString("00");
        //string seconds = (timer % 60).ToString("00");

        //print(string.Format("{0}:{1}", minutes, seconds));
    }

    public void HandleGainCoin()
    {
        coins++;
        coinCountText.text = coins.ToString();
        PlayerPrefs.SetInt("score", coins);
        PlayerPrefs.Save();
        NewBest(coins);
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("PLAYER DIED");
        CancelInvoke();
    }

    public void HandleBossDeath()
    {
        Debug.Log("BOSS DIED");
        monsterSpeed++;
        monsterWaveCount+=2;
        wavesLeft = monsterWaveCount;

        StartEnemyGeneration();
    }

    public void NewBest(int score)
    {
        int highscore = PlayerPrefs.GetInt("highscore");
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            scoreText.text = "NEW BEST";
        }
    }

    public void StartEnemyGeneration()
    {
        InvokeRepeating("GenerateWave", 2 ,3);
    }

    public void GenerateBoss()
    {
        Vector2 bossPos = new Vector2(0, topRight.y);

        int rand = Random.Range(1, 5);

        if (rand == 1)
        {
            Instantiate(boss_01, bossPos, Quaternion.identity, transform);
        }
        else if (rand == 2)
        {
            Instantiate(boss_02, bossPos, Quaternion.identity, transform);
        }
        else if (rand == 3)
        {
            Instantiate(boss_03, bossPos, Quaternion.identity, transform);
        }
        else if (rand == 4)
        {
            Instantiate(boss_04, bossPos, Quaternion.identity, transform);
        }
        else if (rand == 5)
        {
            Instantiate(boss_05, bossPos, Quaternion.identity, transform);
        }
        
        Boss.instance.OnBossDied += HandleBossDeath;
    }

    public void GenerateWave()
    {
        if (wavesLeft == 0)
        {
            CancelInvoke();
            Invoke("GenerateBoss", 5f);
        }
        else
        {
            wavesLeft--;
            GameObject enemyWave = Instantiate(wave, Vector2.zero, Quaternion.identity, transform);

            for (int i = 0; i < 5; i++)
            {
                float x = (i + 0.5f) / 5;
                Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * x, Screen.height));
                pos += Vector2.up * enemy.transform.localScale.y;
                Instantiate(enemy, pos, Quaternion.identity, enemyWave.transform);
            }
        }
    }
}
