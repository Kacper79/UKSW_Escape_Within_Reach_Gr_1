using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueOption : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI number_text;
    [SerializeField] private TextMeshProUGUI dialogue_option_text;

    private string option_id;

    public void SetInfo(string id, int number, string message)
    {
        option_id = id;
        number_text.text = number.ToString();
        dialogue_option_text.text = message;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEvents.OnChoosingCertainDialogueOptionEventArgs args = new(option_id);
        GlobalEvents.FireOnChoosingCertainDialogueOption(this, args);
    }
}
