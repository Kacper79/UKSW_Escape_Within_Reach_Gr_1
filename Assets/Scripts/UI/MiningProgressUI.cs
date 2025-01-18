using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie interfejsem u�ytkownika (UI) post�pu wydobycia.
/// </summary>
public class MiningProgressUI : MonoBehaviour
{
    /// <summary>
    /// Pasek post�pu, kt�ry reprezentuje post�p wydobycia.
    /// </summary>
    [SerializeField] private ProgressBar mining_progress_bar;

    /// <summary>
    /// Inicjalizuje UI i ustawia jego pocz�tkowy stan (wy��czony).
    /// </summary>
    private void Start()
    {
        DisableUI();
    }

    /// <summary>
    /// Subskrybuje zdarzenie, kt�re wy��cza UI, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnNotLookingOnInteractable += DisableUI;
    }

    /// <summary>
    /// Anuluje subskrypcj� zdarzenia przy wy��czeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnNotLookingOnInteractable -= DisableUI;
    }

    /// <summary>
    /// Wy��cza UI wydobycia, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void DisableUI(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    /// <summary>
    /// W��cza UI post�pu wydobycia.
    /// </summary>
    public void EnableUI()
    {
        mining_progress_bar.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wy��cza UI post�pu wydobycia.
    /// </summary>
    public void DisableUI()
    {
        mining_progress_bar.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ustawia warto�ci paska post�pu na podstawie post�pu i maksymalnego post�pu.
    /// </summary>
    /// <param name="progress">Aktualny post�p wydobycia.</param>
    /// <param name="max_progress">Maksymalny post�p wydobycia.</param>
    public void PassValuesToProgressbar(float progress, float max_progress)
    {
        mining_progress_bar.SetProgressBarValues(progress, max_progress);
    }
}
