using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    private static Music instance = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("Muted_Music", 0) == 0)
        {
            PlayerPrefs.SetInt("Muted_Music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Muted_Music", 0);
        }

        PlayerPrefs.Save();
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Muted_Sound", 0) == 0)
        {
            PlayerPrefs.SetInt("Muted_Sound", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Muted_Sound", 0);
        }

        PlayerPrefs.Save();
    }
}
