using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [SerializeField]
    public AudioClip sound;

    private AudioSource source
    {
        get { return GetComponent<AudioSource>(); }
    }

    void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
    }

    public void PlaySound()
    {
        source.PlayOneShot(sound, 0.5f);
    }
}
