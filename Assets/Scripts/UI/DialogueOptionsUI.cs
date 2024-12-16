using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOptionsUI : MonoBehaviour
{
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
                i++;
            }
        }

        return i;
    }
}
