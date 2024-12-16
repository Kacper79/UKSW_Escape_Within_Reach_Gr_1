using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, IInteractable
{
    private string INTERACTION_TOOLTIP = "Press [E] to talk";

    [SerializeField] private DialogueNodeSO dialogue_root;
    [SerializeField] private string npc_name;

    public void AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    public string GetInteractionTooltip()
    {
        return INTERACTION_TOOLTIP + " to " + npc_name;
    }

    public void Interact()// TODO: pass in name to know which character is talking
    {
        DialogueManager.Instance.StartDialogue(dialogue_root, npc_name);
    }
}
