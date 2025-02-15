using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const string ITEM_QUANTITY_TEXT = "ilosc: ";

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI item_description_display;
    [SerializeField] private TextMeshProUGUI item_quantity_display;

    [Header("Icons")]
    [SerializeField] private Image item_icon_description_display;
    [SerializeField] private Image item_icon_slot_display;

    // Flaga wskazujaca, czy slot jest pusty
    private bool is_empty = true;
    // Opis przedmiotu w slocie
    private string item_description = "";
    // Ilosc przedmiotow w slocie
    private int item_quantity = 0;

    /// <summary>
    /// Inicjalizuje poczatkowy stan slotu (ukrywa informacje o przedmiocie).
    /// </summary>
    private void OnEnable()
    {
        item_description_display.gameObject.SetActive(false);
        item_icon_description_display.gameObject.SetActive(false);
        item_quantity_display.gameObject.SetActive(false);
    }

    /// <summary>
    /// Resetuje informacje o przedmiocie, jesli slot nie jest pusty.
    /// </summary>
    public void OnDisable()
    {
        if (!is_empty)
        {
            item_description_display.GetComponent<TextMeshProUGUI>().text = "";
            item_quantity_display.GetComponent<TextMeshProUGUI>().text = "";
            item_icon_description_display.GetComponent<Image>().sprite = null;
            item_description_display.gameObject.SetActive(false);
            item_quantity_display.gameObject.SetActive(false);
            item_icon_description_display.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Obsluguje zdarzenie, gdy wskaznik myszki wchodzi na slot. Wyswietla informacje o przedmiocie.
    /// </summary>
    /// <param name="eventData">Dane zdarzenia.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!is_empty)
        {
            item_description_display.GetComponent<TextMeshProUGUI>().text = item_description;
            item_quantity_display.GetComponent<TextMeshProUGUI>().text = ITEM_QUANTITY_TEXT + item_quantity.ToString();
            item_icon_description_display.GetComponent<Image>().sprite = item_icon_slot_display.sprite;
            item_quantity_display.gameObject.SetActive(true);
            item_description_display.gameObject.SetActive(true);
            item_icon_description_display.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Obsluguje zdarzenie, gdy wskaznik myszki wychodzi z slotu. Ukrywa informacje o przedmiocie.
    /// </summary>
    /// <param name="eventData">Dane zdarzenia.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!is_empty)
        {
            item_description_display.GetComponent<TextMeshProUGUI>().text = "";
            item_quantity_display.GetComponent<TextMeshProUGUI>().text = "";
            item_icon_description_display.GetComponent<Image>().sprite = null;
            item_description_display.gameObject.SetActive(false);
            item_quantity_display.gameObject.SetActive(false);
            item_icon_description_display.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Ustawia ikone przedmiotu w slocie.
    /// </summary>
    /// <param name="icon">Ikona przedmiotu.</param>
    public void SetIcon(Sprite icon)
    {
        item_icon_slot_display.gameObject.SetActive(true);
        item_icon_slot_display.sprite = icon;
    }

    /// <summary>
    /// Zwraca opis przedmiotu w slocie.
    /// </summary>
    /// <returns>Opis przedmiotu.</returns>
    public string GetItemDescription()
    {
        return item_description;
    }

    /// <summary>
    /// Ustawia opis przedmiotu w slocie.
    /// </summary>
    /// <param name="description">Opis przedmiotu.</param>
    public void SetItemDescritpionInSlot(string description)
    {
        item_description = description;
    }

    /// <summary>
    /// Sprawdza, czy slot jest pusty.
    /// </summary>
    /// <returns>True, jesli slot jest pusty; w przeciwnym razie false.</returns>
    public bool GetIsEmpty()
    {
        return is_empty;
    }

    /// <summary>
    /// Ustawia, czy slot jest pusty.
    /// </summary>
    /// <param name="b">True, jesli slot ma byc pusty, false jesli nie.</param>
    public void SetIsEmpty(bool b)
    {
        is_empty = b;
    }

    /// <summary>
    /// Ustawia ilosc przedmiotow w slocie.
    /// </summary>
    /// <param name="quantity">Ilosc przedmiotow.</param>
    public void SetItemQuantity(int quantity)
    {
        item_quantity = quantity;
    }
}
