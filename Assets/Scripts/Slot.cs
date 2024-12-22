using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const string ITEM_QUANTITY_TEXT = "iloœæ: ";

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI item_description_display;
    [SerializeField] private TextMeshProUGUI item_quantity_display;

    [Header("Icons")]
    [SerializeField] private Image item_icon_description_display;
    [SerializeField] private Image item_icon_slot_display;

    private bool is_empty = true;
    private string item_description = "";
    private int item_quantity = 0;

    private void OnEnable()
    {
        item_description_display.gameObject.SetActive(false);
        item_icon_description_display.gameObject.SetActive(false);
        item_quantity_display.gameObject.SetActive(false);
    }

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

    public void SetIcon(Sprite icon)
    {
        item_icon_slot_display.gameObject.SetActive(true);
        item_icon_slot_display.sprite = icon;
    }

    public string GetItemDescription()
    {
        return item_description;
    }
    public void SetItemDescritpionInSlot(string description)
    {
        item_description = description;
    }
    public bool GetIsEmpty()
    {
        return is_empty;
    }

    public void SetIsEmpty(bool b)
    {
        is_empty = b;
    }
    public void SetItemQuantity(int quantity)
    {
        item_quantity = quantity;
    }
}
