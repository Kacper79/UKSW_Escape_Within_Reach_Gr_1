using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, IInteractable
{
    private const float OPEN_AND_CLOSE_DOOR_TIME = 0.25f;

    private string interaction_tooltip_message_opened = "Press [E] to open doors";
    private string interaction_tooltip_message_closed = "Press [E] to close doors";

    private bool is_opened = false;
    private bool is_already_unlocked = false;
    private bool is_during_opening_animation = false;

    private float euler_degree_for_opening_doors;

    string IInteractable.GetInteractionTooltip()
    {
        return is_opened ? interaction_tooltip_message_closed : interaction_tooltip_message_opened;
    }

    void IInteractable.Interact()
    {
        if(CanInteract())
        {
            euler_degree_for_opening_doors = is_opened ? 0.0f : 90.0f;
            StartCoroutine(OpenOrCloseDoors(euler_degree_for_opening_doors));
            is_opened = !is_opened;
        }
    }

    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        return;
    }

    private IEnumerator OpenOrCloseDoors(float target_degree)
    {
        is_during_opening_animation = true;

        float elapsed_time = 0f;
        float start_degree = transform.localEulerAngles.y;

        if (start_degree > 180f)
        {
            start_degree -= 360f;
        }

        while (elapsed_time < OPEN_AND_CLOSE_DOOR_TIME)
        {
            elapsed_time += Time.deltaTime; // Zwiêksz czas o czas miêdzy klatkami

            float currentDegree = Mathf.Lerp(start_degree, target_degree, elapsed_time / OPEN_AND_CLOSE_DOOR_TIME);

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentDegree, transform.localEulerAngles.z
            );

            yield return null;
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, target_degree, transform.localEulerAngles.z);

        is_during_opening_animation = false;
    }

    private bool CanInteract()
    {
        if(is_already_unlocked && !is_during_opening_animation)
        {
            return true;
        }

        if(true)//if has key or story progressed enough return true...
        {
            is_already_unlocked = true;

            return true;
        }

        //return false;
    }
}
