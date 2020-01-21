using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    private Music music;
    public Image musicImage;
    public Image soundImage;

    void Start()
    {
        music = GameObject.FindObjectOfType<Music>();
        UpdateIconMusic();
        UpdateIconSound();
    }

    public void MuteMusic()
    {
        music.ToggleMusic();
        UpdateIconMusic();
    }

    public void MuteSound()
    {
        music.ToggleSound();
        UpdateIconSound();
    }

    public void UpdateIconMusic()
    {
        try
        {
            if (PlayerPrefs.GetInt("Muted_Music", 1) == 1)
            {
                PlayerPrefs.SetInt("Muted_Music", 1);
                musicImage.color = new Color32(255, 255, 225, 255);
                Mute("Music", false);
            }
            else
            {
                PlayerPrefs.SetInt("Muted_Music", 0);
                musicImage.color = new Color32(255, 255, 225, 80);
                Mute("Music", true);
            }

            PlayerPrefs.Save();
        }
        catch (Exception err)
        {
            if (PlayerPrefs.GetInt("Muted_Music", 1) == 1)
            {
                PlayerPrefs.SetInt("Muted_Music", 1);
                Mute("Music", false);
            }
            else
            {
                PlayerPrefs.SetInt("Muted_Music", 0);
                Mute("Music", true);
            }

            PlayerPrefs.Save();
        }
    }

    public void UpdateIconSound()
    {
        try
        {
            if (PlayerPrefs.GetInt("Muted_Sound", 1) == 1)
            {
                PlayerPrefs.SetInt("Muted_Sound", 1);
                soundImage.color = new Color32(255, 255, 225, 255);
                Mute("Sound", false);
            }
            else
            {
                PlayerPrefs.SetInt("Muted_Sound", 0);
                soundImage.color = new Color32(255, 255, 225, 80);
                Mute("Sound", true);
            }

            PlayerPrefs.Save();
        }
        catch (Exception err)
        {
            if (PlayerPrefs.GetInt("Muted_Sound", 1) == 1)
            {
                PlayerPrefs.SetInt("Muted_Sound", 1);
                Mute("Sound", false);
            }
            else
            {
                PlayerPrefs.SetInt("Muted_Sound", 0);
                Mute("Sound", true);
            }

            PlayerPrefs.Save();
        }
    }

    public void Mute(string tag, bool status)
    {
        var objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                AudioSource audioSource = obj.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.mute = status;
                }
            }
        }
    }
}