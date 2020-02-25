using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// defines which shooting mode is activated. It defines characteristics of the shooting modes. Depending on the activated shooting mode, it makes a shot.
/// </summary>

//enumerator defining which shooting mode is currently active
#region Serializable classes
public enum ActiveShootingMode
{
    Short_Lazer, Swirling, Rocket
}

//serializable class describing all existing shooting modes
[System.Serializable]
public class ShootingMode
{
    public string name;

    [Tooltip("shooting frequency. the higher the more frequent")]
    public float fireRate;

    [Tooltip("projectile prefab")]
    public GameObject projectileObject;

    //time for a new shot
    public float nextFire;

    //time for a new shot
    [HideInInspector] public float nextFireAssistant;
}

//guns objects in 'Player's' hierarchy
[System.Serializable]
public class Guns
{
    public GameObject rightGun, leftGun, centralGun, assistantGun;
    [HideInInspector] public ParticleSystem leftGunVFX, rightGunVFX, centralGunVFX, assistantGunVFX;
}
#endregion

public class PlayerShooting : MonoBehaviour
{

    public float fireRate_;

    public ActiveShootingMode activeShootingMode;

    [Tooltip("current weapon power")]
    [Range(1, 4)]       //change it if you wish
    public int weaponPower = 1;

    public ShootingMode[] shootingModes;

    public Guns guns;
    bool shootingIsActive = true;
    [HideInInspector] public int maxweaponPower = 4;
    public static PlayerShooting instance;

    [Header("Music Clip")]
    public AudioClip shootClip;

    [Header("Music")]
    public AudioSource shootAS;

    public GameObject assistantProjectileObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        UpdateFirerate();
        UpdateFirepower();

        //receiving shooting visual effects components
        guns.leftGunVFX = guns.leftGun.GetComponent<ParticleSystem>();
        guns.rightGunVFX = guns.rightGun.GetComponent<ParticleSystem>();
        guns.centralGunVFX = guns.centralGun.GetComponent<ParticleSystem>();
        guns.assistantGunVFX = guns.assistantGun.GetComponent<ParticleSystem>();

        int selectedDrone = PlayerPrefs.GetInt("SELECTED_DRONE", 1);

        if (selectedDrone == 1)
        {
            activeShootingMode = ActiveShootingMode.Short_Lazer;
        }
        else if (selectedDrone == 2)
        {
            activeShootingMode = ActiveShootingMode.Rocket;
        }
        else if (selectedDrone == 3)
        {
            activeShootingMode = ActiveShootingMode.Swirling;
        }
    }


    public void UpdateFirerate()
    {
        float DEFAULT_VAL = 3.3f;
        float LOOP_VAL = .2f;
        if (PlayerPrefs.GetInt("firerate_lvl", 1) == 1)
        {

            PlayerPrefs.SetInt("firerate_lvl", 1);
            PlayerPrefs.Save();
            fireRate_ = 1;
        }
        else
        {
            int lvl = PlayerPrefs.GetInt("firerate_lvl");
            float new_default_val = (lvl * LOOP_VAL) + DEFAULT_VAL;
            fireRate_ = new_default_val;
        }
    }

    public void UpdateFirepower()
    {
        int DEFAULT_VAL = 0;
        int LOOP_VAL = 1;
        if (PlayerPrefs.GetInt("firepower_lvl", 1) == 1)
        {

            PlayerPrefs.SetInt("firepower_lvl", 1);
            PlayerPrefs.Save();
            weaponPower = 1;
        }
        else
        {
            int lvl = PlayerPrefs.GetInt("firepower_lvl");
            int new_default_val = (lvl * LOOP_VAL) + DEFAULT_VAL;
            weaponPower = new_default_val;
        }
    }

    private void Update()
    {
        if (shootingIsActive)
        {
            int mode = (int)activeShootingMode;  //defining active shooting mode index
            //if (mode == (int)ActiveShootingMode.Ray) //if active shooting mode is ray, making a shot at once; if not, checking if the time for the next shot comes
            //    MakeAShot();
            if (mode == (int)ActiveShootingMode.Rocket) //if active shooting mode is rocket, shooting time depends on current weapon power
            {
                print("rocket");
                if (Time.time > shootingModes[mode].nextFire)   //making a shot and setting time for the next one
                {
                    MakeAShot();
                    shootingModes[mode].nextFire = Time.time + 1 / 1.0f / weaponPower;
                }
            }
            else if (mode == (int)ActiveShootingMode.Swirling) //if active shooting mode is rocket, shooting time depends on current weapon power
            {
                print("swirling");
                if (Time.time > shootingModes[mode].nextFire)
                {
                    MakeAShot();
                    shootingModes[mode].nextFire = Time.time + 1 / 1.0f / weaponPower;
                }
            }
            else
            {
                print("default");
                if (Time.time > shootingModes[mode].nextFire)
                {
                    MakeAShot();
                    if (PlayerPrefs.GetInt("level") >= 2)
                    {
                        if (PlayerPrefs.GetInt("IS_SIDEKICK_ALIVE") == 1)
                        {
                            MakeAShotAssistant();
                        }
                    }
                    shootingModes[mode].nextFire = Time.time + 1 / fireRate_;
                }
            }

            if (Time.time > shootingModes[0].nextFire)
            {
                print("asdasdasdas");
                if (PlayerPrefs.GetInt("level") >= 2)
                {
                    if (PlayerPrefs.GetInt("IS_SIDEKICK_ALIVE") == 1)
                    {
                        MakeAShotAssistant();
                    }
                }
                shootingModes[0].nextFire = Time.time + 1 / fireRate_;
            }
        }
    }

    //method for a shot
    void MakeAShot()
    {
        switch (activeShootingMode) //depending on active shooting mode
        {
            #region Short_Lazer Shot
            case ActiveShootingMode.Short_Lazer:         // if shooting mode is short_lazer
                {
                    //if (guns.PlayerRay.activeSelf)  // if the ray was active, deactivating the ray
                    //    guns.PlayerRay.SetActive(false);

                    switch (weaponPower) // according to weapon power 'pooling' the defined anount of projectiles, on the defined position, in the defined rotation
                    {
                        case 1:
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.centralGun.transform.position, Vector3.zero);
                            guns.centralGunVFX.Play();
                            break;
                        case 2:
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.rightGun.transform.position, Vector3.zero);
                            guns.leftGunVFX.Play();
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.leftGun.transform.position, Vector3.zero);
                            guns.rightGunVFX.Play();
                            break;
                        case 3:
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.centralGun.transform.position, Vector3.zero);
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.rightGun.transform.position, new Vector3(0, 0, -5));
                            guns.leftGunVFX.Play();
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.leftGun.transform.position, new Vector3(0, 0, 5));
                            guns.rightGunVFX.Play();
                            break;
                        case 4:
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.centralGun.transform.position, Vector3.zero);
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.rightGun.transform.position, new Vector3(0, 0, -5));
                            guns.leftGunVFX.Play();
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.leftGun.transform.position, new Vector3(0, 0, 5));
                            guns.rightGunVFX.Play();
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.leftGun.transform.position, new Vector3(0, 0, 15));
                            CreateLazerShot(PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject), guns.rightGun.transform.position, new Vector3(0, 0, -15));
                            break;
                    }
                    shootAS.PlayOneShot(shootClip);
                    //SoundManager.instance.PlaySound("shortLazer");     //play sound from 'SoundManager'
                    break;
                }
            #endregion
            #region Swirling Shot 
            case ActiveShootingMode.Swirling:        // if shootingMode is 'swirling' generating Swirling prefab
                {
                    //if (guns.PlayerRay.activeSelf) // if the ray was active, deactivating the ray
                    //    guns.PlayerRay.SetActive(false);
                    Instantiate(shootingModes[(int)activeShootingMode].projectileObject, guns.centralGun.transform.position, guns.centralGun.transform.rotation);
                    shootAS.PlayOneShot(shootClip);
                    //SoundManager.instance.PlaySound("swirling");
                    break;
                }
            #endregion
            #region Rocket Shot
            case ActiveShootingMode.Rocket:      // if shooting mode is 'rocket' 'pooling' the new rocket
                {
                    //if (guns.PlayerRay.activeSelf) // if the ray was active, deactivating the ray
                    //    guns.PlayerRay.SetActive(false);
                    GameObject newRocket = PoolingController.instance.GetPoolingObject(shootingModes[(int)activeShootingMode].projectileObject);
                    newRocket.transform.position = transform.position;
                    newRocket.SetActive(true);
                    shootAS.PlayOneShot(shootClip);
                    //SoundManager.instance.PlaySound("rocket");     //play sound from 'SoundManager'
                    break;
                }
            #endregion
            #region Ray
            //case ActiveShootingMode.Ray:     // is the shooting mode is 'ray'
            //    {
            //        //if (!guns.PlayerRay.activeSelf && PlayerMoving.instance.controlIsActive) // if the ray was active, deactivating the ray
            //        //    guns.PlayerRay.SetActive(true);
            //        break;
            //    }
            #endregion*/
            default:
                break;
        }
    }

    void MakeAShotAssistant()
    {
        CreateLazerShotAssistant(assistantProjectileObject, guns.assistantGun.transform.position, Vector3.zero);
        guns.assistantGunVFX.Play();
    }

    void CreateLazerShot(GameObject lazer, Vector3 pos, Vector3 rot) //translating 'pooled' lazer shot to the defined position in the defined rotation
    {
        lazer.transform.position = pos;
        lazer.transform.rotation = Quaternion.Euler(rot);
        lazer.SetActive(true);
    }

    void CreateLazerShotAssistant(GameObject lazer, Vector3 pos, Vector3 rot) //translating 'pooled' lazer shot to the defined position in the defined rotation
    {
        Instantiate(lazer, pos, Quaternion.Euler(rot));
        lazer.transform.position = pos;
        lazer.transform.rotation = Quaternion.Euler(rot);
        lazer.SetActive(true);
    }
}
