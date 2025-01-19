using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Klasa reprezentuj¹ca jedn¹ opcjê dialogow¹, reaguj¹c¹ na klikniêcia u¿ytkownika.
/// </summary>
public class DialogueOption : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Tekst wyœwietlaj¹cy numer opcji dialogowej.
    /// </summary>
    [SerializeField] private TextMeshProUGUI number_text;

    /// <summary>
    /// Tekst wyœwietlaj¹cy treœæ opcji dialogowej.
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
    /// <param name="message">Treœæ opcji dialogowej.</param>
    public void SetInfo(string id, int number, string message)
    {
        option_id = id;
        number_text.text = number.ToString();
        dialogue_option_text.text = message;
    }

    /// <summary>
    /// Obs³uguje klikniêcie na opcjê dialogow¹.
    /// </summary>
    /// <param name="eventData">Dane zdarzenia klikniêcia.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEvents.OnChoosingCertainDialogueOptionEventArgs args = new(option_id);
        GlobalEvents.FireOnChoosingCertainDialogueOption(this, args);
    }
}
