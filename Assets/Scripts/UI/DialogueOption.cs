using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Klasa reprezentujaca jedna opcje dialogowa, reagujacy na klikniecia uzytkownika.
/// </summary>
public class DialogueOption : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Tekst wyswietlajacy numer opcji dialogowej.
    /// </summary>
    [SerializeField] private TextMeshProUGUI number_text;

    /// <summary>
    /// Tekst wyswietlajacy tresc opcji dialogowej.
    /// </summary>
    [SerializeField] private TextMeshProUGUI dialogue_option_text;

    /// <summary>
    /// Identyfikator opcji dialogowej.
    /// </summary>
    private string option_id;

    /// <summary>
    /// Ustawia informacje o opcji dialogowej.
    /// </summary>
    /// <param name="id">Identyfikator opcji dialogowej.</param>
    /// <param name="number">Numer opcji dialogowej.</param>
    /// <param name="message">Tresc opcji dialogowej.</param>
    public void SetInfo(string id, int number, string message)
    {
        option_id = id;
        number_text.text = number.ToString();
        dialogue_option_text.text = message;
    }

    /// <summary>
    /// Obsluguje klikniecie na opcje dialogowa.
    /// </summary>
    /// <param name="eventData">Dane zdarzenia klikniecia.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEvents.OnChoosingCertainDialogueOptionEventArgs args = new(option_id);
        GlobalEvents.FireOnChoosingCertainDialogueOption(this, args);
    }
}
