using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfejs dla obiektów, które mog¹ byæ interaktywne w grze.
/// Zawiera metody, które umo¿liwiaj¹ interakcjê, wyœwietlanie podpowiedzi interakcji oraz dodatkowe akcje przy patrzeniu na obiekt.
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
    /// <returns>Tekst do wyœwietlenia jako podpowiedŸ dla gracza (np. "Naciœnij [E], aby wykonaæ interakcjê").</returns>
    public string GetInteractionTooltip();

    /// <summary>
    /// Dodatkowe akcje wykonywane, gdy gracz patrzy na obiekt interakcyjny.
    /// Mo¿e to obejmowaæ np. wyœwietlanie paska postêpu lub podœwietlanie obiektu.
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable();
}
