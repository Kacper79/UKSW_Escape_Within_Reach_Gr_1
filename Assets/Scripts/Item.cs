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

    [SerializeField] private bool is_plot;

    private void OnEnable()
    {
        this.gameObject.GetComponent<MeshFilter>().mesh = model;
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
        PickUpItem();
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
}
