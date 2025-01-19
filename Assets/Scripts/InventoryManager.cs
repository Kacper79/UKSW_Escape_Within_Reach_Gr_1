using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveable
{
    /// <summary>
    /// Maksymalna liczba innych przedmiotów, które mogą znajdować się w ekwipunku.
    /// </summary>
    private const int MAX_OTHER_ITEMS = 4;

    /// <summary>
    /// Maksymalna liczba przedmiotów fabularnych, które mogą znajdować się w ekwipunku.
    /// </summary>
    private const int MAX_PLOT_ITEMS = 4;
    private const int BREAKING_ROCK_GAIN = 500;

    /// <summary>
    /// Lista wszystkich dostępnych przedmiotów, które mogą być podniesione przez gracza.
    /// </summary>
    [SerializeField] private List<Item> allPosibleObjects;

    /// <summary>
    /// Lista przedmiotów fabularnych, które zostały podniesione przez gracza.
    /// </summary>
    private List<Item> plot_picked_up_items = new List<Item>();

    /// <summary>
    /// Lista innych przedmiotów, które zostały podniesione przez gracza.
    /// </summary>
    private List<Item> other_picked_up_items = new List<Item>();

    /// <summary>
    /// Początkowa ilość złota, którą posiada gracz.
    /// </summary>
    [SerializeField] private int gold_amount = 5000;

    /// <summary>
    /// Słownik przechowujący ilość poszczególnych przedmiotów w ekwipunku gracza.
    /// Kluczem jest nazwa przedmiotu, a wartością jego ilość.
    /// </summary>
    public Dictionary<string, int> item_amount = new();
    [SerializeField] private List<DialogueNodeSO> possiblePayoffDialogues;
    private List<DialogueNodeSO> payoffsToCheck = new();


    /// <summary>
    /// Inicjalizuje wartości i subskrybuje zdarzenia.
    /// </summary>
    private void OnEnable()
    {
        gold_amount = 1000;  // Ustawienie początkowej ilości złota

        // Subskrypcja na różne zdarzenia globalne
        GlobalEvents.OnPickUpItem += PickUpItem;
        GlobalEvents.OnInventoryOpen += OnInventoryOpenCallBack;
        GlobalEvents.OnPayoff += OnPayoffCallback;
        GlobalEvents.OnDestroingRock += OnRockDestroyedCallback;
        GlobalEvents.OnLosingOrWinningMoneyInABlackjackGame += ChangeGoldAmount;
    }

    /// <summary>
    /// Anuluje subskrypcję zdarzeń przy wyłączeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnPickUpItem -= PickUpItem;
        GlobalEvents.OnInventoryOpen -= OnInventoryOpenCallBack;
        GlobalEvents.OnDestroingRock -= OnRockDestroyedCallback;
        GlobalEvents.OnPayoff -= OnPayoffCallback;
        GlobalEvents.OnLosingOrWinningMoneyInABlackjackGame -= ChangeGoldAmount;
    }

    /// <summary>
    /// Rejestruje menedżera w systemie zapisywania przy starcie gry.
    /// </summary>
    void Start()
    {
        SaveManager.Instance.saveablesGO.Add(this);
        payoffsToCheck = new(possiblePayoffDialogues);
        CheckIfPayoffAvailable();
    }

    /// <summary>
    /// Wywoływane przy otwarciu ekwipunku - przekazuje dane do innych systemów.
    /// </summary>
    private void OnInventoryOpenCallBack(object sender, System.EventArgs e)
    {
        // Tworzy obiekt z danymi o ekwipunku i wywołuje zdarzenie otwarcia ekwipunku
        GlobalEvents.OnInventoryOpenCallBackEventArgs args = new(plot_picked_up_items, other_picked_up_items, item_amount, gold_amount);
        GlobalEvents.FireOnInventoryOpenCallBack(this, args);
    }

    private void OnRockDestroyedCallback(object sender, System.EventArgs e)
    {
        AddGold(BREAKING_ROCK_GAIN);
    }

    /// <summary>
    /// Zmienia ilość złota po wygranej lub przegranej w blackjacku.
    /// </summary>
    private void ChangeGoldAmount(object sender, GlobalEvents.OnLosingOrWinningMoneyInABlackjackGameEventArgs args)
    {
        AddGold(args.value);  // Dodaje lub odejmuje złoto
        Debug.Log(gold_amount);  // Wyświetla nową ilość złota w logach
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

    /// <summary>
    /// Dodaje złoto do aktualnej ilości.
    /// </summary>
    private void AddGold(int value)
    {
        gold_amount += value;
        CheckIfPayoffAvailable();
    }

    /// <summary>
    /// Wydaje złoto, zmniejszając jego ilość.
    /// </summary>
    private void SpendGold(int value)
    {
        if(gold_amount - value >= 0) gold_amount -= value;
        CheckIfPayoffAvailable();
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
                if(dialogue.questNumber == 0 || (dialogue.questNumber != 0 && !QuestManager.Instance.IsQuestCompleted(dialogue.questNumber)))
                {
                    GlobalEvents.FireOnMakingGivenDialogueOptionAvailableOrUnavailable(this, new(dialogue.id, true));
                    payoffsToCheck.Remove(dialogue);
                }
            }
        }
    }

    /// <summary>
    /// Obsługuje podniesienie przedmiotu - dodaje go do ekwipunku.
    /// </summary>
    private void PickUpItem(object sender, GlobalEvents.OnPickUpItemEventArgs e)
    {
        // Sprawdza, czy przedmiot jest przedmiotem fabularnym, czy innym
        if (!e.item.GetIsPlot())
        {
            // Sprawdza, czy ekwipunek na inne przedmioty nie jest pełny
            if (other_picked_up_items.Count < MAX_OTHER_ITEMS)
            {
                // Sprawdza, czy przedmiot już istnieje w ekwipunku, jeśli nie dodaje go
                if (!item_amount.ContainsKey(e.item.GetItemName()))
                {
                    item_amount.Add(e.item.GetItemName(), 1);
                    other_picked_up_items.Add(e.item);
                }
                else
                {
                    item_amount[e.item.GetItemName()]++;
                }
                e.item.gameObject.SetActive(false);  // Dezaktywuje obiekt przedmiotu w świecie gry
                Debug.Log(item_amount[e.item.GetItemName()]);  // Wyświetla ilość przedmiotu w logach
            }
            else
            {
                Debug.Log("Inventory is full");  // Ekwipunek jest pełny
            }
        }
        else
        {
            // Obsługuje przypadek, gdy przedmiot jest fabularny
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
                Debug.Log("Inventory is full");  // Ekwipunek fabularny jest pełny
            }
        }
    }

    /// <summary>
    /// Usuwa użyty przedmiot z ekwipunku.
    /// </summary>
    public bool RemoveUsedItem(Item item_to_use)
    {
        if (item_amount[item_to_use.GetItemName()] == 0)
        {
            Debug.Log("No item to use.");
            return false;  // Brak przedmiotu do użycia
        }
        else if (item_amount[item_to_use.GetItemName()] == 1)
        {
            item_amount.Remove(item_to_use.GetItemName());  // Usuwa przedmiot z ekwipunku

            // Sprawdza, czy przedmiot jest fabularny, czy inny
            if (item_to_use.GetIsPlot())
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
            item_amount[item_to_use.GetItemName()]--;  // Zmniejsza ilość przedmiotu
        }
        return true;
    }

    /// <summary>
    /// This function is being used to save all the inventory contents to the save file
    /// </summary>
    /// <param name="saveData">Mutable save data struct to save data to</param>
    public void Save(ref SaveData saveData)
    {
        // Zapisuje ilość złota, ilość przedmiotów oraz listy przedmiotów w ekwipunku
        saveData.inventoryGoldAmount = gold_amount;
        saveData.inventoryItemAmount = new(item_amount);
        saveData.inventoryOtherItems = new();
        foreach (Item item in other_picked_up_items)
        {
            int itemID = allPosibleObjects.FindIndex(posItem => posItem == item);
            if (itemID != -1) saveData.inventoryOtherItems.Add(itemID);
        }
        saveData.inventoryPlotItems = new();
        foreach (Item item in plot_picked_up_items)
        {
            int itemID = allPosibleObjects.FindIndex(posItem => posItem == item);
            if (itemID != -1) saveData.inventoryPlotItems.Add(itemID);
        }
        //saveData.inventoryPlotItems = plot_picked_up_items;

        GameAnalytics.Instance.ModifyStat("moneyCounter", gold_amount);
        GameAnalytics.Instance.ModifyStat("plotItemsCount", plot_picked_up_items.Count);
    }

    /// <summary>
    /// This function is being used to load all the inventory contents from the save file
    /// </summary>
    /// <param name="saveData">Save data struct to load data from</param>
    public void Load(SaveData saveData)
    {
        gold_amount = saveData.inventoryGoldAmount;
        item_amount = saveData.inventoryItemAmount;

        // Ładowanie innych przedmiotów
        foreach (int itemID in saveData.inventoryOtherItems)
        {
            Item item = allPosibleObjects[itemID];
            if (itemID >= 0 && itemID < allPosibleObjects.Count) other_picked_up_items.Add(item);
        }

        // Ładowanie przedmiotów fabularnych
        foreach (int itemID in saveData.inventoryPlotItems)
        {
            Item item = allPosibleObjects[itemID];
            if (itemID >= 0 && itemID < allPosibleObjects.Count) plot_picked_up_items.Add(item);
        }
    }

    /// <summary>
    /// Zwraca ilość złota w ekwipunku.
    /// </summary>
    public int GetGoldAmount()
    {
        return gold_amount;
    }
}
