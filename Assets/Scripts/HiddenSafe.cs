using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentujaca ukryty sejf, ktory gracz moze otwierac i zamykac.
/// Wymaga spelnienia warunkow interakcji, takich jak posiadanie klucza lub postep w historii.
/// </summary>
public class HiddenSafe : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Czas trwania animacji otwierania/zamykania drzwi sejfu w sekundach.
    /// </summary>
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.66f;

    /// <summary>
    /// Wartosc przesuniecia na osi Z podczas otwierania sejfu.
    /// </summary>
    private const float Z_TRANSFORM_VALUE_FOR_OPENING = 3.08f;

    /// <summary>
    /// Czy sejf jest obecnie otwarty.
    /// </summary>
    private bool is_opened = false;

    /// <summary>
    /// Czy sejf zostal juz odblokowany (np. po spelnieniu odpowiednich warunkow).
    /// </summary>
    private bool is_already_unlocked = false;

    /// <summary>
    /// Czy animacja otwierania/zamykania sejfu jest w toku.
    /// </summary>
    private bool is_during_opening_animation = false;

    /// <summary>
    /// Docelowa wartosc przesuniecia na osi Z, do ktorej sejf ma sie otwierac/zamykac.
    /// </summary>
    private float z_transform_value;

    /// <summary>
    /// Zwraca tekst podpowiedzi dla gracza, gdy patrzy na sejf.
    /// </summary>
    /// <returns>Tekst podpowiedzi "Gaba".</returns>
    string IInteractable.GetInteractionTooltip()
    {
        return "Gaba";
    }

    /// <summary>
    /// Metoda interakcji z sejfem (otwieranie/zamykanie).
    /// Sprawdza, czy sejf mozna otworzyc/zamknac, a nastepnie uruchamia odpowiednia animacje.
    /// </summary>
    void IInteractable.Interact()
    {
        if (CanInteract())
        {
            z_transform_value = is_opened ? 0.0f : Z_TRANSFORM_VALUE_FOR_OPENING;
            StartCoroutine(OpenOrCloseDoors(z_transform_value));
            is_opened = !is_opened;
        }
    }

    /// <summary>
    /// Dodatkowa logika wykonywana, gdy gracz patrzy na sejf (obecnie brak dodatkowych dzialan).
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Wspolrzedna animacja otwierania lub zamykania sejfu.
    /// Sejf plynnie przesuwa sie na osi Z przez okreslony czas.
    /// </summary>
    /// <param name="target_z_value">Docelowa wartosc przesuniecia na osi Z.</param>
    private IEnumerator OpenOrCloseDoors(float target_z_value)
    {
        is_during_opening_animation = true;

        float elapsed_time = 0f;
        float start_z_value = transform.localPosition.z;

        while (elapsed_time < OPEN_AND_CLOSE_DOOR_TIME)
        {
            elapsed_time += Time.deltaTime;

            // Obliczanie biezacej pozycji na osi Z za pomoca interpolacji liniowej.
            float current_z_transform_value = Mathf.Lerp(start_z_value, target_z_value, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, current_z_transform_value);

            yield return null;
        }

        // Ustawienie ostatecznej pozycji, aby upewnic sie, ze sejf znajduje sie dokladnie w docelowej pozycji.
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, target_z_value);

        is_during_opening_animation = false;
    }

    /// <summary>
    /// Sprawdza, czy sejf mozna otworzyc/zamknac.
    /// </summary>
    /// <returns>True, jesli sejf mozna otworzyc/zamknac, false w przeciwnym wypadku.</returns>
    private bool CanInteract()
    {
        // Sejf mozna otworzyc, jesli zostal juz odblokowany i nie trwa animacja otwierania/zamykania.
        if (is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        // Warunek odblokowania sejfu (np. posiadanie klucza lub postep fabularny).
        if (true) // Sprawdz, czy gracz spelnia wymagania, np. posiada klucz.
        {
            is_already_unlocked = true;
            return true;
        }

        // Jesli warunki nie sa spelnione, sejf pozostaje zablokowany.
        return false;
    }
}
