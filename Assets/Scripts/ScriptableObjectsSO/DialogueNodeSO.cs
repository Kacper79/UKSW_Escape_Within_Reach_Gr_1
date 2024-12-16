using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Sequences.Timeline;

[CreateAssetMenu(fileName = "NewDialoguesForEachCharacterSO", menuName = "Scriptable Objects/Dialogues")]
public class DialogueNodeSO : ScriptableObject
{
    public string main_character_text = "";//Main character text
    public AudioClip main_character_audio;//Audio of character saying given line
    public float eventual_npc_response_time_delay = 0.0f;//Time after which npc responses

    public List<NpcResponses> responses;//responses, since npc might talk in multiple lines

    public List<DialogueEvent> invoked_events_list = new();//List of invoked events

    public List<DialogueNodeSO> options = new();//Possible next dialogue options

    public string id_of_dialogue_option_to_go_back_to;//Id of certain dialogue option to go back to after choosing this one

    public bool is_available = true;//Can this dialogue be shown

    public string id = "";//Id of a dialogue

    public DialogueNodeSO()
    {
        GlobalEvents.OnLookingForDialogueListWithGivenID += TryReturningOptions;
    }

    [ContextMenu("Generate IDs for this and every child object")]
    private void GenerateIDsForAllOptions()
    {
        if (id == "" || id == null)
        {
            id = System.Guid.NewGuid().ToString("N");
        }

        foreach (DialogueNodeSO option in options)
        {
            option.GenerateIDsForAllOptions();
        }
    }

    private void TryReturningOptions(object sender, GlobalEvents.OnLookingForDialogueListWithGivenIDEventArgs e)
    {
        if (id == e.given_id)
        {
            GlobalEvents.CallbackForOnLookingForDialogueListWithGivenIDEventArgs args = new(options);
            GlobalEvents.FireCallbackForOnLookingForDialogueListWithGivenID(this, args);
        }
    }

    [System.Serializable]
    public class NpcResponses//response with time delay
    {
        public string response = "";
        public AudioClip response_audio_clip;
        public float eventual_response_time_delay = 0.0f;
    }

    [System.Serializable]
    public enum DialogueEvent//Invoked events, in DialogueManager you can Invoke GlobalEvents by using it
    {
        EndDialogue,//Throw it in if you want to end dialogue
        GoBackToCertainDialogueOption//Throw it in if you want to go back to certain options
    }
}
