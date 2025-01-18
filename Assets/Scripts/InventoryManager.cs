using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveable
{
    private const int MAX_OTHER_ITEMS = 4;
    private const int MAX_PLOT_ITEMS = 4;
    private const int BREAKING_ROCK_GAIN = 500;

    [SerializeField] private List<Item> allPosibleObjects;
    private List<Item> plot_picked_up_items = new List<Item>();
    private List<Item> other_picked_up_items = new List<Item>();

    [SerializeField] private int gold_amount = 5000;

    public Dictionary<string, int> item_amount = new();
    [SerializeField] private List<DialogueNodeSO> possiblePayoffDialogues;
    private List<DialogueNodeSO> payoffsToCheck = new();

    private void OnEnable()
    {
        gold_amount = 1000;

        GlobalEvents.OnPickUpItem += PickUpItem;
        GlobalEvents.OnInventoryOpen += OnInventoryOpenCallBack;
        GlobalEvents.OnPayoff += OnPayoffCallback;
        GlobalEvents.OnDestroingRock += OnRockDestroyedCallback;
        GlobalEvents.OnLosingOrWinningMoneyInABlackjackGame += ChangeGoldAmount;
    }    

    private void OnDisable()
    {
        GlobalEvents.OnPickUpItem -= PickUpItem;
        GlobalEvents.OnInventoryOpen -= OnInventoryOpenCallBack;
        GlobalEvents.OnDestroingRock -= OnRockDestroyedCallback;
        GlobalEvents.OnPayoff -= OnPayoffCallback;
        GlobalEvents.OnLosingOrWinningMoneyInABlackjackGame -= ChangeGoldAmount;
    }

    void Start()
    {
        SaveManager.Instance.saveablesGO.Add(this);
        payoffsToCheck = possiblePayoffDialogues;
        CheckIfPayoffAvailable();
    }

    private void OnInventoryOpenCallBack(object sender, System.EventArgs e)
    {
        GlobalEvents.OnInventoryOpenCallBackEventArgs args = new(plot_picked_up_items, other_picked_up_items, item_amount, gold_amount);
        GlobalEvents.FireOnInventoryOpenCallBack(this, args);
    }

    private void OnRockDestroyedCallback(object sender, System.EventArgs e)
    {
        AddGold(BREAKING_ROCK_GAIN);
    }

    private void ChangeGoldAmount(object sender, GlobalEvents.OnLosingOrWinningMoneyInABlackjackGameEventArgs args)
    {
        AddGold(args.value);
        Debug.Log(gold_amount);
    }

    private void OnPayoffCallback(object sender, GlobalEvents.OnPayoffEventArgs args)
    {
        if(gold_amount - args.payment_amount >= 0)
        {
            SpendGold(args.payment_amount);
        } else
        {
            Debug.Log("You don't have enough money for a payoff");
        }
    }

    private void AddGold(int value)
    {
        gold_amount += value;
        CheckIfPayoffAvailable();
    }

    private void SpendGold(int value)
    {
        if(gold_amount - value >= 0) gold_amount -= value;
    }

    private void CheckIfPayoffAvailable()
    {
        //Disable payoffs if the player cannot now afford them
        foreach(DialogueNodeSO dialogue in possiblePayoffDialogues)
        {
            if (!payoffsToCheck.Contains(dialogue))
            {
                if(dialogue.payoffAmount > 0 && gold_amount < dialogue.payoffAmount && dialogue.is_available)
                {
                    GlobalEvents.FireOnMakingGivenDialogueOptionAvailableOrUnavailable(this, new(dialogue.id, false));
                }
            } 
        }

        //Check for some new payoff dialogue option available
        foreach(DialogueNodeSO dialogue in payoffsToCheck.Reverse<DialogueNodeSO>())
        {
            if (gold_amount >= dialogue.payoffAmount && dialogue.payoffAmount > 0)
            {
                GlobalEvents.FireOnMakingGivenDialogueOptionAvailableOrUnavailable(this, new(dialogue.id, true));
                payoffsToCheck.Remove(dialogue);
            }
        }
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

    public bool RemoveUsedItem(Item item_to_use) //funkcja do uzywania itemk�w takich jak szlugi wytrychy itp, usuwa jeden itemek z eq
    {
        if(item_amount[item_to_use.GetItemName()] == 0)
        {
            Debug.Log("No item to use.");
            return false;
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
        return true;
    }

    /// <summary>
    /// This function is being used to save all the inventory contents to the save file
    /// </summary>
    /// <param name="saveData">Mutable save data struct to save data to</param>
    public void Save(ref SaveData saveData)
    {
        saveData.inventoryGoldAmount = gold_amount;
        saveData.inventoryItemAmount = new(item_amount);
        saveData.inventoryOtherItems = new();
        foreach (Item item in other_picked_up_items)
        {
            int itemID = allPosibleObjects.FindIndex(posItem => posItem == item);
            if (itemID != -1) saveData.inventoryOtherItems.Add(itemID);
        }
        //saveData.inventoryOtherItems = other_picked_up_items;
        saveData.inventoryPlotItems = new();
        foreach (Item item in plot_picked_up_items)
        {
            int itemID = allPosibleObjects.FindIndex(posItem => posItem == item);
            if (itemID != -1) saveData.inventoryPlotItems.Add(itemID);
        }
        //saveData.inventoryPlotItems = plot_picked_up_items;
    }

    /// <summary>
    /// This function is being used to load all the inventory contents from the save file
    /// </summary>
    /// <param name="saveData">Save data struct to load data from</param>
    public void Load(SaveData saveData)
    {
        gold_amount = saveData.inventoryGoldAmount;
        item_amount = saveData.inventoryItemAmount;
        foreach (int itemID in saveData.inventoryOtherItems)
        {
            Item item = allPosibleObjects[itemID];
            if (itemID >= 0 && itemID < allPosibleObjects.Count) other_picked_up_items.Add(item);
        }
        foreach (int itemID in saveData.inventoryPlotItems)
        {
            Item item = allPosibleObjects[itemID];
            if (itemID >= 0 && itemID < allPosibleObjects.Count) plot_picked_up_items.Add(item);
        }

        //other_picked_up_items = saveData.inventoryOtherItems;
        //plot_picked_up_items = saveData.inventoryPlotItems;
    }

    public int GetGoldAmount()
    {
        return gold_amount;
    }
}
