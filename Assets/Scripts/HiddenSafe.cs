using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa reprezentuj�ca ukryty sejf, kt�ry gracz mo�e otwiera� i zamyka�.
/// Wymaga spe�nienia warunk�w interakcji, takich jak posiadanie klucza lub post�p w historii.
/// </summary>
public class HiddenSafe : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Czas trwania animacji otwierania/zamykania drzwi sejfu w sekundach.
    /// </summary>
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.66f;

    /// <summary>
    /// Warto�� przesuni�cia na osi Z podczas otwierania sejfu.
    /// </summary>
    private const float Z_TRANSFORM_VALUE_FOR_OPENING = 3.08f;

    /// <summary>
    /// Czy sejf jest obecnie otwarty.
    /// </summary>
    private bool is_opened = false;

    /// <summary>
    /// Czy sejf zosta� ju� odblokowany (np. po spe�nieniu odpowiednich warunk�w).
    /// </summary>
    private bool is_already_unlocked = false;

    /// <summary>
    /// Czy animacja otwierania/zamykania sejfu jest w toku.
    /// </summary>
    private bool is_during_opening_animation = false;

    /// <summary>
    /// Docelowa warto�� przesuni�cia na osi Z, do kt�rej sejf ma si� otwiera�/zamyka�.
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
    /// Sprawdza, czy sejf mo�na otworzy�/zamkn��, a nast�pnie uruchamia odpowiedni� animacj�.
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
    /// Dodatkowa logika wykonywana, gdy gracz patrzy na sejf (obecnie brak dodatkowych dzia�a�).
    /// </summary>
    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    /// <summary>
    /// Wsp�rz�dna animacja otwierania lub zamykania sejfu.
    /// Sejf p�ynnie przesuwa si� na osi Z przez okre�lony czas.
    /// </summary>
    /// <param name="target_z_value">Docelowa warto�� przesuni�cia na osi Z.</param>
    private IEnumerator OpenOrCloseDoors(float target_z_value)
    {
        is_during_opening_animation = true;

        float elapsed_time = 0f;
        float start_z_value = transform.localPosition.z;

        while (elapsed_time < OPEN_AND_CLOSE_DOOR_TIME)
        {
            elapsed_time += Time.deltaTime;

            // Obliczanie bie��cej pozycji na osi Z za pomoc� interpolacji liniowej.
            float current_z_transform_value = Mathf.Lerp(start_z_value, target_z_value, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, current_z_transform_value);

            yield return null;
        }

        // Ustawienie ostatecznej pozycji, aby upewni� si�, �e sejf znajduje si� dok�adnie w docelowej pozycji.
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, target_z_value);

        is_during_opening_animation = false;
    }

    /// <summary>
    /// Sprawdza, czy sejf mo�na otworzy�/zamkn��.
    /// </summary>
    /// <returns>True, je�li sejf mo�na otworzy�/zamkn��, false w przeciwnym wypadku.</returns>
    private bool CanInteract()
    {
        // Sejf mo�na otworzy�, je�li zosta� ju� odblokowany i nie trwa animacja otwierania/zamykania.
        if (is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        // Warunek odblokowania sejfu (np. posiadanie klucza lub post�p fabularny).
        if (true) // Sprawd�, czy gracz spe�nia wymagania, np. posiada klucz.
        {
            is_already_unlocked = true;
            return true;
        }

        // Je�li warunki nie s� spe�nione, sejf pozostaje zablokowany.
        return false;
    }
}
