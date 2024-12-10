using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sequences.Timeline;

[CreateAssetMenu(fileName = "NewDialoguesForEachCharacterSO", menuName = "Scriptable Objects/Dialogues")]
public class DialogueNodeSO : ScriptableObject
{
    public string id = "";//Id of a dialogue

    public string text = "";//Main character text
    public float response_time_delay = 0.0f;//Time after which npc responses

    public List<string> responses = new();//List of responses, since npc's speech might be longer than one line
    public List<float> next_response_time_delay = new();//Each delay, after the next response/next dialogue option is started

    public List<DialogueNodeSO> options = new();//Possible next dialogue options

    public bool is_available = true;//Can this dialogue be shown

    public List<DialogueEvent> invoked_events_list = new();//List of invoked events

    public enum DialogueEvent//Invoked events, in DialogueManager you can Invoke GlobalEvents by using it
    {
        BlackjackPickaxeWon,
        BlackjackGoldWon,
        BlackjackGoldLost
    }

    [ContextMenu("Generate ID")]
    private void GenerateIDsForAllOptions()
    {
        id = System.Guid.NewGuid().ToString("N");
    }
}
