using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detekcja wspinaczki
/// </summary>
public class ClimbUpDetector : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player_movement_controller;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Climbable"))
        {
            player_movement_controller.TryClimbingUp();
        }
    }
}
