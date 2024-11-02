using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interaction_tooltip_tmp;


    private void OnEnable()
    {
        DisableUI();

        GlobalEvents.OnNotLookingOnInteractable += OnNotLookingForAnythingInteractable;
    }

    private void OnDisable()
    {
        GlobalEvents.OnNotLookingOnInteractable -= OnNotLookingForAnythingInteractable;
    }

    private void OnNotLookingForAnythingInteractable(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    private void EnableUI()
    {
        interaction_tooltip_tmp.enabled = true;
    }

    public void DisableUI()
    {
        interaction_tooltip_tmp.enabled = false;
    }

    public void SetTooltip(string tooltip_message)
    {
        EnableUI();
        interaction_tooltip_tmp.text = tooltip_message;
    }
}
