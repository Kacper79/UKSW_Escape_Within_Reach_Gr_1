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
    /// Wyœwietla opis zadania w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI quest_description_display;

    /// <summary>
    /// Ikona przedstawiaj¹ca ukoñczenie zadania.
    /// </summary>
    [SerializeField] private Image quest_complition_icon;

    /// <summary>
    /// Opis zadania.
    /// </summary>
    [SerializeField] private string quest_description;


    /// <summary>
    /// Aktywuje element UI i ukrywa opis zadania na pocz¹tku
    /// </summary>
    private void OnEnable()
    {
        quest_description_display.gameObject.SetActive(false);
    }

    /// <summary>
    /// Wyœwietla opis zadania, gdy kursor znajduje siê nad elementem UI
    /// </summary>
    /// <param name="eventData">Dane zdarzenia wejœcia myszy</param>
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
    /// Ustawia ikonê ukoñczenia zadania
    /// </summary>
    /// <param name="icon">Ikona reprezentuj¹ca ukoñczenie zadania</param>
    public void SetComplitionIcon(Sprite icon)
    {
        quest_complition_icon.sprite = icon;
    }

    /// <summary>
    /// Ustawia nag³ówek zadania
    /// </summary>
    /// <param name="quest_header">Nag³ówek zadania</param>
    public void SetQuestHeader(string quest_header)
    {
        this.GetComponentInChildren<TMP_Text>().text = quest_header;
    }
}
