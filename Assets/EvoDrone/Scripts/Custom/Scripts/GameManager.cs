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

    private int monsterWaveCount = 8;
    private int wavesLeft;
    private float monsterSpeed = 2;

    [SerializeField]
    public GameObject wave_01;
    [SerializeField]
    public GameObject wave_02;
    [SerializeField]
    public GameObject wave_03;
    [SerializeField]
    public GameObject wave_04;
    [SerializeField]
    public GameObject wave_05;
    [SerializeField]
    public GameObject wave_06;

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

    public void Update()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        //string time = string.Format("{0}:{1}", minutes, seconds);
        string get_time = minutes +  "" + seconds;
        int time = int.Parse(get_time);

        if (time > 0500)
        {
            print("level 6 " + time);
            PlayerPrefs.SetInt("current_level", 6);
            PlayerPrefs.Save();
        }
        else if (time > 0400)
        {
            print("level 5 " + time);
            PlayerPrefs.SetInt("current_level", 5);
            PlayerPrefs.Save();
        }
        else if (time > 0300)
        {
            print("level 4 " + time);
            PlayerPrefs.SetInt("current_level", 4);
            PlayerPrefs.Save();
        }
        else if (time > 0200)
        {
            print("level 3 " + time);
            PlayerPrefs.SetInt("current_level", 3);
            PlayerPrefs.Save();
        }
        else if (time > 0100)
        {
            print("level 2 " + time);
            PlayerPrefs.SetInt("current_level", 2);
            PlayerPrefs.Save();
        }
        else
        {
            print("level 1 " + time);
        }
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
        CancelInvoke();
    }

    public void HandleBossDeath()
    {
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
        int current_level = PlayerPrefs.GetInt("current_level");
        if (current_level == 1)
        {
            InvokeRepeating("GenerateWave", 1, 6);
        }
        else if (current_level == 2)
        {
            InvokeRepeating("GenerateWave", 1, 5);
        }
        else if (current_level == 3)
        {
            InvokeRepeating("GenerateWave", 1, 4);
        }
        else if (current_level == 4)
        {
            InvokeRepeating("GenerateWave", 1, 3);
        }
        else if (current_level == 5)
        {
            InvokeRepeating("GenerateWave", 1, 2);
        }
    }

    public void GenerateBoss()
    {
        Vector2 bossPos = new Vector2(0, topRight.y);

        int rand = Random.Range(1, 6);
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
        else
        {
            // leave blank
        }
        
        Boss.instance.OnBossDied += HandleBossDeath;
    }

    public void GenerateDefaultWave()
    {

        int rand = Random.Range(1, 7);

        if (rand == 1)
        {
            Instantiate(wave_01);
        }
        else if (rand == 2)
        {
            Instantiate(wave_02);
        }
        else if (rand == 3)
        {
            Instantiate(wave_03);
        }
        else if (rand == 4)
        {
            Instantiate(wave_04);
        }
        else if (rand == 5)
        {
            Instantiate(wave_05);
        }
        else if (rand == 6)
        {
            Instantiate(wave_06);
        }
        else
        {
            // leave blank
        }
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

            int current_level = PlayerPrefs.GetInt("current_level");
            if (current_level == 2 || current_level == 3 || current_level == 4)
            {
                int rand = Random.Range(1, 3);
                if (rand == 1)
                {
                    GenerateDefaultWave();
                }
            }
            else if (current_level == 5 || current_level == 6)
            {
                GenerateDefaultWave();
            }

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
