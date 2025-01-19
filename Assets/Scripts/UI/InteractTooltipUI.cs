using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wyœwietlanie i zarz¹dzanie tooltipem interakcji.
/// </summary>
public class InteractTooltipUI : MonoBehaviour
{
    /// <summary>
    /// Pole przechowuj¹ce komponent TextMeshProUGUI dla tekstu tooltipa.
    /// </summary>
    [SerializeField] private TextMeshProUGUI interaction_tooltip_tmp;


    /// <summary>
    /// Subskrybuje zdarzenia potrzebne do zarz¹dzania UI tooltipem.
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
    /// Anuluje subskrypcjê zdarzeñ przy wy³¹czeniu obiektu.
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
    /// W³¹cza UI tooltipa, gdy nie patrzymy na interaktywny obiekt.
    /// </summary>
    private void EnableSelf(object sender, System.EventArgs e)
    {
        EnableUI();
    }

    /// <summary>
    /// Wy³¹cza UI tooltipa, gdy rozpoczniemy interakcjê z obiektem.
    /// </summary>
    private void DisableSelf(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    /// <summary>
    /// Wy³¹cza UI tooltipa, gdy nie patrzymy na ¿aden interaktywny obiekt.
    /// </summary>
    private void OnNotLookingForAnythingInteractable(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    /// <summary>
    /// W³¹cza UI tooltipa.
    /// </summary>
    private void EnableUI()
    {
        interaction_tooltip_tmp.enabled = true;
    }

    /// <summary>
    /// Wy³¹cza UI tooltipa.
    /// </summary>
    public void DisableUI()
    {
        interaction_tooltip_tmp.enabled = false;
    }

    /// <summary>
    /// Ustawia tekst tooltipa.
    /// </summary>
    /// <param name="tooltip_message">Tekst, który ma siê pojawiæ w tooltipie.</param>
    public void SetTooltip(string tooltip_message)
    {
        EnableUI();
        interaction_tooltip_tmp.text = tooltip_message;
    }
}
