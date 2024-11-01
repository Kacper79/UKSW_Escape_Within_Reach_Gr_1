using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput player_input;

    private void Awake()
    {
        player_input = new();

        //GetComponentInChildren<PlayerInteractionController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerMovementController>().SetPlayerInput(player_input);
        //GetComponentInChildren<PlayerAttackingController>().SetPlayerInput(player_input);
    }
}
