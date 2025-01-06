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

        GlobalEvents.OnReadingPage += DisableSelf;
        GlobalEvents.OnStoppingReadingPage += EnableSelf;

        GlobalEvents.OnStartingDialogue += DisableSelf;
        GlobalEvents.OnEndingDialogue += EnableSelf;

        GlobalEvents.OnStartingBlackJackGameForMoney += DisableSelf;
        GlobalEvents.OnStartingBlackJackGameForPickaxe += DisableSelf;

        GlobalEvents.OnEndingBlackjackGame += EnableSelf;
    }

    private void OnDisable()
    {
        GlobalEvents.OnNotLookingOnInteractable -= OnNotLookingForAnythingInteractable;

        GlobalEvents.OnReadingPage -= DisableSelf;
        GlobalEvents.OnStoppingReadingPage -= EnableSelf;

        GlobalEvents.OnStartingDialogue -= DisableSelf;
        GlobalEvents.OnEndingDialogue -= EnableSelf;

        GlobalEvents.OnStartingBlackJackGameForMoney -= DisableSelf;
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= DisableSelf;

        GlobalEvents.OnEndingBlackjackGame -= EnableSelf;
    }

    private void EnableSelf(object sender, System.EventArgs e)
    {
        EnableUI();
    }

    private void DisableSelf(object sender, System.EventArgs e)
    {
        DisableUI();
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
