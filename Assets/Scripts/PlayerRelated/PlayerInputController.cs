using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput player_input;

    private void Awake()
    {
        player_input = new();
        
        GetComponentInChildren<PlayerInteractionController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerMovementController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerAttackingController>().SetPlayerInput(player_input);
    }

    private void OnEnable()
    {
        GlobalEvents.OnStartingDialogue += DisableAllControllers;
        GlobalEvents.OnEndingDialogue += EnableAllControllers;

        GlobalEvents.OnReadingPage += DisableAllControllers;
        GlobalEvents.OnStoppingReadingPage += EnableAllControllers;
    }

    private void OnDisable()
    {
        GlobalEvents.OnStartingDialogue -= DisableAllControllers;
        GlobalEvents.OnEndingDialogue -= EnableAllControllers;

        GlobalEvents.OnReadingPage -= DisableAllControllers;
        GlobalEvents.OnStoppingReadingPage -= EnableAllControllers;
    }

    private void EnableAllControllers(object sender, System.EventArgs e)
    {
        EnableAllControllers();
    }

    private void DisableAllControllers(object sender, System.EventArgs e)
    {
        DisableAllControllers();
    }

    private void DisableAllControllers()
    {
        DisableMovementController();
        DisableAttackingController();
        DisableInteractionController();
    }

    private void EnableAllControllers()
    {
        EnableMovementController();
        EnableAttackingController();
        EnableInteractionController();
    }

    private void DisableMovementController()
    {
        player_input.MovementPlayerInput.Disable();
    }

    private void EnableMovementController()
    {
        player_input.MovementPlayerInput.Enable();
    }

    private void DisableAttackingController()
    {
        player_input.AttackPlayerInput.Disable();
    }

    private void EnableAttackingController()
    {
        player_input.AttackPlayerInput.Enable();
    }

    private void DisableInteractionController()
    {
        player_input.InteractPlayerInput.Disable();
    }

    private void EnableInteractionController()
    {
        player_input.InteractPlayerInput.Enable();
    }
}
