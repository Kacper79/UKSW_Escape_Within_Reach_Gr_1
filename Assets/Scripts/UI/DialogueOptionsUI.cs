using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie opcjami dialogowymi w UI.
/// </summary>
public class DialogueOptionsUI : MonoBehaviour
{
    /// <summary>
    /// Stala identyfikujaca dialog, ktory pyta o zgode na gre o kilof.
    /// </summary>
    private const string AGREEMENT_TO_PLAY_FOR_PICKAXE_DIALOGUE_ID = "d241bd3f97c2428e9ed357f48edebb04";

    /// <summary>
    /// Stala okreslajaca wymagana ilosc zlota, by zagraæ o kilof.
    /// </summary>
    private const int GOLD_REQUIRED_TO_PLAY_FOR_PICKAXE = 500;

    /// <summary>
    /// Opcja dialogowa nr 1.
    /// </summary>
    [SerializeField] private DialogueOption option_1;

    /// <summary>
    /// Opcja dialogowa nr 2.
    /// </summary>
    [SerializeField] private DialogueOption option_2;

    /// <summary>
    /// Opcja dialogowa nr 3.
    /// </summary>
    [SerializeField] private DialogueOption option_3;

    /// <summary>
    /// Opcja dialogowa nr 4.
    /// </summary>
    [SerializeField] private DialogueOption option_4;

    /// <summary>
    /// Slownik, ktory przechowuje opcje dialogowe powiazane z numerami.
    /// </summary>
    private Dictionary<int, DialogueOption> options_dict;

    /// <summary>
    /// Inicjalizuje slownik opcji dialogowych i ukrywa wszystkie opcje.
    /// </summary>
    private void Awake()
    {
        InitializeOptionsDict();
        DisableAllOptions();
    }

    /// <summary>
    /// Wyswietla opcje dialogowe na podstawie listy dostepnych opcji.
    /// </summary>
    /// <param name="options_list">Lista dostepnych opcji dialogowych.</param>
    public void DisplayOptions(List<DialogueNodeSO> options_list)
    {
        DisableAllOptions();

        EnableGivenNumberOfOptions(CalculateAvailableDialogueOptions(options_list));

        SetDataInEachOption(options_list);
    }

    /// <summary>
    /// Inicjalizuje slownik opcji dialogowych z przypisaniem numerow do obiektow.
    /// </summary>
    private void InitializeOptionsDict()
    {
        options_dict = new();

        options_dict.Add(0, option_1);
        options_dict.Add(1, option_2);
        options_dict.Add(2, option_3);
        options_dict.Add(3, option_4);
    }

    /// <summary>
    /// Umozliwia okreslona liczbe opcji dialogowych.
    /// </summary>
    /// <param name="amount">Liczba opcji do aktywowania.</param>
    private void EnableGivenNumberOfOptions(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            options_dict[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Ukrywa wszystkie opcje dialogowe.
    /// </summary>
    public void DisableAllOptions()
    {
        for (int i = 0; i < options_dict.Count; i++)
        {
            options_dict[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Ustawia dane dla kazdej opcji dialogowej.
    /// </summary>
    /// <param name="options_list">Lista dostepnych opcji dialogowych.</param>
    private void SetDataInEachOption(List<DialogueNodeSO> options_list)
    {
        int i = 0;

        foreach (DialogueNodeSO dialogue_option in options_list)
        {
            if (dialogue_option.is_available)
            {
                if (dialogue_option.id == AGREEMENT_TO_PLAY_FOR_PICKAXE_DIALOGUE_ID && FindObjectOfType<InventoryManager>().GetGoldAmount() < GOLD_REQUIRED_TO_PLAY_FOR_PICKAXE)
                {
                    Debug.Log("Jestem tu");
                    continue;
                }

                options_dict[i].SetInfo(dialogue_option.id, i + 1, dialogue_option.main_character_text);

                i++;
            }
        }
    }

    /// <summary>
    /// Oblicza liczbe dostepnych opcji dialogowych w danej liœcie.
    /// </summary>
    /// <param name="options_list">Lista dostepnych opcji dialogowych.</param>
    /// <returns>Liczba dostepnych opcji dialogowych.</returns>
    private int CalculateAvailableDialogueOptions(List<DialogueNodeSO> options_list)
    {
        int i = 0;

        foreach (DialogueNodeSO dialogue_option in options_list)
        {
            if (dialogue_option.is_available)
            {
                if (dialogue_option.id == AGREEMENT_TO_PLAY_FOR_PICKAXE_DIALOGUE_ID && FindObjectOfType<InventoryManager>().GetGoldAmount() < GOLD_REQUIRED_TO_PLAY_FOR_PICKAXE)
                {
                    Debug.Log("Jestem tu w liczeniu");
                    continue;
                }

                i++;
            }
        }

        return i;
    }
}
