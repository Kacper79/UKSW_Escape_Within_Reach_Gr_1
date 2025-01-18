using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Klasa odpowiedzialna za zarz¹dzanie interakcjami gracza z obiektami w grze.
/// Obs³uguje detekcjê obiektów interakcji oraz wykonanie interakcji po naciœniêciu przycisku przez gracza.
/// </summary>
public class PlayerInteractionController : MonoBehaviour
{
    /// <summary>
    /// Detektor obiektów interakcji w grze.
    /// </summary>
    [SerializeField] private InteractableTargetsDetector interactable_targets_detector;

    /// <summary>
    /// Komponent wejœcia gracza (PlayerInput).
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Flaga wskazuj¹ca, czy gracz mo¿e przeprowadzaæ interakcje.
    /// </summary>
    private bool can_interact = true;

    /// <summary>
    /// Aktualizacja logiki interakcji, sprawdzanie detektora obiektów.
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
    /// Subskrybuje zdarzenia wejœcia gracza.
    /// </summary>
    private void OnEnable()
    {
        player_input.InteractPlayerInput.Enable();

        // Rejestracja metody obs³uguj¹cej interakcjê
        player_input.InteractPlayerInput.Interact.performed += InteractPerformed;
    }

    /// <summary>
    /// Odsubskrybowuje zdarzenia, gdy obiekt jest dezaktywowany.
    /// </summary>
    private void OnDisable()
    {
        player_input.InteractPlayerInput.Disable();

        // Usuniêcie metody obs³uguj¹cej interakcjê
        player_input.InteractPlayerInput.Interact.performed -= InteractPerformed;
    }

    /// <summary>
    /// Wykonywanie interakcji, gdy gracz naciœnie odpowiedni przycisk.
    /// </summary>
    private void InteractPerformed(InputAction.CallbackContext context)
    {
        // Sprawdzenie, czy gracz mo¿e przeprowadzaæ interakcjê
        if (can_interact)
        {
            // Wywo³anie próby interakcji
            interactable_targets_detector.TryInteracting();
        }
    }

    /// <summary>
    /// Ustawia obiekt wejœcia gracza.
    /// </summary>
    /// <param name="input">Obiekt wejœcia gracza (PlayerInput).</param>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
