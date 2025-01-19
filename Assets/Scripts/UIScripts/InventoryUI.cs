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
    /// <summary>
    /// Przycisk strza³ki w prawo, u¿ywany do przechodzenia do kolejnych interfejsów u¿ytkownika.
    /// </summary>
    [SerializeField] private Button right_arrow_button;

    /// <summary>
    /// Przycisk strza³ki w lewo, u¿ywany do przechodzenia do poprzednich interfejsów u¿ytkownika.
    /// </summary>
    [SerializeField] private Button left_arrow_button;

    [Header("Scripts")]
    /// <summary>
    /// Referencja do g³ównego UI gracza, które zarz¹dza zasobami gracza.
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;

    [Header("Slots")]
    /// <summary>
    /// Lista slotów przechowuj¹cych przedmioty zwi¹zane z fabu³¹, takie jak przedmioty do uprawy.
    /// </summary>
    [SerializeField] private List<Slot> plot_items_slots;

    /// <summary>
    /// Lista slotów przechowuj¹cych inne przedmioty, które nie s¹ zwi¹zane z fabu³¹.
    /// </summary>
    [SerializeField] private List<Slot> other_items_slots;

    [Header("OtherVisuals")]
    /// <summary>
    /// Wyœwietlacz, który pokazuje iloœæ z³ota gracza.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gold_display;


    /// <summary>
    /// Inicjalizuje nas³uchiwanie na klikniêcie przycisków strza³ek do nawigacji po UI.
    /// </summary>
    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
        left_arrow_button.onClick.AddListener(OnLeftArrowButtonClick);
    }

    /// <summary>
    /// Rejestruje subskrypcjê do zdarzenia otwarcia ekwipunku oraz wyœwietlania przedmiotów i z³ota.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnInventoryOpenCallBack += LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack += DisplayGold;

        // Wysy³a zdarzenie otwarcia ekwipunku
        GlobalEvents.FireOnInventoryOpen(this);
    }

    /// <summary>
    /// Usuwa subskrypcjê do zdarzeñ zwi¹zanych z otwieraniem ekwipunku.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnInventoryOpenCallBack -= LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack -= DisplayGold;
    }

    /// <summary>
    /// Wyœwietla iloœæ z³ota w ekwipunku gracza w interfejsie u¿ytkownika.
    /// </summary>
    /// <param name="sender">Obiekt, który wys³a³ zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawieraj¹ce informacje o iloœci z³ota.</param>
    private void DisplayGold(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        gold_display.text = e.gold_amount.ToString();  // Ustawia tekst w UI na wartoœæ z³ota
    }

    /// <summary>
    /// £aduje przedmioty do odpowiednich slotów w ekwipunku, w zale¿noœci od typu przedmiotu.
    /// </summary>
    /// <param name="sender">Obiekt, który wys³a³ zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawieraj¹ce listy przedmiotów i ich iloœæ.</param>
    private void LoadItemsToSlots(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        int plot_slot_to_fill = 0;
        int other_slot_to_fill = 0;

        // £adowanie przedmiotów do slotów upraw
        foreach (Item item in e.plot_items_list)
        {
            plot_items_slots[plot_slot_to_fill].SetIsEmpty(false);
            plot_items_slots[plot_slot_to_fill].SetIcon(item.GetIcon());
            plot_items_slots[plot_slot_to_fill].SetItemDescritpionInSlot(item.GetItemDescription());
            plot_items_slots[plot_slot_to_fill].SetItemQuantity(e.item_amount[item.GetItemName()]);
            plot_slot_to_fill++;
        }

        // £adowanie innych przedmiotów do odpowiednich slotów
        foreach (Item item in e.other_items_list)
        {
            other_items_slots[other_slot_to_fill].SetIsEmpty(false);
            other_items_slots[other_slot_to_fill].SetIcon(item.GetIcon());
            other_items_slots[other_slot_to_fill].SetItemDescritpionInSlot(item.GetItemDescription());
            other_items_slots[other_slot_to_fill].SetItemQuantity(e.item_amount[item.GetItemName()]);
            other_slot_to_fill++;
        }
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku strza³ki w prawo i otwiera interfejs dziennika zadañ.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.QuestLog);
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku strza³ki w lewo i otwiera interfejs osi¹gniêæ.
    /// </summary>
    private void OnLeftArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Achievements);
    }
}
