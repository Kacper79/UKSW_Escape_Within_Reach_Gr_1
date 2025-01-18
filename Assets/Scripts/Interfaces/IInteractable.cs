using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfejs dla obiekt�w, kt�re mog� by� interaktywne w grze.
/// Zawiera metody, kt�re umo�liwiaj� interakcj�, wy�wietlanie podpowiedzi interakcji oraz dodatkowe akcje przy patrzeniu na obiekt.
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
    /// <returns>Tekst do wy�wietlenia jako podpowied� dla gracza (np. "Naci�nij [E], aby wykona� interakcj�").</returns>
    public string GetInteractionTooltip();

    /// <summary>
    /// Dodatkowe akcje wykonywane, gdy gracz patrzy na obiekt interakcyjny.
    /// Mo�e to obejmowa� np. wy�wietlanie paska post�pu lub pod�wietlanie obiektu.
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable();
}
