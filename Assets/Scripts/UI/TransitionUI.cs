using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasa odpowiedzialna za zarz¹dzanie animacj¹ przejœæ UI, w tym wyœwietlanie czarnego t³a podczas zmiany scen.
/// </summary>
public class TransitionUI : MonoBehaviour
{
    /// <summary>
    /// Czas trwania animacji przejœcia.
    /// </summary>
    private const float TRANSITION_TIME = 0.2f;

    /// <summary>
    /// Obrazek czarnego t³a, który bêdzie stopniowo zape³nia³ ekran w trakcie przejœcia.
    /// </summary>
    [SerializeField] private Image black_background_image;

    /// <summary>
    /// Subskrybuje zdarzenie rozpoczêcia przejœcia, które jest wywo³ywane z innych czêœci gry.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnStartingTransition += StartTransition;
    }

    /// <summary>
    /// Anuluje subskrypcjê zdarzenia po wy³¹czeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingTransition -= StartTransition;
    }

    /// <summary>
    /// Rozpoczyna animacjê przejœcia, uruchamiaj¹c odpowiedni¹ coroutine.
    /// </summary>
    /// <param name="sender">Obiekt, który wywo³uje zdarzenie (globalne). </param>
    /// <param name="args">Argumenty zawieraj¹ce czas po zakoñczeniu przejœcia, który jest wykorzystywany do oczekiwania przed zakoñczeniem animacji.</param>
    private void StartTransition(object sender, GlobalEvents.OnStartingTransitionEventArgs args)
    {
        StartCoroutine(BeginTransition(args.time_after_the_transition_ends));
    }

    /// <summary>
    /// Rozpoczyna animacjê przejœcia, stopniowo zmieniaj¹c przezroczystoœæ t³a.
    /// </summary>
    /// <param name="wait_time_after_finishing">Czas oczekiwania po zakoñczeniu przejœcia, przed rozpoczêciem zakoñczenia animacji.</param>
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
    /// Koñczy animacjê przejœcia, stopniowo zmieniaj¹c przezroczystoœæ t³a w odwrotn¹ stronê.
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
