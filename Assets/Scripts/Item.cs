using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // Atrybut wskazuj�cy, �e klasa mo�e by� zapisywana w inspektorze Unity
public class Item : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Sekcja zawieraj�ca dane tekstowe.
    /// </summary>
    [Header("Strings")]
    [SerializeField] private string item_name;  // Nazwa przedmiotu
    [SerializeField] private string item_description;  // Opis przedmiotu

    /// <summary>
    /// Sekcja zawieraj�ca dane graficzne.
    /// </summary>
    [Header("Graphics")]
    [SerializeField] private Sprite icon;  // Ikona przedmiotu
    [SerializeField] private Mesh model;  // Model przedmiotu

    /// <summary>
    /// Sekcja zwi�zana z questami.
    /// </summary>
    [Header("What quest does this item finish")]
    [SerializeField] private int quest_id;  // Identyfikator questu, kt�ry ten przedmiot ko�czy

    /// <summary>
    /// Flaga, kt�ra okre�la, czy przedmiot jest fabularny.
    /// </summary>
    [SerializeField] private bool is_plot;

    /// <summary>
    /// Flaga, kt�ra okre�la, czy przedmiot jest interaktywny.
    /// </summary>
    [SerializeField] private bool is_interactable = true;


    /// <summary>
    /// Metoda wywo�ywana po w��czeniu obiektu.
    /// </summary>
    private void OnEnable()
    {
        // Ustawienie modelu przedmiotu w przypadku, gdy ma by� on wy�wietlany
        this.gameObject.GetComponent<MeshFilter>().mesh = model;

        // Subskrypcja zdarzenia - po wygranej �opacie w blackjacku przedmiot staje si� interaktywny
        GlobalEvents.OnWinningPickaxeInABlackjackGame += MakeItemInteractable;
    }

    /// <summary>
    /// Metoda wywo�ywana po wy��czeniu obiektu.
    /// </summary>
    private void OnDisable()
    {
        // Anulowanie subskrypcji zdarzenia
        GlobalEvents.OnWinningPickaxeInABlackjackGame -= MakeItemInteractable;
    }

    /// <summary>
    /// Zdarzenie, kt�re sprawia, �e przedmiot staje si� interaktywny po wygranej �opacie.
    /// </summary>
    private void MakeItemInteractable(object sender, GlobalEvents.OnMakingGivenItemInteractableEventArgs e)
    {
        // Sprawdzamy, czy przedmiot ma t� sam� nazw� co ten, kt�ry powinien sta� si� interaktywny
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
        // Wywo�anie zdarzenia informuj�cego o podniesieniu przedmiotu
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
        // Je�eli przedmiot jest interaktywny, mo�emy go podnie��
        if (is_interactable)
        {
            PickUpItem();  // Podnosi przedmiot

            // Je�li przedmiot jest fabularny, oznaczamy quest za uko�czony
            if (is_plot)
            {
                QuestManager.Instance.MarkQuestCompleted(quest_id);  // Zako�czenie questa
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
    /// Zwraca nazw� przedmiotu.
    /// </summary>
    public string GetItemName()
    {
        return item_name;
    }

    /// <summary>
    /// Zwraca ikon� przedmiotu.
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
    /// Zwraca tekst, kt�ry ma si� pojawi� w podpowiedzi interakcji z przedmiotem.
    /// </summary>
    public string GetInteractionTooltip()
    {
        return $"Press [E] to pickup item {item_name}";  // Tekst wy�wietlany przy interakcji
    }

    /// <summary>
    /// Metoda, kt�r� mo�na nadpisa�, aby doda� dodatkowe dzia�ania, gdy gracz patrzy na przedmiot (np. pod�wietlenie).
    /// </summary>
    public void AdditionalStuffWhenLookingAtInteractable()
    {
        // Nic nie robimy dla zwyk�ego przedmiotu
    }

    /// <summary>
    /// Ustawia stan interaktywno�ci przedmiotu.
    /// </summary>
    public void SetIsInteractable(bool b)
    {
        is_interactable = b;  // Zmienia stan interaktywno�ci przedmiotu
    }
}
