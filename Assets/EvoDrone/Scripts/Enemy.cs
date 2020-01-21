using UnityEngine;

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

    private float enemySpeed = 5f;

    public AudioClip explosionClip;

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

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage) 
    {
        health -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure
        if (health <= 0)
            Destruction();
        else
            Instantiate(hitEffect,transform.position,Quaternion.identity,transform);
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
