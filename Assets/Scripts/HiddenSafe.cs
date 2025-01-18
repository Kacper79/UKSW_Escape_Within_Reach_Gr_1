using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj¹ca ukryty sejf, który gracz mo¿e otwieraæ i zamykaæ.
/// Wymaga spe³nienia warunków interakcji, takich jak posiadanie klucza lub postêp w historii.
/// </summary>
public class HiddenSafe : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Czas trwania animacji otwierania/zamykania drzwi sejfu w sekundach.
    /// </summary>
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.66f;

    /// <summary>
    /// Wartoœæ przesuniêcia na osi Z podczas otwierania sejfu.
    /// </summary>
    private const float Z_TRANSFORM_VALUE_FOR_OPENING = 3.08f;

    /// <summary>
    /// Czy sejf jest obecnie otwarty.
    /// </summary>
    private bool is_opened = false;

    /// <summary>
    /// Czy sejf zosta³ ju¿ odblokowany (np. po spe³nieniu odpowiednich warunków).
    /// </summary>
    private bool is_already_unlocked = false;

    /// <summary>
    /// Czy animacja otwierania/zamykania sejfu jest w toku.
    /// </summary>
    private bool is_during_opening_animation = false;

    /// <summary>
    /// Docelowa wartoœæ przesuniêcia na osi Z, do której sejf ma siê otwieraæ/zamykaæ.
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
    /// Sprawdza, czy sejf mo¿na otworzyæ/zamkn¹æ, a nastêpnie uruchamia odpowiedni¹ animacjê.
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
    /// Dodatkowa logika wykonywana, gdy gracz patrzy na sejf (obecnie brak dodatkowych dzia³añ).
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Wspó³rzêdna animacja otwierania lub zamykania sejfu.
    /// Sejf p³ynnie przesuwa siê na osi Z przez okreœlony czas.
    /// </summary>
    /// <param name="target_z_value">Docelowa wartoœæ przesuniêcia na osi Z.</param>
    private IEnumerator OpenOrCloseDoors(float target_z_value)
    {
        is_during_opening_animation = true;

        float elapsed_time = 0f;
        float start_z_value = transform.localPosition.z;

        while (elapsed_time < OPEN_AND_CLOSE_DOOR_TIME)
        {
            elapsed_time += Time.deltaTime;

            // Obliczanie bie¿¹cej pozycji na osi Z za pomoc¹ interpolacji liniowej.
            float current_z_transform_value = Mathf.Lerp(start_z_value, target_z_value, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, current_z_transform_value);

            yield return null;
        }

        // Ustawienie ostatecznej pozycji, aby upewniæ siê, ¿e sejf znajduje siê dok³adnie w docelowej pozycji.
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, target_z_value);

        is_during_opening_animation = false;
    }

    /// <summary>
    /// Sprawdza, czy sejf mo¿na otworzyæ/zamkn¹æ.
    /// </summary>
    /// <returns>True, jeœli sejf mo¿na otworzyæ/zamkn¹æ, false w przeciwnym wypadku.</returns>
    private bool CanInteract()
    {
        // Sejf mo¿na otworzyæ, jeœli zosta³ ju¿ odblokowany i nie trwa animacja otwierania/zamykania.
        if (is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        // Warunek odblokowania sejfu (np. posiadanie klucza lub postêp fabularny).
        if (true) // SprawdŸ, czy gracz spe³nia wymagania, np. posiada klucz.
        {
            is_already_unlocked = true;
            return true;
        }

        // Jeœli warunki nie s¹ spe³nione, sejf pozostaje zablokowany.
        return false;
    }
}
