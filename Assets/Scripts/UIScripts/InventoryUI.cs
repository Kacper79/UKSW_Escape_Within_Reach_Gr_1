using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button right_arrow_button;
    [SerializeField] private Button left_arrow_button;

    [Header("Scripts")]
    [SerializeField] private PlayerAssetsUI player_assets_UI;

    [Header("Slots")]
    [SerializeField] private List<Slot> plot_items_slots;
    [SerializeField] private List<Slot> other_items_slots;

    [Header("OtherVisuals")]
    [SerializeField] private TextMeshProUGUI gold_display;


    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
        left_arrow_button.onClick.AddListener(OnLeftArrowButtonClick);          
    }

    private void OnEnable()
    {
        GlobalEvents.OnInventoryOpenCallBack += LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack += DisplayGold;

        GlobalEvents.FireOnInventoryOpen(this);
    }

    private void OnDisable()
    {
        GlobalEvents.OnInventoryOpenCallBack -= LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack -= DisplayGold;
    }

    private void DisplayGold(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        gold_display.text = e.gold_amount.ToString();
    }

    private void LoadItemsToSlots(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        int plot_slot_to_fill = 0;
        int other_slot_to_fill = 0;

        foreach (Item item in e.plot_items_list)
        {
            plot_items_slots[plot_slot_to_fill].SetIsEmpty(false);
            plot_items_slots[plot_slot_to_fill].SetIcon(item.GetIcon());
            plot_items_slots[plot_slot_to_fill].SetItemDescritpionInSlot(item.GetItemDescription());
            plot_items_slots[plot_slot_to_fill].SetItemQuantity(e.item_amount[item.GetItemName()]);
            plot_slot_to_fill++;
        }

        foreach(Item item in e.other_items_list)
        {
            other_items_slots[other_slot_to_fill].SetIsEmpty(false);
            other_items_slots[other_slot_to_fill].SetIcon(item.GetIcon());
            other_items_slots[other_slot_to_fill].SetItemDescritpionInSlot(item.GetItemDescription());
            other_items_slots[other_slot_to_fill].SetItemQuantity(e.item_amount[item.GetItemName()]);
            other_slot_to_fill++;
        }
    }
    
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.QuestLog);
    }

    private void OnLeftArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Achievements);
    }
    
}
