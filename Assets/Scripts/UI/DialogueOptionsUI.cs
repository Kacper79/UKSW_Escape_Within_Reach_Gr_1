using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOptionsUI : MonoBehaviour
{
    private const string AGREEMENT_TO_PLAY_FOR_PICKAXE_DIALOGUE_ID = "d241bd3f97c2428e9ed357f48edebb04";
    private const int GOLD_REQUIRED_TO_PLAY_FOR_PICKAXE = 500;

    [SerializeField] private DialogueOption option_1;
    [SerializeField] private DialogueOption option_2;
    [SerializeField] private DialogueOption option_3;
    [SerializeField] private DialogueOption option_4;

    private Dictionary<int, DialogueOption> options_dict;

    private void Awake()
    {
        InitializeOptionsDict();
        DisableAllOptions();
    }

    public void DisplayOptions(List<DialogueNodeSO> options_list)
    {
        DisableAllOptions();

        EnableGivenNumberOfOptions(CalculateAvailableDialogueOptions(options_list));

        SetDataInEachOption(options_list);
    }

    private void InitializeOptionsDict()
    {
        options_dict = new();

        options_dict.Add(0, option_1);
        options_dict.Add(1, option_2);
        options_dict.Add(2, option_3);
        options_dict.Add(3, option_4);
    }

    private void EnableGivenNumberOfOptions(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            options_dict[i].gameObject.SetActive(true);
        }
    }

    public void DisableAllOptions()
    {
        for(int i = 0; i < options_dict.Count; i++)
        {
            options_dict[i].gameObject.SetActive(false);
        }
    }

    private void SetDataInEachOption(List<DialogueNodeSO> options_list)
    {
        int i = 0;

        foreach (DialogueNodeSO dialogue_option in options_list)
        {
            if(dialogue_option.is_available)
            {
                if(dialogue_option.id == AGREEMENT_TO_PLAY_FOR_PICKAXE_DIALOGUE_ID && FindObjectOfType<InventoryManager>().GetGoldAmount() < GOLD_REQUIRED_TO_PLAY_FOR_PICKAXE)
                {
                    Debug.Log("Jestem tu");
                    continue;
                }

                options_dict[i].SetInfo(dialogue_option.id, i + 1, dialogue_option.main_character_text);

                i++;
            }
        }
    }

    private int CalculateAvailableDialogueOptions(List<DialogueNodeSO> options_list)
    {
        int i = 0;

        foreach(DialogueNodeSO dialogue_option in options_list)
        {
            if(dialogue_option.is_available)
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
