using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa trzymajaca wszystkie potrzebne rzeczy, aby
/// przedmioty pokazywaly sie do podniesienia lub w ekwipunku
/// </summary>
[Serializable]
public class Item : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Sekcja zawierajaca dane tekstowe.
    /// </summary>
    [Header("Strings")]
    [SerializeField] private string item_name;  // Nazwa przedmiotu
    [SerializeField] private string item_description;  // Opis przedmiotu

    /// <summary>
    /// Sekcja zawierajaca dane graficzne.
    /// </summary>
    [Header("Graphics")]
    [SerializeField] private Sprite icon;  // Ikona przedmiotu
    [SerializeField] private Mesh model;  // Model przedmiotu

    /// <summary>
    /// Sekcja zwiazana z questami.
    /// </summary>
    [Header("What quest does this item finish")]
    [SerializeField] private int quest_id;  // Identyfikator questu, ktory ten przedmiot konczy

    /// <summary>
    /// Flaga, ktora okresla, czy przedmiot jest fabularny.
    /// </summary>
    [SerializeField] private bool is_plot;

    /// <summary>
    /// Flaga, ktora okresla, czy przedmiot jest interaktywny.
    /// </summary>
    [SerializeField] private bool is_interactable = true;


    /// <summary>
    /// Metoda wywolywana po wlaczeniu obiektu.
    /// </summary>
    private void OnEnable()
    {
        // Ustawienie modelu przedmiotu w przypadku, gdy ma byc on wyswietlany
        this.gameObject.GetComponent<MeshFilter>().mesh = model;

        // Subskrypcja zdarzenia - po wygranej lopacie w blackjacku przedmiot staje sie interaktywny
        GlobalEvents.OnWinningPickaxeInABlackjackGame += MakeItemInteractable;
    }

    /// <summary>
    /// Metoda wywolywana po wylaczeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        // Anulowanie subskrypcji zdarzenia
        GlobalEvents.OnWinningPickaxeInABlackjackGame -= MakeItemInteractable;
    }

    /// <summary>
    /// Zdarzenie, ktore sprawia, ze przedmiot staje sie interaktywny po wygranej lopacie.
    /// </summary>
    private void MakeItemInteractable(object sender, GlobalEvents.OnMakingGivenItemInteractableEventArgs e)
    {
        // Sprawdzamy, czy przedmiot ma te sama nazwe co ten, ktory powinien stac sie interaktywny
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
        // Wywolanie zdarzenia informujacego o podniesieniu przedmiotu
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
        // Jezeli przedmiot jest interaktywny, mozemy go podniesc
        if (is_interactable)
        {
            PickUpItem();  // Podnosi przedmiot

            // Jesli przedmiot jest fabularny, oznaczamy quest za ukonczony
            if (is_plot || quest_id == 3)
            {
                QuestManager.Instance.MarkQuestCompleted(quest_id);  // Zakonczenie questa
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
    /// Zwraca nazwe przedmiotu.
    /// </summary>
    public string GetItemName()
    {
        return item_name;
    }

    /// <summary>
    /// Zwraca ikone przedmiotu.
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
    /// Zwraca tekst, ktory ma sie pojawic w podpowiedzi interakcji z przedmiotem.
    /// </summary>
    public string GetInteractionTooltip()
    {
        return $"Press [E] to pickup item {item_name}";  // Tekst wyswietlany przy interakcji
    }

    /// <summary>
    /// Metoda, ktora mozna nadpisac, aby dodac dodatkowe dzialania, gdy gracz patrzy na przedmiot (np. podswietlenie).
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable()
    {
        // Nic nie robimy dla zwykzego przedmiotu
    }

    /// <summary>
    /// Ustawia stan interaktywnosci przedmiotu.
    /// </summary>
    public void SetIsInteractable(bool b)
    {
        is_interactable = b;  // Zmienia stan interaktywnosci przedmiotu
    }
}
