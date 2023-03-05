using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    public float PitchFix;

    public float DefaultVolume;

    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ResetVolume();
    }

    private void Update()
    {
        audioSource.pitch = Random.Range(1f - PitchFix, 1f + PitchFix);
    }

    public void ResetVolume()
    {
        audioSource.volume = DefaultVolume;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlaySound(AudioClip Sound /*, float Volume*/)
    {
        //SetVolume(DefaultVolume);
        if (Sound != null)
        {
            Debug.Log("play sound");
            audioSource.PlayOneShot(Sound);
        }
        //ResetVolume();
    }
}