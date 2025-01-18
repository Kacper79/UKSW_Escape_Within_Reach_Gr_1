using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour, IInteractable
{
    [Header("Strings")]
    [SerializeField] private string item_name;
    [SerializeField] private string item_description;

    [Header("Graphics")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Mesh model;

    [Header("What quest does this item finish")]
    [SerializeField] private int quest_id;

    [SerializeField] private bool is_plot;

    [SerializeField] private bool is_interactable = true;

    void Start()
    {
        gameObject.GetComponent<MeshFilter>().mesh = model;
    }

    private void OnEnable()
    {
        GlobalEvents.OnWinningPickaxeInABlackjackGame += MakeItemInteractable;
    }


    private void OnDisable()
    {
        GlobalEvents.OnWinningPickaxeInABlackjackGame -= MakeItemInteractable;
    }

    private void MakeItemInteractable(object sender, GlobalEvents.OnMakingGivenItemInteractableEventArgs e)
    {
        if(item_name == e.name)
        {
            this.is_interactable = true;
        }
    }
    private void PickUpItem()
    {
        GlobalEvents.OnPickUpItemEventArgs args = new(this);
        GlobalEvents.FireOnPickUpItem(this, args);
    }

    public string GetItemDescription()
    {
        return item_description;
    }

    public void Interact()
    {
        if(is_interactable)
        {
            PickUpItem();

            if(is_plot)
            {
                QuestManager.Instance.MarkQuestCompleted(quest_id);
            }
        }
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public string GetItemName()
    {
        return item_name;
    }

    public Sprite GetIcon()
    {
        return icon;
    }
    public bool GetIsPlot()
    {
        return is_plot;
    }

    public string GetInteractionTooltip()
    {
        return $"Press [E] to pickup item {item_name}";
    }

    public void AdditionalStuffWhenLookingAtInteractable()
    {
        //Nothing to do for generic item
    }
    public void SetIsInteractable(bool b)
    {
        is_interactable = b;
    }
}
