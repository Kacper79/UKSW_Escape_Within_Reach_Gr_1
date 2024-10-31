using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbUpDetector : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player_movement_controller;

    private void OnTriggerStay(Collider other)
    {
        player_movement_controller.TryClimbingUp();
    }
}
