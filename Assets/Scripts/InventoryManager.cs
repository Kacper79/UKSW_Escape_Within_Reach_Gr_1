using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private const int MAX_OTHER_ITEMS = 4;
    private const int MAX_PLOT_ITEMS = 4;

    private List<Item> plot_picked_up_items = new List<Item>();
    private List<Item> other_picked_up_items = new List<Item>();

    private int gold_amount = 0;

    public Dictionary<string, int> item_amount = new();

    private void OnEnable()
    {
        GlobalEvents.OnPickUpItem += PickUpItem;
        GlobalEvents.OnInventoryOpen += OnInventoryOpenCallBack;
    }    

    private void OnDisable()
    {
        GlobalEvents.OnPickUpItem -= PickUpItem;
        GlobalEvents.OnInventoryOpen -= OnInventoryOpenCallBack;
    }

    private void OnInventoryOpenCallBack(object sender, System.EventArgs e)
    {
        GlobalEvents.OnInventoryOpenCallBackEventArgs args = new(plot_picked_up_items, other_picked_up_items, item_amount);
        GlobalEvents.FireOnInventoryOpenCallBack(this, args);
    }

    private void AddGold(int value)
    {
        gold_amount += value;
    }

    private void SpendGold(int value)
    {
        gold_amount -= value;
    }
    private void PickUpItem(object sender, GlobalEvents.OnPickUpItemEventArgs e)
    {
        if(!e.item.GetIsPlot())
        {
            if(other_picked_up_items.Count < MAX_OTHER_ITEMS) 
            {
                if (!item_amount.ContainsKey(e.item.GetItemName()))
                {
                    item_amount.Add(e.item.GetItemName(), 1);
                    other_picked_up_items.Add(e.item);
                }
                else
                {
                    item_amount[e.item.GetItemName()]++;
                }
                e.item.gameObject.SetActive(false);
                Debug.Log(item_amount[e.item.GetItemName()]);
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
        else
        {
            if (plot_picked_up_items.Count < MAX_PLOT_ITEMS)
            {
                if (!item_amount.ContainsKey(e.item.GetItemName()))
                {
                    item_amount.Add(e.item.GetItemName(), 1);
                    plot_picked_up_items.Add(e.item);
                }
                else
                {
                    item_amount[e.item.GetItemName()]++;
                }
                Debug.Log(item_amount[e.item.GetItemName()]);
                e.item.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
    }

    private void RemoveUsedItem(Item item_to_use) //funkcja do uzywania itemk�w takich jak szlugi wytrychy itp, usuwa jeden itemek z eq
    {
        if(item_amount[item_to_use.GetItemName()] == 0)
        {
            Debug.Log("No item to use.");
        }
        else if (item_amount[item_to_use.GetItemName()] == 1)
        {
            item_amount.Remove(item_to_use.GetItemName());

            if(item_to_use.GetIsPlot())
            {
                plot_picked_up_items.Remove(item_to_use);
            }
            else
            {
                other_picked_up_items.Remove(item_to_use);
            }
        }
        else
        {
            item_amount[item_to_use.GetItemName()]--;
        }
    }
}
