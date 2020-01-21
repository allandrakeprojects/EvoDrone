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
    public GameObject PlayerDrone;

    [SerializeField]
    static Text coinCountText;
    [SerializeField]
    static Text scoreText;

    [SerializeField]
    public Text finishText;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    private int coins = 0;

    private int monsterWaveCount = 5;
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

    private float timer = 0.0f;


    public GameObject Score_Count;
    public GameObject Score_Text;

    public GameObject Finish_Header;
    public GameObject Finish;

    private static Text score;
    public Text Score_Text_Header;

    [Header("Music Clip")]
    public AudioClip introClip;
    public AudioClip endlessClip;
    public AudioClip level1Clip;
    public AudioClip level2Clip;
    public AudioClip level3Clip;
    public AudioClip level4Clip;
    public AudioClip level5Clip;

    [Header("Music")]
    public AudioSource introAS;
    public AudioSource endlessAS;
    public AudioSource level1AS;
    public AudioSource level2AS;
    public AudioSource level3AS;
    public AudioSource level4AS;
    public AudioSource level5AS;

    private bool detectStartPlaying = false;

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

        //intro music
        introAS.PlayOneShot(introClip);
        detectStartPlaying = true;

        string playmode = PlayerPrefs.GetString("playmode");
        if (playmode == "endless")
        {
            timer = 0.0f;
        }
        else
        {
            int selected_level = PlayerPrefs.GetInt("selected_level");

            if (selected_level == 1)
            {
                timer = 0.0f;
                wavesLeft = 10;
            }
            else if (selected_level == 2)
            {
                timer = 60.0f;
                wavesLeft = 20;
                PlayerPrefs.SetInt("current_level", 2);
                PlayerPrefs.Save();
            }
            else if (selected_level == 3)
            {
                timer = 120.0f;
                wavesLeft = 30;
                PlayerPrefs.SetInt("current_level", 3);
                PlayerPrefs.Save();
            }
            else if (selected_level == 4)
            {
                timer = 180.0f;
                wavesLeft = 40;
                PlayerPrefs.SetInt("current_level", 4);
                PlayerPrefs.Save();
            }
            else if (selected_level == 5)
            {
                timer = 240.0f;
                wavesLeft = 50;
                PlayerPrefs.SetInt("current_level", 5);
                PlayerPrefs.Save();
            }
        }

        Invoke("StartEnemyGeneration", 5f);
    }

    public void Update()
    {
        if (detectStartPlaying)
        {
            if (!introAS.isPlaying)
            {
                detectStartPlaying = false;

                string playmode_ = PlayerPrefs.GetString("playmode");
                if (playmode_ == "endless")
                {
                    endlessAS.loop = true;
                    endlessAS.PlayOneShot(endlessClip);
                }
                else
                {
                    int selected_level = PlayerPrefs.GetInt("selected_level");
                    if (selected_level == 1)
                    {
                        level1AS.loop = true;
                        level1AS.PlayOneShot(level1Clip);
                    }
                    else if (selected_level == 2)
                    {
                        level2AS.loop = true;
                        level2AS.PlayOneShot(level2Clip);
                    }
                    else if (selected_level == 3)
                    {
                        level3AS.loop = true;
                        level3AS.PlayOneShot(level3Clip);
                    }
                    else if (selected_level == 4)
                    {
                        level4AS.loop = true;
                        level4AS.PlayOneShot(level4Clip);
                    }
                    else if (selected_level == 5)
                    {
                        level5AS.loop = true;
                        level5AS.PlayOneShot(level5Clip);
                    }
                }
            }
        }


        string playmode = PlayerPrefs.GetString("playmode");
        if (playmode == "endless")
        {
            timer += Time.deltaTime;

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");

            //string time = string.Format("{0}:{1}", minutes, seconds);
            string get_time = minutes + "" + seconds;
            int time = int.Parse(get_time);

            if (time > 0500)
            {
                //print("level 6 " + time + " --- " + timer);
                PlayerPrefs.SetInt("current_level", 6);
                PlayerPrefs.Save();
            }
            else if (time > 0400)
            {
                //print("level 5 " + time + " --- " + timer);
                PlayerPrefs.SetInt("current_level", 5);
                PlayerPrefs.Save();
            }
            else if (time > 0300)
            {
                //print("level 4 " + time + " --- " + timer);
                PlayerPrefs.SetInt("current_level", 4);
                PlayerPrefs.Save();
            }
            else if (time > 0200)
            {
                //print("level 3 " + time + " --- " + timer);
                PlayerPrefs.SetInt("current_level", 3);
                PlayerPrefs.Save();
            }
            else if (time > 0100)
            {
                //print("level 2 " + time + " --- " + timer);
                PlayerPrefs.SetInt("current_level", 2);
                PlayerPrefs.Save();
            }
            else
            {
                //print("level 1 " + time + " --- " + timer);
            }
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
        string playmode = PlayerPrefs.GetString("playmode");
        if (playmode == "endless")
        {
            monsterSpeed++;
            monsterWaveCount += 2;
            wavesLeft = monsterWaveCount;

            StartEnemyGeneration();
        }
        else
        {
            int level = PlayerPrefs.GetInt("level");
            int selected_level = PlayerPrefs.GetInt("selected_level");

            if (level == selected_level)
            {
                if (level != 5)
                {
                    level += 1;
                    PlayerPrefs.SetInt("level", level);
                    PlayerPrefs.Save();

                    finishText.text = "LEVEL PASSED";
                }
                else
                {
                    // show win
                    finishText.text = "MAX LEVEL";
                }
            }
            else
            {
                finishText.text = "LEVEL PASSED";
            }

            Invoke("ShowFinishScreen", 3f);
        }
    }

    public void ShowFinishScreen()
    {
        PlayerDrone.SetActive(false);

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

        Text scoreFinish = Finish.GetComponentInChildren<Text>();
        scoreFinish.text = score.text;
        Finish_Header.SetActive(true);
        Finish.SetActive(true);

        int new_coin = PlayerPrefs.GetInt("coin");
        new_coin += int.Parse(score.text);
        PlayerPrefs.SetInt("coin", new_coin);
        PlayerPrefs.Save();
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

        int selected_level = PlayerPrefs.GetInt("selected_level");

        if (selected_level == 1)
        {
            Instantiate(boss_01, bossPos, Quaternion.identity, transform);
        }
        else if (selected_level == 2)
        {
            Instantiate(boss_02, bossPos, Quaternion.identity, transform);
        }
        else if (selected_level == 3)
        {
            Instantiate(boss_03, bossPos, Quaternion.identity, transform);
        }
        else if (selected_level == 4)
        {
            Instantiate(boss_04, bossPos, Quaternion.identity, transform);
        }
        else if (selected_level == 5)
        {
            Instantiate(boss_05, bossPos, Quaternion.identity, transform);
        }

        //int rand = Random.Range(1, 6);
        //if (rand == 1)
        //{
        //    Instantiate(boss_01, bossPos, Quaternion.identity, transform);
        //}
        //else if (rand == 2)
        //{
        //    Instantiate(boss_02, bossPos, Quaternion.identity, transform);
        //}
        //else if (rand == 3)
        //{
        //    Instantiate(boss_03, bossPos, Quaternion.identity, transform);
        //}
        //else if (rand == 4)
        //{
        //    Instantiate(boss_04, bossPos, Quaternion.identity, transform);
        //}
        //else if (rand == 5)
        //{
        //    Instantiate(boss_05, bossPos, Quaternion.identity, transform);
        //}
        //else
        //{
        //    Instantiate(boss_05, bossPos, Quaternion.identity, transform);
        //}

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
            Instantiate(wave_06);
        }
    }

    public void GenerateWave()
    {
        print(wavesLeft);
        if (wavesLeft == 0)
        {
            CancelInvoke();

            string playmode = PlayerPrefs.GetString("playmode");
            if (playmode == "endless")
            {
                monsterSpeed++;
                monsterWaveCount += 2;
                wavesLeft = monsterWaveCount;
                Invoke("StartEnemyGeneration", 5f);
            }
            else
            {
                Invoke("GenerateBoss", 5f);
            }
        }
        else
        {
            wavesLeft--;

            int current_level = PlayerPrefs.GetInt("current_level");
            if (current_level == 2 || current_level == 3 || current_level == 4)
            {
                if (current_level == 4)
                {
                    int rand = Random.Range(1, 2);
                    if (rand == 1)
                    {
                        GenerateDefaultWave();
                    }
                }
                else
                {
                    int rand = Random.Range(1, 4);
                    if (rand == 1)
                    {
                        GenerateDefaultWave();
                    }
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
