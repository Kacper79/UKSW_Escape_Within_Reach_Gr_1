using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private InteractableTargetsDetector interactable_targets_detector;

    private PlayerInput player_input;

    private bool can_interact = true;

    private void OnEnable()
    {
        player_input.InteractPlayerInput.Enable();

        player_input.InteractPlayerInput.Interact.performed += InteractPerformed;

        GlobalEvents.OnReadingPage += OnReadingPage;
        GlobalEvents.OnStoppingReadingPage += OnStoppingReadingPage;
    }

    private void OnDisable()
    {
        player_input.InteractPlayerInput.Disable();

        player_input.InteractPlayerInput.Interact.performed -= InteractPerformed;

        GlobalEvents.OnReadingPage -= OnReadingPage;
        GlobalEvents.OnStoppingReadingPage -= OnStoppingReadingPage;
    }

    private void OnStoppingReadingPage(object sender, EventArgs e)
    {
        can_interact = true;
    }

    private void OnReadingPage(object sender, EventArgs e)
    {
        can_interact = false;
    }

    private void InteractPerformed(InputAction.CallbackContext context)
    {
        if(can_interact)
        {
            interactable_targets_detector.TryInteracting();
        }   
    }

    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
