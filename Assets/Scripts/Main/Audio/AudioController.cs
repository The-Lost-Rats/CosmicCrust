using System;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffectKeys
{
    public const string ItemGrab = "Item Grab";
    public const string PizzaCorrect = "Ding!";
    public const string PizzaIncorrect = "Error";
    public const string DrawerOpening = "Cabniet Open";
    public const string MeatBoxOpen = "Box Open";
    public const string CheeseGrate = "Cheese Grating";
    public const string SauceSpray = "Sauce Spraying";
    public const string WateringCan = "Watering Can";
}

public static class MusicKeys
{
    public const string PlayScene = "slap_jazz";
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private GameAudioSource gameAudioSourcePrefab;

    [SerializeField] private List<AudioClip> audioClipsList;
    [SerializeField] private List<AudioClip> musicClipsList;

    [SerializeField] private Dictionary<string, AudioClip> audioClips;
    [SerializeField] private Dictionary<string, AudioClip> musicClips;

    [SerializeField] private GameAudioSource musicSource;
    private List<GameAudioSource> oneShotAudioSources = new List<GameAudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioClips = new Dictionary<string, AudioClip>(audioClipsList.Count);
        foreach (AudioClip clip in audioClipsList)
        {
            audioClips[clip.name] = clip;
        }
        musicClips = new Dictionary<string, AudioClip>(musicClipsList.Count);
        foreach (AudioClip clip in musicClipsList)
        {
            musicClips[clip.name] = clip;
        }

        VerifyAudioSources();
    }

    private void Update()
    {
        for (int i = oneShotAudioSources.Count - 1; i >= 0; i--)
        {
            GameAudioSource audioSource = oneShotAudioSources[i];
            if (!audioSource.IsPlaying())
            {
                Destroy(audioSource.gameObject);
                oneShotAudioSources.Remove(audioSource);
            }
        }
    }

    public void PlayOneShotAudio(string key, float volume = 1f)
    {
        if (audioClips.ContainsKey(key))
        {
            PlayOneShotAudio(audioClips[key], volume);
        }
        else
        {
            throw new Exception(String.Format("Audio clip not available {0}", key));
        }
    }

    public void PlayOneShotAudio(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            GameAudioSource audioSource = Instantiate(gameAudioSourcePrefab, transform);
            audioSource.SetAudioClip(clip);
            audioSource.GetAudioSource().volume = volume; // TODO Will this work? May have to use .GetComponent<AudioSource>()
            oneShotAudioSources.Add(audioSource);
        }
        else
        {
            throw new Exception("Audio clip not available {null}");
        }
    }

    public void StopAllOneShotAudio()
    {
        for (int i = 0; i < oneShotAudioSources.Count; i++)
        {
            oneShotAudioSources[i].GetAudioSource().Stop();
            // Audio source will be cleaned up in the next Update
        }
    }

    public void PlayMusic(string key, float volume = 1f)
    {
        if (musicClips.ContainsKey(key))
        {
            PlayMusic(musicClips[key], volume);
        }
        else
        {
            throw new Exception(String.Format("Music clip not available {0}", key));
        }
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            musicSource.SetAudioClip(clip);
            musicSource.GetAudioSource().volume = volume; // TODO Will this work? May have to use .GetComponent<AudioSource>()
        }
        else
        {
            throw new Exception("Music clip not available {null}");
        }
    }

    public void StopMusic()
    {
        musicSource.GetAudioSource().Stop();
    }

    /**************************************************/

    public void VerifyAudioSources()
    {
        List<string> invalidSoundEffectKeys = new List<string>();
        foreach (System.Reflection.FieldInfo constant in typeof(SoundEffectKeys).GetFields())
        {
            string clipName = (string)constant.GetValue(null);
            if (!audioClips.ContainsKey(clipName))
            {
                invalidSoundEffectKeys.Add(clipName);
            }
        }
        
        List<string> invalidMusicKeys = new List<string>();
        foreach (System.Reflection.FieldInfo constant in typeof(MusicKeys).GetFields())
        {
            string clipName = (string)constant.GetValue(null);
            if (!musicClips.ContainsKey(clipName))
            {
                invalidMusicKeys.Add(clipName);
            }
        }

        if (invalidSoundEffectKeys.Count > 0 || invalidMusicKeys.Count > 0)
        {
            string errorStr = "The following audio clips do not exist!";
            if (invalidSoundEffectKeys.Count > 0)
            {
                errorStr += "\nSound effects:";
                foreach (string clipName in invalidSoundEffectKeys)
                {
                    errorStr += String.Format("\n - {0}", clipName);
                }
            }
            if (invalidMusicKeys.Count > 0)
            {
                errorStr += "\nMusic:";
                foreach (string clipName in invalidMusicKeys)
                {
                    errorStr += String.Format("\n - {0}", clipName);
                }
            }
            throw new Exception(errorStr);
        }
    }
}
