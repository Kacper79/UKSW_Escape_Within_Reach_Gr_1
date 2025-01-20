using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfejs dla obiektow, ktore moga byc interaktywne w grze.
/// Zawiera metody, ktore umozliwiaja interakcje, wyswietlanie podpowiedzi interakcji oraz dodatkowe akcje przy patrzeniu na obiekt.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Metoda wykonywana podczas interakcji z obiektem.
    /// </summary>
    public void Interact();

    /// <summary>
    /// Zwraca tekst podpowiedzi dla interakcji z obiektem.
    /// </summary>
    /// <returns>Tekst do wyswietlenia jako podpowiedz dla gracza (np. "Naciœnij [E], aby wykonac interakcje").</returns>
    public string GetInteractionTooltip();

    /// <summary>
    /// Dodatkowe akcje wykonywane, gdy gracz patrzy na obiekt interakcyjny.
    /// Moze to obejmowac np. wyswietlanie paska postepu lub podswietlanie obiektu.
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable();
}
