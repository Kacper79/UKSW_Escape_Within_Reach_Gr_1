using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj�ca obiekt, z kt�rym mo�na rozmawia�.
/// Implementuje interfejs IInteractable, co pozwala na interakcj� z NPC.
/// </summary>
public class Talkable : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Wiadomo�� wy�wietlana w tooltipie podczas patrzenia na obiekt.
    /// </summary>
    private string INTERACTION_TOOLTIP = "Press [E] to talk";

    /// <summary>
    /// G��wny w�ze� dialogu, kt�ry jest powi�zany z tym NPC.
    /// </summary>
    [SerializeField] private DialogueNodeSO dialogue_root;

    /// <summary>
    /// Nazwa NPC, z kt�rym mo�na rozmawia�.
    /// </summary>
    [SerializeField] private string npc_name;

    /// <summary>
    /// Dodatkowe dzia�ania wykonywane podczas patrzenia na obiekt. 
    /// Aktualnie brak implementacji.
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Zwraca wiadomo�� tooltipa do wy�wietlenia podczas patrzenia na obiekt.
    /// Uwzgl�dnia nazw� NPC.
    /// </summary>
    /// <returns>Tekst tooltipa z nazw� NPC.</returns>
    public string GetInteractionTooltip()
    {
        return INTERACTION_TOOLTIP + " to " + npc_name;
    }

    /// <summary>
    /// Rozpoczyna interakcj� z NPC, uruchamiaj�c dialog za pomoc� DialogueManager.
    /// </summary>
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue_root, npc_name);
    }
}

