using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj�ca kamie� do wydobycia, kt�ry gracz mo�e kopa�.
/// Po zniszczeniu kamienia gracz otrzymuje z�oto, a obiekt zostaje zniszczony.
/// </summary>
public class RockToMine : MonoBehaviour, IInteractable
{
    private const int GOLD_FOR_COMPLETING = 20; // Ilo�� z�ota za zniszczenie kamienia
    private const int MAX_HP = 10; // Maksymalna ilo�� HP kamienia

    private MiningProgressUI mining_progress_UI; // UI post�pu wydobycia

    private string interaction_tooltip_message = "Press [E] to mine"; // Wiadomo��, kt�ra pojawia si� przy interakcji

    private int hp = MAX_HP; // Aktualne HP kamienia

    /// <summary>
    /// Inicjalizacja obiektu. Znajduje UI post�pu wydobycia.
    /// </summary>
    private void Awake()
    {
        mining_progress_UI = FindObjectOfType<MiningProgressUI>();
    }

    /// <summary>
    /// Zwraca wiadomo��, kt�ra wy�wietla si� graczowi, kiedy patrzy na kamie�.
    /// </summary>
    /// <returns>Wiadomo�� z instrukcj� interakcji</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return interaction_tooltip_message;
    }

    /// <summary>
    /// Wykonuje akcj� kopania kamienia, zmniejszaj�c jego HP i aktualizuj�c grafik�.
    /// Sprawdza, czy kamie� zosta� ca�kowicie zniszczony.
    /// </summary>
    void IInteractable.Interact()
    {
        if (CanInteract()) // Je�li mo�na wykona� interakcj�
        {
            hp -= 1; // Zmniejsza HP kamienia
            ChangeGraphic(); // Zmienia grafik� kamienia (do zrobienia)
            CheckIfDone(); // Sprawdza, czy kamie� zosta� zniszczony
        }
    }

    /// <summary>
    /// Umo�liwia pokazanie UI post�pu wydobycia, aktualizuje pasek post�pu.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        mining_progress_UI.EnableUI(); // W��cza UI post�pu
        mining_progress_UI.PassValuesToProgressbar(MAX_HP - hp, MAX_HP); // Ustawia pasek post�pu
    }

    /// <summary>
    /// Sprawdza, czy interakcja jest mo�liwa. Na razie zawsze zwraca true.
    /// Mo�e by� przydatne, by w przysz�o�ci doda� ograniczenia (np. wymaganie narz�dzia).
    /// </summary>
    /// <returns>True, je�li interakcja jest mo�liwa</returns>
    private bool CanInteract()
    {
        return true; // Mo�liwo�� interakcji, mo�e by� rozszerzona w przysz�o�ci
    }

    /// <summary>
    /// Zmienia grafik� kamienia w zale�no�ci od jego HP.
    /// </summary>
    private void ChangeGraphic()
    {
        //TODO: Zmiana grafiki w zale�no�ci od HP kamienia
    }

    /// <summary>
    /// Sprawdza, czy kamie� zosta� zniszczony. Je�li tak, usuwa go i przyznaje z�oto.
    /// </summary>
    private void CheckIfDone()
    {
        if (hp <= 0)
        {
            Debug.Log(GOLD_FOR_COMPLETING + " gold for this rock"); // Debugowanie informacji o z�ocie
            // TODO: Dodanie z�ota do zasob�w gracza

            Destroy(this.gameObject); // Niszczy obiekt kamienia

            GlobalEvents.FireOnDestroingRock(this); // Informuje o zniszczeniu kamienia (je�li s� zdefiniowane odpowiednie zdarzenia globalne)
        }
    }
}
