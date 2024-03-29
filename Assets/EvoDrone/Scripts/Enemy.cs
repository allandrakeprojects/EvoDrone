﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>
public class Enemy : MonoBehaviour {

    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health;

    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;
    
    [HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path
    [HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path
    #endregion

    [SerializeField] private Coin coin;

    public float enemySpeed = 5f;

    public AudioClip explosionClip;

    public GameObject healthParent, healthBar;

    public void Update()
    {
        transform.Translate(Vector2.down * enemySpeed * Time.deltaTime);

        if (transform.position.y + transform.localScale.y <= GameManager.bottomLeft.y)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(int speed)
    {
        enemySpeed = speed;
    }

    private void Start()
    {
        Invoke("ActivateShooting", Random.Range(shotTimeMin, shotTimeMax));
    }

    //coroutine making a shot
    void ActivateShooting() 
    {
        if (Random.value < (float)shotChance / 100)                             //if random value less than shot probability, making a shot
        {                         
            Instantiate(Projectile,  gameObject.transform.position, Quaternion.identity);             
        }
    }

    private float healthLeft = 1f;
    private float newDamage = 0f;

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage) 
    {
        try
        {
            healthParent.SetActive(true);
            if (newDamage == 0f)
            {
                newDamage = 1.0f / health;
            }
            healthLeft -= (newDamage * damage);
            healthBar.transform.localScale = new Vector3(healthLeft, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            health -= damage;

            if (health <= 0)
                Destruction();
            else
                Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
        }
        catch (Exception err)
        {
            // leave blank
        }
    }    

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.tag == "SideKick")
            {
                SideKick.instance.GetDamage(1);
                print("testtesttest");

                return;
            }

            if (collision.tag == "Player")
            {
                Player.instance.GetDamage(1);
                //if (Projectile.GetComponent<Projectile>() != null)
                //    Player.instance.GetDamage(Projectile.GetComponent<Projectile>().damage);
                //else
                //    Player.instance.GetDamage(1);
            }
        }
        catch (Exception err)
        {
            // leave blank
        }
    }

    private int asd = 0;
    //method of destroying the 'Enemy'
    void Destruction()
    {
        int muted_sound = PlayerPrefs.GetInt("Muted_Sound");
        if (muted_sound == 1)
        {
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);
        }

        int coinCount = 1;

        for (int i = 0; i < coinCount; ++i)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }

        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
