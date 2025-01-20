using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Klasa odpowiedzialna za zarzadzanie interakcjami gracza z obiektami w grze.
/// Obsluguje detekcje obiektow interakcji oraz wykonanie interakcji po nacisnieciu przycisku przez gracza.
/// </summary>
public class PlayerInteractionController : MonoBehaviour
{
    /// <summary>
    /// Detektor obiektow interakcji w grze.
    /// </summary>
    [SerializeField] private InteractableTargetsDetector interactable_targets_detector;

    /// <summary>
    /// Komponent wejscia gracza (PlayerInput).
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Flaga wskazujaca, czy gracz moze przeprowadzaæ interakcje.
    /// </summary>
    private bool can_interact = true;

    /// <summary>
    /// Aktualizacja logiki interakcji, sprawdzanie detektora obiektow.
    /// </summary>
    private void Update()
    {
        // Sprawdzenie, czy detektor interakcji jest przypisany
        if (interactable_targets_detector == null)
        {
            Debug.Log("O chuj z tym nullem chodzi?");
        }
    }

    /// <summary>
    /// Subskrybuje zdarzenia wejscia gracza.
    /// </summary>
    private void OnEnable()
    {
        player_input.InteractPlayerInput.Enable();

        // Rejestracja metody obs³ugujacej interakcje
        player_input.InteractPlayerInput.Interact.performed += InteractPerformed;
    }

    /// <summary>
    /// Odsubskrybowuje zdarzenia, gdy obiekt jest dezaktywowany.
    /// </summary>
    private void OnDisable()
    {
        player_input.InteractPlayerInput.Disable();

        // Usuniecie metody obs³ugujacej interakcje
        player_input.InteractPlayerInput.Interact.performed -= InteractPerformed;
    }

    /// <summary>
    /// Wykonywanie interakcji, gdy gracz nacisnie odpowiedni przycisk.
    /// </summary>
    private void InteractPerformed(InputAction.CallbackContext context)
    {
        // Sprawdzenie, czy gracz moze przeprowadzaæ interakcje
        if (can_interact)
        {
            // Wywolanie proby interakcji
            interactable_targets_detector.TryInteracting();
        }
    }

    /// <summary>
    /// Ustawia obiekt wejscia gracza.
    /// </summary>
    /// <param name="input">Obiekt wejscia gracza (PlayerInput).</param>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
