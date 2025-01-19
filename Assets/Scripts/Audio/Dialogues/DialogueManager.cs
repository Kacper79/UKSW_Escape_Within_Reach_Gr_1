using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private const float DELAY_EPSILON_BETWEEN_EACH_DIALOGUE_PROC = 0.02f;
    private const string MAIN_CHARACTER_NAME = "MAIN CHARACTER NAME";

    public static DialogueManager Instance { get; private set; }

    private DialogueNodeSO current_dialogue_root;
    private List<DialogueNodeSO> current_dialogue_list = new();

    private DialogueOptionsUI dialogue_options_ui;
    private SpokenTextDisplayUI spoken_text_display_ui;

    private string current_npc_name;//npc name that we talk to

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Another instance of SingletonExample exists! Destroying this one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        dialogue_options_ui = FindObjectOfType<DialogueOptionsUI>();
        spoken_text_display_ui = FindObjectOfType<SpokenTextDisplayUI>();

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GlobalEvents.OnChoosingCertainDialogueOption += ChosenGivenOption;
        GlobalEvents.CallbackForOnLookingForDialogueListWithGivenID += GoBackToCertainDialogueOptionsCallbacked;
    }

    private void OnDisable()
    {
        GlobalEvents.OnChoosingCertainDialogueOption -= ChosenGivenOption;
        GlobalEvents.CallbackForOnLookingForDialogueListWithGivenID -= GoBackToCertainDialogueOptionsCallbacked;
    }

    public void StartDialogue(DialogueNodeSO dialogue_root, string npc_name)
    {
        Cursor.lockState = CursorLockMode.None;
        GlobalEvents.FireOnTimeStop(this);

        GlobalEvents.FireOnStartingDialogue(this);

        current_npc_name = npc_name;
        current_dialogue_root = ScriptableObject.Instantiate(dialogue_root);

        current_dialogue_list = new();
        current_dialogue_list.Add(dialogue_root);

        dialogue_options_ui.DisplayOptions(current_dialogue_list);
    }

    private void GoBackToCertainDialogueOptionsCallbacked(object sender, GlobalEvents.CallbackForOnLookingForDialogueListWithGivenIDEventArgs e)
    {
        current_dialogue_list = e.list_of_options;
        dialogue_options_ui.DisplayOptions(current_dialogue_list);
    }

    private void ChosenGivenOption(object sender, GlobalEvents.OnChoosingCertainDialogueOptionEventArgs e)
    {
        DialogueNodeSO choosen_option = GetChoosenOptionBasedOnId(e.choosen_option_id);

        dialogue_options_ui.DisableAllOptions();

        StartCoroutine(DisplayDialogueOnScreen(choosen_option));
    }

    private IEnumerator DisplayDialogueOnScreen(DialogueNodeSO choosen_option)
    {
        spoken_text_display_ui.DisplayText(choosen_option.main_character_text, MAIN_CHARACTER_NAME);

        AudioManager.Instance.PlayGivenClip(choosen_option.main_character_audio);

        yield return new WaitForSeconds(choosen_option.main_character_audio.length + choosen_option.eventual_npc_response_time_delay + DELAY_EPSILON_BETWEEN_EACH_DIALOGUE_PROC);

        foreach(DialogueNodeSO.NpcResponses response in choosen_option.responses)
        {
            spoken_text_display_ui.DisplayText(response.response, current_npc_name);
            AudioManager.Instance.PlayGivenClip(response.response_audio_clip);
            yield return new WaitForSeconds(response.response_audio_clip.length + response.eventual_response_time_delay + DELAY_EPSILON_BETWEEN_EACH_DIALOGUE_PROC);
        }

        StuffToBeDoneAfterNpcFinishes(choosen_option);
    }

    private void StuffToBeDoneAfterNpcFinishes(DialogueNodeSO choosen_option)
    {
        spoken_text_display_ui.DisableAllComponents();
        // TODO: if choosen option options list is non-existant something has to be done e.g leave dialogue, or go back to previous node
        if(choosen_option.options.Count != 0)
        {
            current_dialogue_list = choosen_option.options;
            dialogue_options_ui.DisplayOptions(current_dialogue_list);
        }
        InvokeEventsOnChoosingCertainOptions(choosen_option);
    }

    private void InvokeEventsOnChoosingCertainOptions(DialogueNodeSO option)
    {
        foreach(DialogueNodeSO.DialogueEvent each_event in option.invoked_events_list)//Some events have to be controlled differently than others, hence if-statements
        {
            if(each_event == DialogueNodeSO.DialogueEvent.GoBackToCertainDialogueOption)
            {
                if(option.id_of_dialogue_option_to_go_back_to == current_dialogue_root.id)
                {
                    current_dialogue_list = current_dialogue_root.options;
                    dialogue_options_ui.DisplayOptions(current_dialogue_list);
                }

                GlobalEvents.OnLookingForDialogueListWithGivenIDEventArgs args = new(option.id_of_dialogue_option_to_go_back_to);
                GlobalEvents.FireOnLookingForDialogueListWithGivenID(this, args);
            }
            else if(each_event == DialogueNodeSO.DialogueEvent.EndDialogue)
            {
                Cursor.lockState = CursorLockMode.Locked;

                current_dialogue_root = null;
                current_dialogue_list = null;

                GlobalEvents.FireOnEndingDialogue(this);
                GlobalEvents.FireOnTimeStart(this);
            }
            else if(each_event == DialogueNodeSO.DialogueEvent.MakePayoff)
            {
                GlobalEvents.FireOnPayoff(this, new(option.payoffAmount, current_npc_name));
            }
            else if(each_event == DialogueNodeSO.DialogueEvent.CompleteQuest)
            {
                if(option.questNumber != 0) QuestManager.Instance.MarkQuestCompleted(option.questNumber);
            }
            else
            {
                GlobalEvents.FireCertainDialogueEvent(this, each_event);
            }
        }
    }

    private DialogueNodeSO GetChoosenOptionBasedOnId(string id)
    {
        foreach(DialogueNodeSO option in current_dialogue_list)
        {
            if(option.id == id)
            {
                return option;
            }
        }

        Debug.Log("Dupa, nie powinno Cie tu byc :/");
        return null;
    }
}
