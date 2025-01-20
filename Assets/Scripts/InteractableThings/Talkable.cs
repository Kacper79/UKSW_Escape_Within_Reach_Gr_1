using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentujaca obiekt, z ktorym mozna rozmawiac.
/// Implementuje interfejs IInteractable, co pozwala na interakcje z NPC.
/// </summary>
public class Talkable : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Wiadomosc wyswietlana w tooltipie podczas patrzenia na obiekt.
    /// </summary>
    private string INTERACTION_TOOLTIP = "Press [E] to talk";

    /// <summary>
    /// Glowny wezel dialogu, ktory jest powiazany z tym NPC.
    /// </summary>
    [SerializeField] private DialogueNodeSO dialogue_root;

    /// <summary>
    /// Nazwa NPC, z ktorym mozna rozmawiac.
    /// </summary>
    [SerializeField] private string npc_name;

    /// <summary>
    /// Dodatkowe dzialania wykonywane podczas patrzenia na obiekt. 
    /// Aktualnie brak implementacji.
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Zwraca wiadomosc tooltipa do wyswietlenia podczas patrzenia na obiekt.
    /// Uwzglednia nazwe NPC.
    /// </summary>
    /// <returns>Tekst tooltipa z nazwe NPC.</returns>
    public string GetInteractionTooltip()
    {
        return INTERACTION_TOOLTIP + " to " + npc_name;
    }

    /// <summary>
    /// Rozpoczyna interakcje z NPC, uruchamiajac dialog za pomoca DialogueManager.
    /// </summary>
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue_root, npc_name);
    }
}

