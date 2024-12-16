using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableTargetsDetector : MonoBehaviour
{
    private const float LOOK_FOR_INTERACTABLES_MAX_DISTANCE = 3.0f;//has to be lass than z value in detector_raycast_direction.position
    private const float TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START = 0.3f;
    private const float LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS = 0.1f;

    [SerializeField] private Transform detector_raycast_origin;// TODO: direction is not moving upwards, we gotta decide if it's intended or nah
    [SerializeField] private Transform detector_raycast_direction;
    [SerializeField] private InteractTooltipUI interact_tooltip_UI;

    private Vector3 ray_origin;
    private Vector3 ray_direction;

    private void OnEnable()
    {
        InvokeRepeating(nameof(LookForInteractableToShowUITooltip), TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START, LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS);

        GlobalEvents.OnReadingPage += StopLookingForInteractable;
        GlobalEvents.OnStoppingReadingPage += StartLookingForInteractable;

        GlobalEvents.OnStartingDialogue += StopLookingForInteractable;
        GlobalEvents.OnEndingDialogue += StartLookingForInteractable;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(LookForInteractableToShowUITooltip));

        GlobalEvents.OnReadingPage -= StopLookingForInteractable;
        GlobalEvents.OnStoppingReadingPage -= StartLookingForInteractable;

        GlobalEvents.OnStartingDialogue -= StopLookingForInteractable;
        GlobalEvents.OnEndingDialogue -= StartLookingForInteractable;
    }

    private void StopLookingForInteractable(object sender, EventArgs e)
    {
        CancelInvoke(nameof(LookForInteractableToShowUITooltip));
    }

    private void StartLookingForInteractable(object sender, EventArgs e)
    {
        InvokeRepeating(nameof(LookForInteractableToShowUITooltip), TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START, LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS);
    }

    private void LookForInteractableToShowUITooltip()
    {
        ray_origin = detector_raycast_origin.position;
        ray_direction = detector_raycast_direction.position - detector_raycast_origin.position;

        RaycastHit[] all_hits = Physics.RaycastAll(ray_origin, ray_direction, LOOK_FOR_INTERACTABLES_MAX_DISTANCE);

        foreach (RaycastHit hit in all_hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interact_tooltip_UI.SetTooltip(interactable.GetInteractionTooltip());
                interactable.AdditionalStuffWhenLookingAtInteractable();

                return;
            }
        }

        GlobalEvents.FireOnNotLookingOnInteractable(this);
    }

    public void TryInteracting()
    {
        ray_origin = detector_raycast_origin.position;
        ray_direction = detector_raycast_direction.position - detector_raycast_origin.position;

        RaycastHit[] all_hits = Physics.RaycastAll(ray_origin, ray_direction, LOOK_FOR_INTERACTABLES_MAX_DISTANCE);
        
        foreach (RaycastHit hit in all_hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();

                return;
            }
        }
    }
}
