using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenSafe : MonoBehaviour, IInteractable
{
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.66f;
    private const float Z_TRANSFORM_VALUE_FOR_OPENING = 2.5f;

    private bool is_opened = false;
    private bool is_already_unlocked = false;
    private bool is_during_opening_animation = false;

    private float z_transform_value;

    string IInteractable.GetInteractionTooltip()
    {
        return "Gaba";
    }

    void IInteractable.Interact()
    {
        if (CanInteract())
        {
            z_transform_value = is_opened ? 0.0f : Z_TRANSFORM_VALUE_FOR_OPENING;
            StartCoroutine(OpenOrCloseDoors(z_transform_value));
            is_opened = !is_opened;
        }
    }

    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    private IEnumerator OpenOrCloseDoors(float target_z_value)
    {
        is_during_opening_animation = true;

        float elapsed_time = 0f;
        float start_z_value = transform.localPosition.z;

        while (elapsed_time < OPEN_AND_CLOSE_DOOR_TIME)
        {
            elapsed_time += Time.deltaTime; // Zwiêksz czas o czas miêdzy klatkami

            float current_z_transform_value = Mathf.Lerp(start_z_value, target_z_value, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, current_z_transform_value);

            yield return null;
        }

        transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, target_z_value);

        is_during_opening_animation = false;
    }

    private bool CanInteract()
    {
        if (is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        if (true)//if has key or story progressed enough return true...
        {
            is_already_unlocked = true;

            return true;
        }

        //return false;
    }
}
