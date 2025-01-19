using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie opcjami dialogowymi w UI.
/// </summary>
public class DialogueOptionsUI : MonoBehaviour
{
    /// <summary>
    /// Sta�a identyfikuj�ca dialog, kt�ry pyta o zgod� na gr� o kilof.
    /// </summary>
    private const string AGREEMENT_TO_PLAY_FOR_PICKAXE_DIALOGUE_ID = "d241bd3f97c2428e9ed357f48edebb04";

    /// <summary>
    /// Sta�a okre�laj�ca wymagan� ilo�� z�ota, by zagra� o kilof.
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
    /// S�ownik, kt�ry przechowuje opcje dialogowe powi�zane z numerami.
    /// </summary>
    private Dictionary<int, DialogueOption> options_dict;

    /// <summary>
    /// Inicjalizuje s�ownik opcji dialogowych i ukrywa wszystkie opcje.
    /// </summary>
    private void Awake()
    {
        InitializeOptionsDict();
        DisableAllOptions();
    }

    /// <summary>
    /// Wy�wietla opcje dialogowe na podstawie listy dost�pnych opcji.
    /// </summary>
    /// <param name="options_list">Lista dost�pnych opcji dialogowych.</param>
    public void DisplayOptions(List<DialogueNodeSO> options_list)
    {
        DisableAllOptions();

        EnableGivenNumberOfOptions(CalculateAvailableDialogueOptions(options_list));

        SetDataInEachOption(options_list);
    }

    /// <summary>
    /// Inicjalizuje s�ownik opcji dialogowych z przypisaniem numer�w do obiekt�w.
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
    /// Umo�liwia okre�lon� liczb� opcji dialogowych.
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
    /// Ustawia dane dla ka�dej opcji dialogowej.
    /// </summary>
    /// <param name="options_list">Lista dost�pnych opcji dialogowych.</param>
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
    /// Oblicza liczb� dost�pnych opcji dialogowych w danej li�cie.
    /// </summary>
    /// <param name="options_list">Lista dost�pnych opcji dialogowych.</param>
    /// <returns>Liczba dost�pnych opcji dialogowych.</returns>
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
