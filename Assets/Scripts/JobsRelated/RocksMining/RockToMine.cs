using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentujaca kamien do wydobycia, ktory gracz moze kopac.
/// Po zniszczeniu kamienia gracz otrzymuje zloto, a obiekt zostaje zniszczony.
/// </summary>
public class RockToMine : MonoBehaviour, IInteractable
{
    private const int GOLD_FOR_COMPLETING = 20; // Ilosc zlota za zniszczenie kamienia
    private const int MAX_HP = 10; // Maksymalna ilosc HP kamienia

    private MiningProgressUI mining_progress_UI; // UI postepu wydobycia

    private string interaction_tooltip_message = "Press [E] to mine"; // Wiadomosc, ktora pojawia sie przy interakcji

    private int hp = MAX_HP; // Aktualne HP kamienia
    public int rock_instance;

    /// <summary>
    /// Inicjalizacja obiektu. Znajduje UI postepu wydobycia.
    /// </summary>
    private void Awake()
    {
        mining_progress_UI = FindObjectOfType<MiningProgressUI>();
    }

    /// <summary>
    /// Zwraca wiadomosc, ktora wyswietla sie graczowi, kiedy patrzy na kamien.
    /// </summary>
    /// <returns>Wiadomosc z instrukcja interakcji</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return interaction_tooltip_message;
    }

    /// <summary>
    /// Wykonuje akcje kopania kamienia, zmniejszajac jego HP i aktualizujac grafike.
    /// Sprawdza, czy kamien zostal calkowicie zniszczony.
    /// </summary>
    void IInteractable.Interact()
    {
        if (CanInteract()) // Jesli mozna wykonac interakcje
        {
            hp -= 1; // Zmniejsza HP kamienia
            ChangeGraphic(); // Zmienia grafike kamienia (do zrobienia)
            CheckIfDone(); // Sprawdza, czy kamien zostal zniszczony
        }
    }

    /// <summary>
    /// Umozliwia pokazanie UI postepu wydobycia, aktualizuje pasek postepu.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        mining_progress_UI.EnableUI(); // Wlacza UI postepu
        mining_progress_UI.PassValuesToProgressbar(MAX_HP - hp, MAX_HP); // Ustawia pasek postepu
    }

    /// <summary>
    /// Sprawdza, czy interakcja jest mozliwa. Na razie zawsze zwraca true.
    /// Moze byc przydatne, by w przyszlosci dodaæ ograniczenia (np. wymaganie narzedzia).
    /// </summary>
    /// <returns>True, jesli interakcja jest mozliwa</returns>
    private bool CanInteract()
    {
        return true; // Mozliwosc interakcji, moze byc rozszerzona w przyszlosci
    }

    /// <summary>
    /// Zmienia grafike kamienia w zaleznosci od jego HP.
    /// </summary>
    private void ChangeGraphic()
    {
        //TODO: Zmiana grafiki w zaleznosci od HP kamienia
    }

    /// <summary>
    /// Sprawdza, czy kamien zostal zniszczony. Jesli tak, usuwa go i przyznaje zloto.
    /// </summary>
    private void CheckIfDone()
    {
        if (hp <= 0)
        {
            Debug.Log(GOLD_FOR_COMPLETING + " gold for this rock"); // Debugowanie informacji o zlocie
            GlobalEvents.FireOnDestroingRock(this, new(rock_instance));
            Destroy(gameObject);
        }
    }
}
