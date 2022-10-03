using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    public static SoundController scInstance = null;
    public AudioSource efxSource;
    public AudioSource secondaryEfxSource;
    public AudioSource tertiaryEfxSource;
    public AudioSource musicSource;

    public AudioClip ingredientPickUp;
    public AudioClip drawerOpening;
    public AudioClip gameStart;
    public AudioClip pizzaCorrect;
    public AudioClip pizzaWrong;

    private Hashtable soundEffects;

    void Awake ()
    {
        if (scInstance == null) {
            scInstance = this;
        }
        else if (scInstance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        InitSounds();
    }

    void InitSounds()
    {
        soundEffects = new Hashtable();
        soundEffects["ingredientPickUp"] = ingredientPickUp;
        soundEffects["drawerOpenhing"] = drawerOpening;
        soundEffects["gameStart"] = gameStart;
        soundEffects["pizzaCorrect"] = pizzaCorrect;
        soundEffects["pizzaWrong"] = pizzaWrong;
    }

    public void PlaySingle(string clipName)
    {
        AudioClip clip = (AudioClip)soundEffects[clipName];

        if(!efxSource.isPlaying)
        {
            efxSource.clip = clip;
            efxSource.Play();
        }
        else if (!secondaryEfxSource.isPlaying)
        {
            secondaryEfxSource.clip = clip;
            secondaryEfxSource.Play();
        }
        else
        {
            tertiaryEfxSource.clip = clip;
            tertiaryEfxSource.Play();
        }
    }

    public void StopSounds()
    {
        efxSource.Stop();
        secondaryEfxSource.Stop();
        tertiaryEfxSource.Stop();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StartMusic()
    {
        musicSource.Play();
    }
}