﻿using System;
using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>
public class Boss : MonoBehaviour
{
    #region FIELDS
    [Tooltip("Health points in integer")]
    public float health;

    [SerializeField] public Transform bulletSpawnspot;

    [SerializeField] public Bullet bulletPrefab;

    [SerializeField] public Transform player;

    public delegate void BossDied();

    public event BossDied OnBossDied;

    public static Boss instance;


    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;

    [HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path
    [HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path
    #endregion

    [SerializeField] private Coin coin;

    public AudioClip explosionClip;

    public GameObject healthParent, healthBar;

    //private float enemySpeed = 5f;

    //public void Update()
    //{
    //    transform.Translate(Vector2.down * enemySpeed * Time.deltaTime);

    //    if (transform.position.y + transform.localScale.y <= GameManager.bottomLeft.y)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //public void SetSpeed(int speed)
    //{
    //    enemySpeed = speed;
    //}

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ActivateShooting", 1, 1);
    }

    private bool detectContinue = false;

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, Time.deltaTime * 3f);

        if (PlayerPrefs.GetInt("IS_PLAYER_ALIVE") == 0)
        {
            detectContinue = true;
            CancelInvoke();
        }

        if (detectContinue)
        {
            if (PlayerPrefs.GetInt("IS_PLAYER_ALIVE") == 1)
            {
                detectContinue = false;
                InvokeRepeating("ActivateShooting", 1, 1);
            }
        }
    }

    //coroutine making a shot
    void ActivateShooting()
    {
        try
        {
            Vector2 player_dir = (player.position - transform.position).normalized;
            Bullet bulletGO = Instantiate(bulletPrefab, bulletSpawnspot.position, Quaternion.identity) as Bullet;
            bulletGO.Init(player_dir, 3f, false);

            //Instantiate(Projectile, gameObject.transform.position, Quaternion.identity);
        }
        catch (Exception err)
        {

        }
    }

    private float healthLeft = 4f;
    private float newDamage = 0f;

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage)
    {
        try
        {
            healthParent.SetActive(true);
            if (newDamage == 0f)
            {
                newDamage = 4.0f / health;
            }
            healthLeft -= (newDamage * damage);
            healthBar.transform.localScale = new Vector3(healthLeft, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            health -= damage;

            if (health <= 0)
            {
                int muted_sound = PlayerPrefs.GetInt("Muted_Sound");
                if (muted_sound == 1)
                {
                    AudioSource.PlayClipAtPoint(explosionClip, transform.position);
                }

                if (OnBossDied != null)
                {
                    OnBossDied();
                }

                Destruction();
                return;
            }
            else
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SideKick")
        {
            SideKick.instance.GetDamage(1);
            print("testtesttest");

            return;
        }

        if (collision.tag == "Player")
        {
            if (Projectile.GetComponent<Projectile>() != null)
                Player.instance.GetDamage(Projectile.GetComponent<Projectile>().damage);
            else
                Player.instance.GetDamage(1);
        }
    }

    private int asd = 0;
    //method of destroying the 'Enemy'
    void Destruction()
    {
        int coinCount = 15;

        for (int i = 0; i < coinCount; ++i)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }

        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
