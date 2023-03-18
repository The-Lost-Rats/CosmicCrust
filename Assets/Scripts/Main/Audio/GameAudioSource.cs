using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameAudioSource : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetAudioClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }
}