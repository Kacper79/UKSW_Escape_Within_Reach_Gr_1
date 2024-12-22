using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TextMeshProUGUI quest_description_display;

    [SerializeField] private string quest_description;

    private void OnEnable()
    {
        quest_description_display.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        quest_description_display.GetComponent<TextMeshProUGUI>().text = quest_description;
        quest_description_display.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        quest_description_display.GetComponent<TextMeshProUGUI>().text = "";
        quest_description_display.gameObject.SetActive(false);
    }
}
