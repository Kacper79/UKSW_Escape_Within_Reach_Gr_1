using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // Atrybut wskazuj¹cy, ¿e klasa mo¿e byæ zapisywana w inspektorze Unity
public class Item : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Sekcja zawieraj¹ca dane tekstowe.
    /// </summary>
    [Header("Strings")]
    [SerializeField] private string item_name;  // Nazwa przedmiotu
    [SerializeField] private string item_description;  // Opis przedmiotu

    /// <summary>
    /// Sekcja zawieraj¹ca dane graficzne.
    /// </summary>
    [Header("Graphics")]
    [SerializeField] private Sprite icon;  // Ikona przedmiotu
    [SerializeField] private Mesh model;  // Model przedmiotu

    /// <summary>
    /// Sekcja zwi¹zana z questami.
    /// </summary>
    [Header("What quest does this item finish")]
    [SerializeField] private int quest_id;  // Identyfikator questu, który ten przedmiot koñczy

    /// <summary>
    /// Flaga, która okreœla, czy przedmiot jest fabularny.
    /// </summary>
    [SerializeField] private bool is_plot;

    /// <summary>
    /// Flaga, która okreœla, czy przedmiot jest interaktywny.
    /// </summary>
    [SerializeField] private bool is_interactable = true;


    /// <summary>
    /// Metoda wywo³ywana po w³¹czeniu obiektu.
    /// </summary>
    private void OnEnable()
    {
        // Ustawienie modelu przedmiotu w przypadku, gdy ma byæ on wyœwietlany
        this.gameObject.GetComponent<MeshFilter>().mesh = model;

        // Subskrypcja zdarzenia - po wygranej ³opacie w blackjacku przedmiot staje siê interaktywny
        GlobalEvents.OnWinningPickaxeInABlackjackGame += MakeItemInteractable;
    }

    /// <summary>
    /// Metoda wywo³ywana po wy³¹czeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        // Anulowanie subskrypcji zdarzenia
        GlobalEvents.OnWinningPickaxeInABlackjackGame -= MakeItemInteractable;
    }

    /// <summary>
    /// Zdarzenie, które sprawia, ¿e przedmiot staje siê interaktywny po wygranej ³opacie.
    /// </summary>
    private void MakeItemInteractable(object sender, GlobalEvents.OnMakingGivenItemInteractableEventArgs e)
    {
        // Sprawdzamy, czy przedmiot ma tê sam¹ nazwê co ten, który powinien staæ siê interaktywny
        if (item_name == e.name)
        {
            this.is_interactable = true;  // Zmieniamy stan przedmiotu na interaktywny
        }
    }

    /// <summary>
    /// Metoda odpowiedzialna za podniesienie przedmiotu.
    /// </summary>
    private void PickUpItem()
    {
        // Wywo³anie zdarzenia informuj¹cego o podniesieniu przedmiotu
        GlobalEvents.OnPickUpItemEventArgs args = new(this);
        GlobalEvents.FireOnPickUpItem(this, args);
    }

    /// <summary>
    /// Zwraca opis przedmiotu.
    /// </summary>
    public string GetItemDescription()
    {
        return item_description;
    }

    /// <summary>
    /// Metoda wykonywana w momencie interakcji z przedmiotem (np. jego podniesienia).
    /// </summary>
    public void Interact()
    {
        // Je¿eli przedmiot jest interaktywny, mo¿emy go podnieœæ
        if (is_interactable)
        {
            PickUpItem();  // Podnosi przedmiot

            // Jeœli przedmiot jest fabularny, oznaczamy quest za ukoñczony
            if (is_plot)
            {
                QuestManager.Instance.MarkQuestCompleted(quest_id);  // Zakoñczenie questa
            }
        }
    }

    /// <summary>
    /// Metoda niszczy obiekt przedmiotu.
    /// </summary>
    public void DestroyItem()
    {
        Destroy(gameObject);  // Usuwa obiekt przedmiotu z gry
    }

    /// <summary>
    /// Zwraca nazwê przedmiotu.
    /// </summary>
    public string GetItemName()
    {
        return item_name;
    }

    /// <summary>
    /// Zwraca ikonê przedmiotu.
    /// </summary>
    public Sprite GetIcon()
    {
        return icon;
    }

    /// <summary>
    /// Zwraca, czy przedmiot jest fabularny (questowy).
    /// </summary>
    public bool GetIsPlot()
    {
        return is_plot;
    }

    /// <summary>
    /// Zwraca tekst, który ma siê pojawiæ w podpowiedzi interakcji z przedmiotem.
    /// </summary>
    public string GetInteractionTooltip()
    {
        return $"Press [E] to pickup item {item_name}";  // Tekst wyœwietlany przy interakcji
    }

    /// <summary>
    /// Metoda, któr¹ mo¿na nadpisaæ, aby dodaæ dodatkowe dzia³ania, gdy gracz patrzy na przedmiot (np. podœwietlenie).
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable()
    {
        // Nic nie robimy dla zwyk³ego przedmiotu
    }

    /// <summary>
    /// Ustawia stan interaktywnoœci przedmiotu.
    /// </summary>
    public void SetIsInteractable(bool b)
    {
        is_interactable = b;  // Zmienia stan interaktywnoœci przedmiotu
    }
}
