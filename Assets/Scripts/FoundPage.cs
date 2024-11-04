using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundPage : MonoBehaviour, IInteractable
{
    private const string INTERACTION_TOOLTIP = "Press [E] to read the paper";

    [Header("Content")]
    [SerializeField] private string title;
    [SerializeField] private string message;

    [Header("Spawned PageUI Prefab")]
    [SerializeField] private GameObject pageUI_prefab;

    [Header("Canvas reference")]
    [SerializeField] private Canvas in_game_canvas;

    string IInteractable.GetInteractionTooltip()
    {
        return INTERACTION_TOOLTIP;
    }

    void IInteractable.Interact()
    {
        GameObject created_pageUI = Instantiate(pageUI_prefab, in_game_canvas.transform);
        created_pageUI.GetComponent<FoundPageUI>().Init(title, message);

        GlobalEvents.FireOnReadingPage(this);
    }
}
    
