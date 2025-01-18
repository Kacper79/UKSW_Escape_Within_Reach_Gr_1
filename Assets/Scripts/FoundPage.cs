using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj¹ca znajdywan¹ stronê w grze. 
/// Strona zawiera tytu³ i wiadomoœæ, które mog¹ byæ wyœwietlone na UI po interakcji.
/// </summary>
public class FoundPage : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Tekst wyœwietlany w podpowiedzi interakcji.
    /// </summary>
    private const string INTERACTION_TOOLTIP = "Press [E] to read the paper";

    /// <summary>
    /// Tytu³ wyœwietlany na stronie.
    /// </summary>
    [Header("Content")]
    [SerializeField] private string title;

    /// <summary>
    /// Treœæ wiadomoœci wyœwietlanej na stronie.
    /// </summary>
    [SerializeField] private string message;

    /// <summary>
    /// Prefab strony UI, który zostanie wyœwietlony po interakcji.
    /// </summary>
    [Header("Spawned PageUI Prefab")]
    [SerializeField] private GameObject pageUI_prefab;

    /// <summary>
    /// Canvas w grze, na którym zostanie wyœwietlony prefab strony.
    /// </summary>
    [Header("Canvas reference")]
    [SerializeField] private Canvas in_game_canvas;

    /// <summary>
    /// Zwraca tekst podpowiedzi interakcji dla gracza.
    /// </summary>
    /// <returns>Tekst podpowiedzi.</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return INTERACTION_TOOLTIP;
    }

    /// <summary>
    /// Metoda wywo³ywana po interakcji gracza z obiektem.
    /// Tworzy instancjê prefabrykatu UI strony i wyœwietla j¹.
    /// </summary>
    void IInteractable.Interact()
    {
        GameObject created_pageUI = Instantiate(pageUI_prefab, in_game_canvas.transform);
        created_pageUI.GetComponent<FoundPageUI>().Init(title, message);

        GlobalEvents.FireOnReadingPage(this);
    }

    /// <summary>
    /// Metoda wywo³ywana, gdy gracz patrzy na obiekt, ale nie dodaje dodatkowych akcji.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }
}
