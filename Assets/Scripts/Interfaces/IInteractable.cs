using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();

    public string GetInteractionTooltip();

    public void AdditionalStuffWhenLookingAtInteractable();
}
