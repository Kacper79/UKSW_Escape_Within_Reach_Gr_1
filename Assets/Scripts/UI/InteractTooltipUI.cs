using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wyswietlanie i zarzadzanie tooltipem interakcji.
/// </summary>
public class InteractTooltipUI : MonoBehaviour
{
    /// <summary>
    /// Pole przechowujace komponent TextMeshProUGUI dla tekstu tooltipa.
    /// </summary>
    [SerializeField] private TextMeshProUGUI interaction_tooltip_tmp;


    /// <summary>
    /// Subskrybuje zdarzenia potrzebne do zarzadzania UI tooltipem.
    /// </summary>
    private void OnEnable()
    {
        DisableUI();

        GlobalEvents.OnNotLookingOnInteractable += OnNotLookingForAnythingInteractable;

        GlobalEvents.OnReadingPage += DisableSelf;
        GlobalEvents.OnStoppingReadingPage += EnableSelf;

        GlobalEvents.OnStartingDialogue += DisableSelf;
        GlobalEvents.OnEndingDialogue += EnableSelf;

        GlobalEvents.OnStartingBlackJackGameForMoney += DisableSelf;
        GlobalEvents.OnStartingBlackJackGameForPickaxe += DisableSelf;

        GlobalEvents.OnEndingBlackjackGame += EnableSelf;
    }

    /// <summary>
    /// Anuluje subskrypcje zdarzen przy wylaczeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnNotLookingOnInteractable -= OnNotLookingForAnythingInteractable;

        GlobalEvents.OnReadingPage -= DisableSelf;
        GlobalEvents.OnStoppingReadingPage -= EnableSelf;

        GlobalEvents.OnStartingDialogue -= DisableSelf;
        GlobalEvents.OnEndingDialogue -= EnableSelf;

        GlobalEvents.OnStartingBlackJackGameForMoney -= DisableSelf;
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= DisableSelf;

        GlobalEvents.OnEndingBlackjackGame -= EnableSelf;
    }

    /// <summary>
    /// Wlacza UI tooltipa, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void EnableSelf(object sender, System.EventArgs e)
    {
        EnableUI();
    }

    /// <summary>
    /// Wylacza UI tooltipa, gdy rozpoczniemy interakcje z obiektem.
    /// </summary>
    private void DisableSelf(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    /// <summary>
    /// Wylacza UI tooltipa, gdy nie patrzymy na zaden interaktywny obiekt.
    /// </summary>
    private void OnNotLookingForAnythingInteractable(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    /// <summary>
    /// Wlacza UI tooltipa.
    /// </summary>
    private void EnableUI()
    {
        interaction_tooltip_tmp.enabled = true;
    }

    /// <summary>
    /// Wylacza UI tooltipa.
    /// </summary>
    public void DisableUI()
    {
        interaction_tooltip_tmp.enabled = false;
    }

    /// <summary>
    /// Ustawia tekst tooltipa.
    /// </summary>
    /// <param name="tooltip_message">Tekst, ktory ma sie pojawiac w tooltipie.</param>
    public void SetTooltip(string tooltip_message)
    {
        EnableUI();
        interaction_tooltip_tmp.text = tooltip_message;
    }
}
