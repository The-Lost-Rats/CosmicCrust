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

    private GameAudioSource musicSource;
    private Dictionary<int, GameAudioSource> oneShotAudioSources = new Dictionary<int, GameAudioSource>();
    private int oneShotAudioSourceNextId = 0;

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

        musicSource = Instantiate(gameAudioSourcePrefab, transform);
        musicSource.gameObject.name = "MusicSouce"; // Not necessary, but helpful!
        musicSource.SetLooping(true);
    }

    private void Update()
    {
        foreach (int audioClipId in oneShotAudioSources.Keys)
        {
            GameAudioSource audioSource = oneShotAudioSources[audioClipId];
            if (!audioSource.IsPlaying())
            {
                Destroy(audioSource.gameObject);
                oneShotAudioSources.Remove(audioClipId);
            }
        }
    }

    // TODO replace optional params with options struct
    public int PlayOneShotAudio(string key, bool looping = false, float volume = 1f)
    {
        if (audioClips.ContainsKey(key))
        {
            GameAudioSource audioSource = Instantiate(gameAudioSourcePrefab, transform);
            audioSource.SetAudioClip(audioClips[key]);
            audioSource.SetVolume(volume);
            audioSource.SetLooping(looping);
            oneShotAudioSources.Add(oneShotAudioSourceNextId, audioSource);
            return oneShotAudioSourceNextId++;
        }
        else
        {
            throw new Exception(String.Format("Audio clip not available {0}", key));
        }
    }

    public void StopAllOneShotAudio()
    {
        foreach (GameAudioSource audioSource in oneShotAudioSources.Values)
        {
            audioSource.GetAudioSource().Stop();
            // Audio source will be cleaned up in the next Update
        }
    }

    public void PlayMusic(string key, float volume = 1f)
    {
        if (musicClips.ContainsKey(key))
        {
            musicSource.SetAudioClip(musicClips[key]);
            musicSource.SetVolume(volume);
        }
        else
        {
            throw new Exception(String.Format("Music clip not available {0}", key));
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
