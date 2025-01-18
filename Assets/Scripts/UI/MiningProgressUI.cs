using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz¹dzanie interfejsem u¿ytkownika (UI) postêpu wydobycia.
/// </summary>
public class MiningProgressUI : MonoBehaviour
{
    /// <summary>
    /// Pasek postêpu, który reprezentuje postêp wydobycia.
    /// </summary>
    [SerializeField] private ProgressBar mining_progress_bar;

    /// <summary>
    /// Inicjalizuje UI i ustawia jego pocz¹tkowy stan (wy³¹czony).
    /// </summary>
    private void Start()
    {
        DisableUI();
    }

    /// <summary>
    /// Subskrybuje zdarzenie, które wy³¹cza UI, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnNotLookingOnInteractable += DisableUI;
    }

    /// <summary>
    /// Anuluje subskrypcjê zdarzenia przy wy³¹czeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnNotLookingOnInteractable -= DisableUI;
    }

    /// <summary>
    /// Wy³¹cza UI wydobycia, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void DisableUI(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    /// <summary>
    /// W³¹cza UI postêpu wydobycia.
    /// </summary>
    public void EnableUI()
    {
        mining_progress_bar.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wy³¹cza UI postêpu wydobycia.
    /// </summary>
    public void DisableUI()
    {
        mining_progress_bar.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ustawia wartoœci paska postêpu na podstawie postêpu i maksymalnego postêpu.
    /// </summary>
    /// <param name="progress">Aktualny postêp wydobycia.</param>
    /// <param name="max_progress">Maksymalny postêp wydobycia.</param>
    public void PassValuesToProgressbar(float progress, float max_progress)
    {
        mining_progress_bar.SetProgressBarValues(progress, max_progress);
    }
}
