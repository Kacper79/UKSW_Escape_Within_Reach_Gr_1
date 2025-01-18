using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj�ca drzwi, kt�re mog� by� otwierane i zamykane przez gracza.
/// Implementuje interfejs IInteractable, co pozwala na interakcj� z drzwiami.
/// </summary>
public class Doors : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Czas trwania animacji otwierania lub zamykania drzwi.
    /// </summary>
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.25f;

    /// <summary>
    /// Wiadomo�� wy�wietlana w tooltipie, gdy drzwi s� otwarte.
    /// </summary>
    private string interaction_tooltip_message_opened = "Press [E] to open doors";

    /// <summary>
    /// Wiadomo�� wy�wietlana w tooltipie, gdy drzwi s� zamkni�te.
    /// </summary>
    private string interaction_tooltip_message_closed = "Press [E] to close doors";

    /// <summary>
    /// Flaga wskazuj�ca, czy drzwi s� obecnie otwarte.
    /// </summary>
    private bool is_opened = false;

    /// <summary>
    /// Flaga wskazuj�ca, czy drzwi zosta�y ju� odblokowane.
    /// </summary>
    private bool is_already_unlocked = false;

    /// <summary>
    /// Flaga wskazuj�ca, czy aktualnie trwa animacja otwierania lub zamykania drzwi.
    /// </summary>
    private bool is_during_opening_animation = false;

    /// <summary>
    /// Warto�� obrotu w stopniach u�ywana przy otwieraniu drzwi.
    /// </summary>
    private float euler_degree_for_opening_doors;

    /// <summary>
    /// Zwraca wiadomo�� do wy�wietlenia w tooltipie na podstawie stanu drzwi (otwarte lub zamkni�te).
    /// </summary>
    /// <returns>Tekst wiadomo�ci tooltipa.</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return is_opened ? interaction_tooltip_message_closed : interaction_tooltip_message_opened;
    }

    /// <summary>
    /// Obs�uguje interakcj� z drzwiami. Je�li mo�liwa jest interakcja, rozpoczyna animacj� otwierania lub zamykania drzwi.
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
    /// Dodatkowe dzia�ania podczas patrzenia na drzwi. Aktualnie nie zawiera �adnej logiki.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Animuje otwieranie lub zamykanie drzwi do podanej warto�ci obrotu (w stopniach).
    /// </summary>
    /// <param name="target_degree">Docelowy k�t obrotu drzwi.</param>
    /// <returns>Enumerator kontroluj�cy przebieg animacji.</returns>
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
            elapsed_time += Time.deltaTime; // Zwi�ksz czas o czas mi�dzy klatkami

            float currentDegree = Mathf.Lerp(start_degree, target_degree, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentDegree, transform.localEulerAngles.z);

            yield return null;
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, target_degree, transform.localEulerAngles.z);

        is_during_opening_animation = false;
    }

    /// <summary>
    /// Sprawdza, czy drzwi mog� by� otwarte lub zamkni�te. 
    /// Ustalane jest to na podstawie stanu odblokowania i trwania animacji.
    /// </summary>
    /// <returns>True, je�li interakcja jest mo�liwa; w przeciwnym razie false.</returns>
    private bool CanInteract()
    {
        if (is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        if (true) // Sprawdzenie warunk�w np. posiadania klucza lub post�pu fabularnego
        {
            is_already_unlocked = true;

            return true;
        }

        //return false;
    }
}
