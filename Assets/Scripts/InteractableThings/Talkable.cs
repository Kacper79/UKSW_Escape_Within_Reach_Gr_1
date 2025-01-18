using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj¹ca obiekt, z którym mo¿na rozmawiaæ.
/// Implementuje interfejs IInteractable, co pozwala na interakcjê z NPC.
/// </summary>
public class Talkable : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Wiadomoœæ wyœwietlana w tooltipie podczas patrzenia na obiekt.
    /// </summary>
    private string INTERACTION_TOOLTIP = "Press [E] to talk";

    /// <summary>
    /// G³ówny wêze³ dialogu, który jest powi¹zany z tym NPC.
    /// </summary>
    [SerializeField] private DialogueNodeSO dialogue_root;

    /// <summary>
    /// Nazwa NPC, z którym mo¿na rozmawiaæ.
    /// </summary>
    [SerializeField] private string npc_name;

    /// <summary>
    /// Dodatkowe dzia³ania wykonywane podczas patrzenia na obiekt. 
    /// Aktualnie brak implementacji.
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Zwraca wiadomoœæ tooltipa do wyœwietlenia podczas patrzenia na obiekt.
    /// Uwzglêdnia nazwê NPC.
    /// </summary>
    /// <returns>Tekst tooltipa z nazw¹ NPC.</returns>
    public string GetInteractionTooltip()
    {
        return INTERACTION_TOOLTIP + " to " + npc_name;
    }

    /// <summary>
    /// Rozpoczyna interakcjê z NPC, uruchamiaj¹c dialog za pomoc¹ DialogueManager.
    /// </summary>
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue_root, npc_name);
    }
}

