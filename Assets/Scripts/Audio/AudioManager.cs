using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zarz¹dza dŸwiêkami i muzyk¹ w grze jako singleton.
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
    /// Ustawia instancjê singletona AudioManager. 
    /// Gwarantuje, ¿e istnieje tylko jedna instancja w grze. 
    /// Obiekt nie jest niszczony podczas prze³adowania scen.
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
    /// Odtwarza jednorazowy efekt dŸwiêkowy.
    /// </summary>
    /// <param name="clip">AudioClip, który ma zostaæ odtworzony.</param>
    public void PlayGivenClip(AudioClip clip)
    {
        AudioSource audio_source = new GameObject("Audio Source").AddComponent<AudioSource>();

        audio_source.clip = clip;
        audio_source.volume = 3.0f; // Domyœlna g³oœnoœæ (mo¿e byæ powi¹zana z ustawieniami u¿ytkownika)
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
    /// Rozpoczyna odtwarzanie muzyki w pêtli, zastêpuj¹c wczeœniej odtwarzan¹ muzykê.
    /// </summary>
    /// <param name="music">AudioClip z muzyk¹ do odtworzenia.</param>
    public void PlayMusic(AudioClip music)
    {
        Destroy(current_music_gameobject);

        current_music_gameobject = new GameObject("Audio Source");
        AudioSource audio_source = current_music_gameobject.AddComponent<AudioSource>();

        audio_source.clip = music;
        audio_source.volume = 3.0f; // Domyœlna g³oœnoœæ muzyki (mo¿e byæ powi¹zana z ustawieniami u¿ytkownika)
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
