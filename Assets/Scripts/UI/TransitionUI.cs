using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie animacj� przej�� UI, w tym wy�wietlanie czarnego t�a podczas zmiany scen.
/// </summary>
public class TransitionUI : MonoBehaviour
{
    /// <summary>
    /// Czas trwania animacji przej�cia.
    /// </summary>
    private const float TRANSITION_TIME = 0.2f;

    /// <summary>
    /// Obrazek czarnego t�a, kt�ry b�dzie stopniowo zape�nia� ekran w trakcie przej�cia.
    /// </summary>
    [SerializeField] private Image black_background_image;

    /// <summary>
    /// Subskrybuje zdarzenie rozpocz�cia przej�cia, kt�re jest wywo�ywane z innych cz�ci gry.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnStartingTransition += StartTransition;
    }

    /// <summary>
    /// Anuluje subskrypcj� zdarzenia po wy��czeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingTransition -= StartTransition;
    }

    /// <summary>
    /// Rozpoczyna animacj� przej�cia, uruchamiaj�c odpowiedni� coroutine.
    /// </summary>
    /// <param name="sender">Obiekt, kt�ry wywo�uje zdarzenie (globalne). </param>
    /// <param name="args">Argumenty zawieraj�ce czas po zako�czeniu przej�cia, kt�ry jest wykorzystywany do oczekiwania przed zako�czeniem animacji.</param>
    private void StartTransition(object sender, GlobalEvents.OnStartingTransitionEventArgs args)
    {
        StartCoroutine(BeginTransition(args.time_after_the_transition_ends));
    }

    /// <summary>
    /// Rozpoczyna animacj� przej�cia, stopniowo zmieniaj�c przezroczysto�� t�a.
    /// </summary>
    /// <param name="wait_time_after_finishing">Czas oczekiwania po zako�czeniu przej�cia, przed rozpocz�ciem zako�czenia animacji.</param>
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
    /// Ko�czy animacj� przej�cia, stopniowo zmieniaj�c przezroczysto�� t�a w odwrotn� stron�.
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
