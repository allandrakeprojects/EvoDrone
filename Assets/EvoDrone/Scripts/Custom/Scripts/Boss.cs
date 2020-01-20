using System;
using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>
public class Boss : MonoBehaviour
{
    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health;

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

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, Time.deltaTime * 3f);
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

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage)
    {
        health -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure
        if (health <= 0)
        {
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

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
