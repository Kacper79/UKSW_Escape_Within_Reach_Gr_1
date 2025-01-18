using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj¹ca drzwi, które mog¹ byæ otwierane i zamykane przez gracza.
/// Implementuje interfejs IInteractable, co pozwala na interakcjê z drzwiami.
/// </summary>
public class Doors : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Czas trwania animacji otwierania lub zamykania drzwi.
    /// </summary>
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.25f;

    /// <summary>
    /// Wiadomoœæ wyœwietlana w tooltipie, gdy drzwi s¹ otwarte.
    /// </summary>
    private string interaction_tooltip_message_opened = "Press [E] to open doors";

    /// <summary>
    /// Wiadomoœæ wyœwietlana w tooltipie, gdy drzwi s¹ zamkniête.
    /// </summary>
    private string interaction_tooltip_message_closed = "Press [E] to close doors";

    /// <summary>
    /// Flaga wskazuj¹ca, czy drzwi s¹ obecnie otwarte.
    /// </summary>
    private bool is_opened = false;

    /// <summary>
    /// Flaga wskazuj¹ca, czy drzwi zosta³y ju¿ odblokowane.
    /// </summary>
    private bool is_already_unlocked = false;

    /// <summary>
    /// Flaga wskazuj¹ca, czy aktualnie trwa animacja otwierania lub zamykania drzwi.
    /// </summary>
    private bool is_during_opening_animation = false;

    /// <summary>
    /// Wartoœæ obrotu w stopniach u¿ywana przy otwieraniu drzwi.
    /// </summary>
    private float euler_degree_for_opening_doors;

    /// <summary>
    /// Zwraca wiadomoœæ do wyœwietlenia w tooltipie na podstawie stanu drzwi (otwarte lub zamkniête).
    /// </summary>
    /// <returns>Tekst wiadomoœci tooltipa.</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return is_opened ? interaction_tooltip_message_closed : interaction_tooltip_message_opened;
    }

    /// <summary>
    /// Obs³uguje interakcjê z drzwiami. Jeœli mo¿liwa jest interakcja, rozpoczyna animacjê otwierania lub zamykania drzwi.
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
    /// Dodatkowe dzia³ania podczas patrzenia na drzwi. Aktualnie nie zawiera ¿adnej logiki.
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Animuje otwieranie lub zamykanie drzwi do podanej wartoœci obrotu (w stopniach).
    /// </summary>
    /// <param name="target_degree">Docelowy k¹t obrotu drzwi.</param>
    /// <returns>Enumerator kontroluj¹cy przebieg animacji.</returns>
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
            elapsed_time += Time.deltaTime; // Zwiêksz czas o czas miêdzy klatkami

            float currentDegree = Mathf.Lerp(start_degree, target_degree, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentDegree, transform.localEulerAngles.z);

            yield return null;
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, target_degree, transform.localEulerAngles.z);

        is_during_opening_animation = false;
    }

    /// <summary>
    /// Sprawdza, czy drzwi mog¹ byæ otwarte lub zamkniête. 
    /// Ustalane jest to na podstawie stanu odblokowania i trwania animacji.
    /// </summary>
    /// <returns>True, jeœli interakcja jest mo¿liwa; w przeciwnym razie false.</returns>
    private bool CanInteract()
    {
        if (is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        if (true) // Sprawdzenie warunków np. posiadania klucza lub postêpu fabularnego
        {
            is_already_unlocked = true;

            return true;
        }

        //return false;
    }
}
