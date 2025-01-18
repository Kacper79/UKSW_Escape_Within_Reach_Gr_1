using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie interakcjami gracza z obiektami w grze.
/// Obs�uguje detekcj� obiekt�w interakcji oraz wykonanie interakcji po naci�ni�ciu przycisku przez gracza.
/// </summary>
public class PlayerInteractionController : MonoBehaviour
{
    /// <summary>
    /// Detektor obiekt�w interakcji w grze.
    /// </summary>
    [SerializeField] private InteractableTargetsDetector interactable_targets_detector;

    /// <summary>
    /// Komponent wej�cia gracza (PlayerInput).
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Flaga wskazuj�ca, czy gracz mo�e przeprowadza� interakcje.
    /// </summary>
    private bool can_interact = true;

    /// <summary>
    /// Aktualizacja logiki interakcji, sprawdzanie detektora obiekt�w.
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
    /// Subskrybuje zdarzenia wej�cia gracza.
    /// </summary>
    private void OnEnable()
    {
        player_input.InteractPlayerInput.Enable();

        // Rejestracja metody obs�uguj�cej interakcj�
        player_input.InteractPlayerInput.Interact.performed += InteractPerformed;
    }

    /// <summary>
    /// Odsubskrybowuje zdarzenia, gdy obiekt jest dezaktywowany.
    /// </summary>
    private void OnDisable()
    {
        player_input.InteractPlayerInput.Disable();

        // Usuni�cie metody obs�uguj�cej interakcj�
        player_input.InteractPlayerInput.Interact.performed -= InteractPerformed;
    }

    /// <summary>
    /// Wykonywanie interakcji, gdy gracz naci�nie odpowiedni przycisk.
    /// </summary>
    private void InteractPerformed(InputAction.CallbackContext context)
    {
        // Sprawdzenie, czy gracz mo�e przeprowadza� interakcj�
        if (can_interact)
        {
            // Wywo�anie pr�by interakcji
            interactable_targets_detector.TryInteracting();
        }
    }

    /// <summary>
    /// Ustawia obiekt wej�cia gracza.
    /// </summary>
    /// <param name="input">Obiekt wej�cia gracza (PlayerInput).</param>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
