using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour, IInteractable
{
    private string INTERACTION_TOOLTIP = "Press [E] to talk";

    public void AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    public string GetInteractionTooltip()
    {
        return INTERACTION_TOOLTIP;
    }

    public void Interact()
    {
        
    }
}
