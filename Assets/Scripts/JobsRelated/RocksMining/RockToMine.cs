using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockToMine : MonoBehaviour, IInteractable
{
    private const int GOLD_FOR_COMPLETING = 20;
    private const int MAX_HP = 10;

    private MiningProgressUI mining_progress_UI;

    private string interaction_tooltip_message = "Press [E] to mine";

    private int hp = MAX_HP;

    private void Awake()
    {
        mining_progress_UI = FindObjectOfType<MiningProgressUI>();
    }

    string IInteractable.GetInteractionTooltip()
    {
        return interaction_tooltip_message;
    }

    void IInteractable.Interact()
    {
        if (CanInteract())
        {
            hp -= 1;
            ChangeGraphic();
            CheckIfDone();
        }
    }

    void IInteractable.AdditionalStuffWhenLookingAtInteractable()
    {
        mining_progress_UI.EnableUI();
        mining_progress_UI.PassValuesToProgressbar(MAX_HP - hp, MAX_HP);
    }

    private bool CanInteract()
    {
        return true;//idk might be useful in the future
    }

    private void ChangeGraphic()
    {
        //TODO: change graphic depeneding on Rock HP
    }

    private void CheckIfDone()
    {
        if (hp <= 0)
        {
            Debug.Log(GOLD_FOR_COMPLETING + " gold for this rock");//TODO: some gold for a player

            Destroy(this.gameObject);

            GlobalEvents.FireOnDestroingRock(this);
        }
    }
}
