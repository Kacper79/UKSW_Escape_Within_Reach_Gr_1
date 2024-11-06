using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, IInteractable
{
    private string interaction_tooltip_message_opened = "Press [E] to open doors";
    private string interaction_tooltip_message_closed = "Press [E] to close doors";

    private bool is_opened = false;
    private bool is_already_unlocked = false;

    private float euler_degree_for_opening_doors;

    string IInteractable.GetInteractionTooltip()
    {
        return is_opened ? interaction_tooltip_message_closed : interaction_tooltip_message_opened;
    }

    void IInteractable.Interact()
    {
        if(CanInteract())
        {
            euler_degree_for_opening_doors = is_opened ? 0.0f : 90.0f;
            transform.eulerAngles = new(transform.eulerAngles.x, euler_degree_for_opening_doors, transform.eulerAngles.z);//consume key or not if you have to, logic later
            is_opened = !is_opened;
        }
    }

    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    private bool CanInteract()
    {
        if(is_already_unlocked)
        {
            return true;
        }

        if(true)//if has key or story progressed enough return true...
        {
            is_already_unlocked = true;

            return true;
        }

        //return false;
    }
}
