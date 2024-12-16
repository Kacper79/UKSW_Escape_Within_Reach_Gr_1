using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private GameObject current_music_gameobject;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGivenClip(AudioClip clip)
    {
        AudioSource audio_source = new GameObject("Audio Source").AddComponent<AudioSource>();

        audio_source.clip = clip;
        audio_source.volume = 3.0f;//From settings.sfx_volume
        audio_source.loop = false;

        Destroy(audio_source.gameObject, clip.length);
        audio_source.Play();
    }

    public void PlayMusic(AudioClip music)
    {
        Destroy(current_music_gameobject);

        current_music_gameobject = new GameObject("Audio Source");
        AudioSource audio_source = current_music_gameobject.AddComponent<AudioSource>();

        audio_source.clip = music;
        audio_source.volume = 3.0f;//From settings.music_volume
        audio_source.loop = true;

        audio_source.Play();
    }
}
