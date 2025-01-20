using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie interfejsem uzytkownika (UI) postepu wydobycia.
/// </summary>
public class MiningProgressUI : MonoBehaviour
{
    /// <summary>
    /// Pasek postepu, ktory reprezentuje postep wydobycia.
    /// </summary>
    [SerializeField] private ProgressBar mining_progress_bar;

    /// <summary>
    /// Inicjalizuje UI i ustawia jego poczatkowy stan (wylaczony).
    /// </summary>
    private void Start()
    {
        DisableUI();
    }

    /// <summary>
    /// Subskrybuje zdarzenie, ktore wylacza UI, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnNotLookingOnInteractable += DisableUI;
    }

    /// <summary>
    /// Anuluje subskrypcje zdarzenia przy wylaczeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnNotLookingOnInteractable -= DisableUI;
    }

    /// <summary>
    /// Wylacza UI wydobycia, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void DisableUI(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    /// <summary>
    /// Wlacza UI postepu wydobycia.
    /// </summary>
    public void EnableUI()
    {
        mining_progress_bar.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wylacza UI postepu wydobycia.
    /// </summary>
    public void DisableUI()
    {
        mining_progress_bar.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ustawia wartosci paska postepu na podstawie postepu i maksymalnego postepu.
    /// </summary>
    /// <param name="progress">Aktualny postep wydobycia.</param>
    /// <param name="max_progress">Maksymalny postep wydobycia.</param>
    public void PassValuesToProgressbar(float progress, float max_progress)
    {
        mining_progress_bar.SetProgressBarValues(progress, max_progress);
    }
}
