using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie animacja przejsc UI, w tym wyswietlanie czarnego tla podczas zmiany scen.
/// </summary>
public class TransitionUI : MonoBehaviour
{
    /// <summary>
    /// Czas trwania animacji przejscia.
    /// </summary>
    private const float TRANSITION_TIME = 0.2f;

    /// <summary>
    /// Obrazek czarnego tla, ktory bedzie stopniowo zapelniac ekran w trakcie przejscia.
    /// </summary>
    [SerializeField] private Image black_background_image;

    /// <summary>
    /// Subskrybuje zdarzenie rozpoczecia przejscia, ktore jest wywolywane z innych czeœci gry.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnStartingTransition += StartTransition;
    }

    /// <summary>
    /// Anuluje subskrypcje zdarzenia po wylaczeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingTransition -= StartTransition;
    }

    /// <summary>
    /// Rozpoczyna animacje przejscia, uruchamiajac odpowiednia coroutine.
    /// </summary>
    /// <param name="sender">Obiekt, ktory wywoluje zdarzenie (globalne). </param>
    /// <param name="args">Argumenty zawierajace czas po zakonczeniu przejscia, ktory jest wykorzystywany do oczekiwania przed zakonczeniem animacji.</param>
    private void StartTransition(object sender, GlobalEvents.OnStartingTransitionEventArgs args)
    {
        StartCoroutine(BeginTransition(args.time_after_the_transition_ends));
    }

    /// <summary>
    /// Rozpoczyna animacje przejscia, stopniowo zmieniajac przezroczystosc tla.
    /// </summary>
    /// <param name="wait_time_after_finishing">Czas oczekiwania po zakonczeniu przejscia, przed rozpoczeciem zakonczenia animacji.</param>
    private IEnumerator BeginTransition(float wait_time_after_finishing)
    {
        float t = 0.0f;

        while (t < TRANSITION_TIME)
        {
            black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               Mathf.Lerp(0.0f, 1.0f, t / TRANSITION_TIME));

            t += Time.deltaTime;

            yield return null;
        }

        black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               1.0f);

        yield return new WaitForSeconds(wait_time_after_finishing);

        StartCoroutine(EndTransition());
    }

    /// <summary>
    /// Konczy animacje przejscia, stopniowo zmieniajac przezroczystosc tla w odwrotna strone.
    /// </summary>
    private IEnumerator EndTransition()
    {
        float t = 0.0f;

        while (t < TRANSITION_TIME)
        {
            black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               Mathf.Lerp(1.0f, 0.0f, t / TRANSITION_TIME));

            t += Time.deltaTime;

            yield return null;
        }

        black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               0.0f);
    }
}
