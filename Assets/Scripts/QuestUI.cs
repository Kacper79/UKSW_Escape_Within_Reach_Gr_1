using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Wyswietla opis zadania w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI quest_description_display;

    /// <summary>
    /// Ikona przedstawiajaca ukonczenie zadania.
    /// </summary>
    [SerializeField] private Image quest_complition_icon;

    /// <summary>
    /// Opis zadania.
    /// </summary>
    [SerializeField] private string quest_description;


    /// <summary>
    /// Aktywuje element UI i ukrywa opis zadania na poczatku
    /// </summary>
    private void OnEnable()
    {
        quest_description_display.gameObject.SetActive(false);
    }

    /// <summary>
    /// Wyswietla opis zadania, gdy kursor znajduje sie nad elementem UI
    /// </summary>
    /// <param name="eventData">Dane zdarzenia wejscia myszy</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        quest_description_display.GetComponent<TextMeshProUGUI>().text = quest_description;
        quest_description_display.gameObject.SetActive(true);
    }

    /// <summary>
    /// Ukrywa opis zadania, gdy kursor opuszcza element UI
    /// </summary>
    /// <param name="eventData">Dane zdarzenia opuszczenia obszaru przez kursor</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        quest_description_display.GetComponent<TextMeshProUGUI>().text = "";
        quest_description_display.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ustawia ikone ukonczenia zadania
    /// </summary>
    /// <param name="icon">Ikona reprezentujaca ukonczenie zadania</param>
    public void SetComplitionIcon(Sprite icon)
    {
        quest_complition_icon.sprite = icon;
    }

    /// <summary>
    /// Ustawia naglowek zadania
    /// </summary>
    /// <param name="quest_header">Naglowek zadania</param>
    public void SetQuestHeader(string quest_header)
    {
        this.GetComponentInChildren<TMP_Text>().text = quest_header;
    }
}
