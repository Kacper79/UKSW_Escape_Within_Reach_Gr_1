using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj¹ca kamieñ do wydobycia, który gracz mo¿e kopaæ.
/// Po zniszczeniu kamienia gracz otrzymuje z³oto, a obiekt zostaje zniszczony.
/// </summary>
public class RockToMine : MonoBehaviour, IInteractable
{
    private const int GOLD_FOR_COMPLETING = 20; // Iloœæ z³ota za zniszczenie kamienia
    private const int MAX_HP = 10; // Maksymalna iloœæ HP kamienia

    private MiningProgressUI mining_progress_UI; // UI postêpu wydobycia

    private string interaction_tooltip_message = "Press [E] to mine"; // Wiadomoœæ, która pojawia siê przy interakcji

    private int hp = MAX_HP; // Aktualne HP kamienia

    /// <summary>
    /// Inicjalizacja obiektu. Znajduje UI postêpu wydobycia.
    /// </summary>
    private void Awake()
    {
        mining_progress_UI = FindObjectOfType<MiningProgressUI>();
    }

    /// <summary>
    /// Zwraca wiadomoœæ, która wyœwietla siê graczowi, kiedy patrzy na kamieñ.
    /// </summary>
    /// <returns>Wiadomoœæ z instrukcj¹ interakcji</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return interaction_tooltip_message;
    }

    /// <summary>
    /// Wykonuje akcjê kopania kamienia, zmniejszaj¹c jego HP i aktualizuj¹c grafikê.
    /// Sprawdza, czy kamieñ zosta³ ca³kowicie zniszczony.
    /// </summary>
    void IInteractable.Interact()
    {
        if (CanInteract()) // Jeœli mo¿na wykonaæ interakcjê
        {
            hp -= 1; // Zmniejsza HP kamienia
            ChangeGraphic(); // Zmienia grafikê kamienia (do zrobienia)
            CheckIfDone(); // Sprawdza, czy kamieñ zosta³ zniszczony
        }
    }

    /// <summary>
    /// Umo¿liwia pokazanie UI postêpu wydobycia, aktualizuje pasek postêpu.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        mining_progress_UI.EnableUI(); // W³¹cza UI postêpu
        mining_progress_UI.PassValuesToProgressbar(MAX_HP - hp, MAX_HP); // Ustawia pasek postêpu
    }

    /// <summary>
    /// Sprawdza, czy interakcja jest mo¿liwa. Na razie zawsze zwraca true.
    /// Mo¿e byæ przydatne, by w przysz³oœci dodaæ ograniczenia (np. wymaganie narzêdzia).
    /// </summary>
    /// <returns>True, jeœli interakcja jest mo¿liwa</returns>
    private bool CanInteract()
    {
        return true; // Mo¿liwoœæ interakcji, mo¿e byæ rozszerzona w przysz³oœci
    }

    /// <summary>
    /// Zmienia grafikê kamienia w zale¿noœci od jego HP.
    /// </summary>
    private void ChangeGraphic()
    {
        //TODO: Zmiana grafiki w zale¿noœci od HP kamienia
    }

    /// <summary>
    /// Sprawdza, czy kamieñ zosta³ zniszczony. Jeœli tak, usuwa go i przyznaje z³oto.
    /// </summary>
    private void CheckIfDone()
    {
        if (hp <= 0)
        {
            Debug.Log(GOLD_FOR_COMPLETING + " gold for this rock"); // Debugowanie informacji o z³ocie
            // TODO: Dodanie z³ota do zasobów gracza

            Destroy(this.gameObject); // Niszczy obiekt kamienia

            GlobalEvents.FireOnDestroingRock(this); // Informuje o zniszczeniu kamienia (jeœli s¹ zdefiniowane odpowiednie zdarzenia globalne)
        }
    }
}
