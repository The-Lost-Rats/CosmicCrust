using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    // To make the hashtable kinda serializable
    [System.Serializable]
    public struct SoundEffectItem
    {
        // Effect name in hash table
        [SerializeField]
        public string id;

        // Audio clip
        [SerializeField]
        public AudioClip clip;
    }

    public static SoundController scInstance = null;

    public AudioSource loopingEfxSource;
    public AudioSource musicSource;

    // List of sources to play singles
    // 0 - primary
    // n - nth efx source
    [SerializeField]
    public List<AudioSource> efxSources;

    [SerializeField]
    private List<SoundEffectItem> soundEffectItems;

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
        // Translate list to hash table
        soundEffects = new Hashtable();
        foreach (SoundEffectItem item in soundEffectItems)
        {
            soundEffects[item.id] = item.clip;
        }
    }

    // Return true if we found a source to play efx
    public bool PlaySingle(string clipName)
    {
        bool playedSource = false;

        // Get audio clip name
        AudioClip clip = (AudioClip)soundEffects[clipName];

        // Find first available source
        foreach (AudioSource efxSource in efxSources)
        {
            if(!efxSource.isPlaying)
            {
                efxSource.clip = clip;
                efxSource.Play();
                playedSource = true;

                break;
            }
        }

        return ( playedSource );
    }

    public void StopSounds()
    {
        foreach (AudioSource efxSource in efxSources)
        {
            efxSource.Stop();
        }

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