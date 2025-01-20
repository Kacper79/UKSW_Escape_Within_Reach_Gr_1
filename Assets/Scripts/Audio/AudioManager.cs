using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zarzadza dzwiekami i muzyke w grze jako singleton.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Statyczna instancja klasy AudioManager (singleton).
    /// </summary>
    public static AudioManager Instance { get; private set; }

    /// <summary>
    /// Obiekt aktualnie odtwarzanej muzyki.
    /// </summary>
    private GameObject current_music_gameobject;

    /// <summary>
    /// Ustawia instancje singletona AudioManager. 
    /// Gwarantuje, ze istnieje tylko jedna instancja w grze. 
    /// Obiekt nie jest niszczony podczas przeladowania scen.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Odtwarza jednorazowy efekt dzwiekowy.
    /// </summary>
    /// <param name="clip">AudioClip, ktory ma zostaæ odtworzony.</param>
    public void PlayGivenClip(AudioClip clip)
    {
        AudioSource audio_source = new GameObject("Audio Source").AddComponent<AudioSource>();

        audio_source.clip = clip;
        audio_source.volume = Settings.GetAudioEffectsVolume(); // Domyslna glosnosc (moze byc powiazana z ustawieniami uzytkownika)
        audio_source.loop = false;

        Destroy(audio_source.gameObject, clip.length);

        //Mute music for the duration of the clip
        if (current_music_gameobject != null && current_music_gameobject.GetComponent<AudioSource>().isPlaying)
        {
            var asc = current_music_gameobject.GetComponent<AudioSource>();
            StartCoroutine(TurnOnMusic(clip.length + 0.2f, asc.clip));
            asc.Stop();
        }

        audio_source.Play();
    }

    /// <summary>
    /// Rozpoczyna odtwarzanie muzyki w petli, zastepujac wczesniej odtwarzana muzyke.
    /// </summary>
    /// <param name="music">AudioClip z muzyka do odtworzenia.</param>
    public void PlayMusic(AudioClip music)
    {
        Destroy(current_music_gameobject);

        current_music_gameobject = new GameObject("Audio Source");
        AudioSource audio_source = current_music_gameobject.AddComponent<AudioSource>();

        audio_source.clip = music;
        audio_source.volume = Settings.GetMusicVolume(); // Domyslna glosnosc muzyki (moze byc powiazana z ustawieniami uzytkownika)
        audio_source.loop = true;

        audio_source.Play();
    }

    /// <summary>
    /// This coroutine is responsible for turning back the music on after the current audio is played
    /// </summary>
    /// <param name="time">Duration of the current audio to wait for </param>
    /// <param name="music">Passing the current clip with music</param>
    /// <returns></returns>
    public IEnumerator TurnOnMusic(float time, AudioClip music)
    {
        yield return new WaitForSecondsRealtime(time);
        PlayMusic(music);
    }
}
