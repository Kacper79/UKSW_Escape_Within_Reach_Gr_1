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
    /// Przycisk strzalki w prawo, uzywany do przechodzenia do kolejnych interfejsow uzytkownika.
    /// </summary>
    [SerializeField] private Button right_arrow_button;

    /// <summary>
    /// Przycisk strzalki w lewo, uzywany do przechodzenia do poprzednich interfejsow uzytkownika.
    /// </summary>
    [SerializeField] private Button left_arrow_button;

    [Header("Scripts")]
    /// <summary>
    /// Referencja do glownego UI gracza, ktore zarzadza zasobami gracza.
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;

    [Header("Slots")]
    /// <summary>
    /// Lista slotow przechowujacych przedmioty zwiazane z fabuly, takie jak przedmioty do uprawy.
    /// </summary>
    [SerializeField] private List<Slot> plot_items_slots;

    /// <summary>
    /// Lista slotow przechowujacych inne przedmioty, ktore nie sa zwiazane z fabuly.
    /// </summary>
    [SerializeField] private List<Slot> other_items_slots;

    [Header("OtherVisuals")]
    /// <summary>
    /// Wyswietlacz, ktory pokazuje ilosc zlota gracza.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gold_display;


    /// <summary>
    /// Inicjalizuje nasluchiwanie na klikniecie przyciskow strzalek do nawigacji po UI.
    /// </summary>
    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
        left_arrow_button.onClick.AddListener(OnLeftArrowButtonClick);
    }

    /// <summary>
    /// Rejestruje subskrypcje do zdarzenia otwarcia ekwipunku oraz wyswietlania przedmiotow i zlota.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnInventoryOpenCallBack += LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack += DisplayGold;

        // Wysyla zdarzenie otwarcia ekwipunku
        GlobalEvents.FireOnInventoryOpen(this);
    }

    /// <summary>
    /// Usuwa subskrypcje do zdarzen zwiazanych z otwieraniem ekwipunku.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnInventoryOpenCallBack -= LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack -= DisplayGold;
    }

    /// <summary>
    /// Wyswietla ilosc zlota w ekwipunku gracza w interfejsie uzytkownika.
    /// </summary>
    /// <param name="sender">Obiekt, ktory wysyla zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawierajace informacje o ilosci zlota.</param>
    private void DisplayGold(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        gold_display.text = e.gold_amount.ToString();  // Ustawia tekst w UI na wartosc zlota
    }

    /// <summary>
    /// Laduje przedmioty do odpowiednich slotow w ekwipunku, w zaleznosci od typu przedmiotu.
    /// </summary>
    /// <param name="sender">Obiekt, ktory wysyla zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawierajace listy przedmiotow i ich ilosc.</param>
    private void LoadItemsToSlots(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        int plot_slot_to_fill = 0;
        int other_slot_to_fill = 0;

        // ladowanie przedmiotow do slotow upraw
        foreach (Item item in e.plot_items_list)
        {
            plot_items_slots[plot_slot_to_fill].SetIsEmpty(false);
            plot_items_slots[plot_slot_to_fill].SetIcon(item.GetIcon());
            plot_items_slots[plot_slot_to_fill].SetItemDescritpionInSlot(item.GetItemDescription());
            plot_items_slots[plot_slot_to_fill].SetItemQuantity(e.item_amount[item.GetItemName()]);
            plot_slot_to_fill++;
        }

        // ladowanie innych przedmiotow do odpowiednich slotow
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
    /// Obsluguje klikniecie przycisku strzalki w prawo i otwiera interfejs dziennika zadan.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.QuestLog);
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku strzalki w lewo i otwiera interfejs osiagniec.
    /// </summary>
    private void OnLeftArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Achievements);
    }
}
