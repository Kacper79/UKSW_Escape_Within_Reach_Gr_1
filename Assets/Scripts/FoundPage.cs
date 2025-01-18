using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj�ca znajdywan� stron� w grze. 
/// Strona zawiera tytu� i wiadomo��, kt�re mog� by� wy�wietlone na UI po interakcji.
/// </summary>
public class FoundPage : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Tekst wy�wietlany w podpowiedzi interakcji.
    /// </summary>
    private const string INTERACTION_TOOLTIP = "Press [E] to read the paper";

    /// <summary>
    /// Tytu� wy�wietlany na stronie.
    /// </summary>
    [Header("Content")]
    [SerializeField] private string title;

    /// <summary>
    /// Tre�� wiadomo�ci wy�wietlanej na stronie.
    /// </summary>
    [SerializeField] private string message;

    /// <summary>
    /// Prefab strony UI, kt�ry zostanie wy�wietlony po interakcji.
    /// </summary>
    [Header("Spawned PageUI Prefab")]
    [SerializeField] private GameObject pageUI_prefab;

    /// <summary>
    /// Canvas w grze, na kt�rym zostanie wy�wietlony prefab strony.
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
    /// Metoda wywo�ywana po interakcji gracza z obiektem.
    /// Tworzy instancj� prefabrykatu UI strony i wy�wietla j�.
    /// </summary>
    void IInteractable.Interact()
    {
        GameObject created_pageUI = Instantiate(pageUI_prefab, in_game_canvas.transform);
        created_pageUI.GetComponent<FoundPageUI>().Init(title, message);

        GlobalEvents.FireOnReadingPage(this);
    }

    /// <summary>
    /// Metoda wywo�ywana, gdy gracz patrzy na obiekt, ale nie dodaje dodatkowych akcji.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }
}
