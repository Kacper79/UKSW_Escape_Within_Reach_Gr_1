using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zarz�dza d�wi�kami i muzyk� w grze jako singleton.
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
    /// Ustawia instancj� singletona AudioManager. 
    /// Gwarantuje, �e istnieje tylko jedna instancja w grze. 
    /// Obiekt nie jest niszczony podczas prze�adowania scen.
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
    /// Odtwarza jednorazowy efekt d�wi�kowy.
    /// </summary>
    /// <param name="clip">AudioClip, kt�ry ma zosta� odtworzony.</param>
    public void PlayGivenClip(AudioClip clip)
    {
        AudioSource audio_source = new GameObject("Audio Source").AddComponent<AudioSource>();

        audio_source.clip = clip;
        audio_source.volume = 3.0f; // Domy�lna g�o�no�� (mo�e by� powi�zana z ustawieniami u�ytkownika)
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
    /// Rozpoczyna odtwarzanie muzyki w p�tli, zast�puj�c wcze�niej odtwarzan� muzyk�.
    /// </summary>
    /// <param name="music">AudioClip z muzyk� do odtworzenia.</param>
    public void PlayMusic(AudioClip music)
    {
        Destroy(current_music_gameobject);

        current_music_gameobject = new GameObject("Audio Source");
        AudioSource audio_source = current_music_gameobject.AddComponent<AudioSource>();

        audio_source.clip = music;
        audio_source.volume = 3.0f; // Domy�lna g�o�no�� muzyki (mo�e by� powi�zana z ustawieniami u�ytkownika)
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
