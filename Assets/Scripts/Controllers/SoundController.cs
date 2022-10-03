using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    public static SoundController scInstance = null;
    public AudioSource efxSource;
    public AudioSource secondaryEfxSource;
    public AudioSource tertiaryEfxSource;
    public AudioSource loopingEfxSource;
    public AudioSource musicSource;

    public AudioClip itemGrab;
    public AudioClip pizzaCorrect;
    public AudioClip pizzaWrong;
    public AudioClip drawerOpening;
    public AudioClip meatBoxOpen;
    public AudioClip cheeseGrate;
    public AudioClip sauceSpray;
    public AudioClip wateringCan;

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
        soundEffects["itemGrab"] = itemGrab;
        soundEffects["pizzaCorrect"] = pizzaCorrect;
        soundEffects["pizzaWrong"] = pizzaWrong;
        soundEffects["drawerOpening"] = drawerOpening;
        soundEffects["meatBoxOpen"] = meatBoxOpen;
        soundEffects["cheeseGrate"] = cheeseGrate;
        soundEffects["sauceSpray"] = sauceSpray;
        soundEffects["wateringCan"] = wateringCan;
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
        loopingEfxSource.Stop();
    }

    public void PlayLoopingSound(string clipName)
    {
        loopingEfxSource.clip = (AudioClip)soundEffects[clipName];
        loopingEfxSource.Play();
    }

    public void StopLoopingSound()
    {
        loopingEfxSource.Stop();
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