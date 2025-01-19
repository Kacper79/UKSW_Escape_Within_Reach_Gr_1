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
    /// Przycisk strza�ki w prawo, u�ywany do przechodzenia do kolejnych interfejs�w u�ytkownika.
    /// </summary>
    [SerializeField] private Button right_arrow_button;

    /// <summary>
    /// Przycisk strza�ki w lewo, u�ywany do przechodzenia do poprzednich interfejs�w u�ytkownika.
    /// </summary>
    [SerializeField] private Button left_arrow_button;

    [Header("Scripts")]
    /// <summary>
    /// Referencja do g��wnego UI gracza, kt�re zarz�dza zasobami gracza.
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;

    [Header("Slots")]
    /// <summary>
    /// Lista slot�w przechowuj�cych przedmioty zwi�zane z fabu��, takie jak przedmioty do uprawy.
    /// </summary>
    [SerializeField] private List<Slot> plot_items_slots;

    /// <summary>
    /// Lista slot�w przechowuj�cych inne przedmioty, kt�re nie s� zwi�zane z fabu��.
    /// </summary>
    [SerializeField] private List<Slot> other_items_slots;

    [Header("OtherVisuals")]
    /// <summary>
    /// Wy�wietlacz, kt�ry pokazuje ilo�� z�ota gracza.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gold_display;


    /// <summary>
    /// Inicjalizuje nas�uchiwanie na klikni�cie przycisk�w strza�ek do nawigacji po UI.
    /// </summary>
    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
        left_arrow_button.onClick.AddListener(OnLeftArrowButtonClick);
    }

    /// <summary>
    /// Rejestruje subskrypcj� do zdarzenia otwarcia ekwipunku oraz wy�wietlania przedmiot�w i z�ota.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnInventoryOpenCallBack += LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack += DisplayGold;

        // Wysy�a zdarzenie otwarcia ekwipunku
        GlobalEvents.FireOnInventoryOpen(this);
    }

    /// <summary>
    /// Usuwa subskrypcj� do zdarze� zwi�zanych z otwieraniem ekwipunku.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnInventoryOpenCallBack -= LoadItemsToSlots;
        GlobalEvents.OnInventoryOpenCallBack -= DisplayGold;
    }

    /// <summary>
    /// Wy�wietla ilo�� z�ota w ekwipunku gracza w interfejsie u�ytkownika.
    /// </summary>
    /// <param name="sender">Obiekt, kt�ry wys�a� zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawieraj�ce informacje o ilo�ci z�ota.</param>
    private void DisplayGold(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        gold_display.text = e.gold_amount.ToString();  // Ustawia tekst w UI na warto�� z�ota
    }

    /// <summary>
    /// �aduje przedmioty do odpowiednich slot�w w ekwipunku, w zale�no�ci od typu przedmiotu.
    /// </summary>
    /// <param name="sender">Obiekt, kt�ry wys�a� zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawieraj�ce listy przedmiot�w i ich ilo��.</param>
    private void LoadItemsToSlots(object sender, GlobalEvents.OnInventoryOpenCallBackEventArgs e)
    {
        int plot_slot_to_fill = 0;
        int other_slot_to_fill = 0;

        // �adowanie przedmiot�w do slot�w upraw
        foreach (Item item in e.plot_items_list)
        {
            plot_items_slots[plot_slot_to_fill].SetIsEmpty(false);
            plot_items_slots[plot_slot_to_fill].SetIcon(item.GetIcon());
            plot_items_slots[plot_slot_to_fill].SetItemDescritpionInSlot(item.GetItemDescription());
            plot_items_slots[plot_slot_to_fill].SetItemQuantity(e.item_amount[item.GetItemName()]);
            plot_slot_to_fill++;
        }

        // �adowanie innych przedmiot�w do odpowiednich slot�w
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
    /// Obs�uguje klikni�cie przycisku strza�ki w prawo i otwiera interfejs dziennika zada�.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.QuestLog);
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku strza�ki w lewo i otwiera interfejs osi�gni��.
    /// </summary>
    private void OnLeftArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Achievements);
    }
}
