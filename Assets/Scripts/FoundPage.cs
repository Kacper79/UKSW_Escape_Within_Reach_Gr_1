using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentujaca znajdywana strone w grze. 
/// Strona zawiera tytul i wiadomosc, ktore moga byc wyswietlone na UI po interakcji.
/// </summary>
public class FoundPage : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Tekst wyswietlany w podpowiedzi interakcji.
    /// </summary>
    private const string INTERACTION_TOOLTIP = "Press [E] to read the paper";

    /// <summary>
    /// Tytul wyswietlany na stronie.
    /// </summary>
    [Header("Content")]
    [SerializeField] private string title;

    /// <summary>
    /// Tresc wiadomosci wyswietlanej na stronie.
    /// </summary>
    [SerializeField] private string message;

    /// <summary>
    /// Prefab strony UI, ktory zostanie wyswietlony po interakcji.
    /// </summary>
    [Header("Spawned PageUI Prefab")]
    [SerializeField] private GameObject pageUI_prefab;

    /// <summary>
    /// Canvas w grze, na ktorym zostanie wyswietlony prefab strony.
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
    /// Metoda wywolywana po interakcji gracza z obiektem.
    /// Tworzy instancje prefabrykatu UI strony i wyswietla ja.
    /// </summary>
    void IInteractable.Interact()
    {
        GameObject created_pageUI = Instantiate(pageUI_prefab, in_game_canvas.transform);
        created_pageUI.GetComponent<FoundPageUI>().Init(title, message);

        GlobalEvents.FireOnReadingPage(this);
    }

    /// <summary>
    /// Metoda wywolywana, gdy gracz patrzy na obiekt, ale nie dodaje dodatkowych akcji.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }
}
