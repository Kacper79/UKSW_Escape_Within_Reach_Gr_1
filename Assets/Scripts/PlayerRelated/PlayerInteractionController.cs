using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private InteractableTargetsDetector interactable_targets_detector;

    private PlayerInput player_input;

    private void OnEnable()
    {
        player_input.InteractPlayerInput.Enable();

        player_input.InteractPlayerInput.Interact.performed += InteractPerformed;
    }

    private void OnDisable()
    {
        player_input.InteractPlayerInput.Disable();

        player_input.InteractPlayerInput.Interact.performed -= InteractPerformed;
    }

    private void InteractPerformed(InputAction.CallbackContext context)
    {
        interactable_targets_detector.TryInteracting();
    }

    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
