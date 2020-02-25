using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideKick : MonoBehaviour
{
    public GameObject sidekick, destructionFX;
    public static SideKick instance;

    [Header("Music Clip")]
    public AudioClip explosionClip;
    public AudioClip coinClip;

    [Header("Music")]
    public AudioSource explosionAS;
    public AudioSource coinAS;

    public delegate void GainCoin();

    public event GainCoin OnGainCoin;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void GetDamage(int damage)
    {
        PlayerPrefs.SetInt("IS_SIDEKICK_ALIVE", 0);
        PlayerPrefs.Save();

        explosionAS.PlayOneShot(explosionClip);
        Destruction();
    }

    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity);
        sidekick.SetActive(false);
        //Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Coin")
        {
            coinAS.PlayOneShot(coinClip);

            if (OnGainCoin != null)
            {
                OnGainCoin();
            }

            Destroy(other.gameObject);
        }
    }
}
