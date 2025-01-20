using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentujaca drzwi, ktore moga byc otwierane i zamykane przez gracza.
/// Implementuje interfejs IInteractable, co pozwala na interakcje z drzwiami.
/// </summary>
public class Doors : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Czas trwania animacji otwierania lub zamykania drzwi.
    /// </summary>
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.25f;

    /// <summary>
    /// Wiadomosc wyswietlana w tooltipie, gdy drzwi sa otwarte.
    /// </summary>
    private string interaction_tooltip_message_opened = "Press [E] to open doors";

    /// <summary>
    /// Wiadomosc wyswietlana w tooltipie, gdy drzwi sa zamkniete.
    /// </summary>
    private string interaction_tooltip_message_closed = "Press [E] to close doors";

    /// <summary>
    /// Flaga wskazujaca, czy drzwi sa obecnie otwarte.
    /// </summary>
    private bool is_opened = false;

    /// <summary>
    /// Flaga wskazujaca, czy drzwi zostaly juz odblokowane.
    /// </summary>
    private bool is_already_unlocked = false;

    /// <summary>
    /// Flaga wskazujaca, czy aktualnie trwa animacja otwierania lub zamykania drzwi.
    /// </summary>
    private bool is_during_opening_animation = false;

    /// <summary>
    /// Wartosc obrotu w stopniach uzywana przy otwieraniu drzwi.
    /// </summary>
    private float euler_degree_for_opening_doors;

    /// <summary>
    /// Zwraca wiadomosc do wyswietlenia w tooltipie na podstawie stanu drzwi (otwarte lub zamkniete).
    /// </summary>
    /// <returns>Tekst wiadomosci tooltipa.</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return is_opened ? interaction_tooltip_message_closed : interaction_tooltip_message_opened;
    }

    /// <summary>
    /// Obsluguje interakcje z drzwiami. Jesli mozliwa jest interakcja, rozpoczyna animacje otwierania lub zamykania drzwi.
    /// </summary>
    void IInteractable.Interact()
    {
        if (CanInteract())
        {
            euler_degree_for_opening_doors = is_opened ? 0.0f : 90.0f;
            StartCoroutine(OpenOrCloseDoors(euler_degree_for_opening_doors));
            is_opened = !is_opened;
        }
    }

    /// <summary>
    /// Dodatkowe dzialania podczas patrzenia na drzwi. Aktualnie nie zawiera ¿adnej logiki.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Animuje otwieranie lub zamykanie drzwi do podanej wartosci obrotu (w stopniach).
    /// </summary>
    /// <param name="target_degree">Docelowy kat obrotu drzwi.</param>
    /// <returns>Enumerator kontrolujacy przebieg animacji.</returns>
    private IEnumerator OpenOrCloseDoors(float target_degree)
    {
        is_during_opening_animation = true;

        float elapsed_time = 0f;
        float start_degree = transform.localEulerAngles.y;

        if (start_degree > 180f)
        {
            start_degree -= 360f;
        }

        while (elapsed_time < OPEN_AND_CLOSE_DOOR_TIME)
        {
            elapsed_time += Time.deltaTime; // Zwieksz czas o czas miedzy klatkami

            float currentDegree = Mathf.Lerp(start_degree, target_degree, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentDegree, transform.localEulerAngles.z);

            yield return null;
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, target_degree, transform.localEulerAngles.z);

        is_during_opening_animation = false;
    }

    /// <summary>
    /// Sprawdza, czy drzwi moga byc otwarte lub zamkniete. 
    /// Ustalane jest to na podstawie stanu odblokowania i trwania animacji.
    /// </summary>
    /// <returns>True, jesli interakcja jest mozliwa; w przeciwnym razie false.</returns>
    private bool CanInteract()
    {
        if (is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        if (true) // Sprawdzenie warunkow np. posiadania klucza lub postepu fabularnego
        {
            is_already_unlocked = true;

            return true;
        }

        //return false;
    }
}
