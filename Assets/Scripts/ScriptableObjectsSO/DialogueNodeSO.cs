using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialoguesForEachCharacterSO", menuName = "Scriptable Objects/Dialogues")]
/// <summary>
/// Klasa reprezentuj¹ca dane dialogowe dla ka¿dego bohatera, zawieraj¹ca tekst, odpowiedzi NPC i inne ustawienia.
/// </summary>
public class DialogueNodeSO : ScriptableObject
{
    /// <summary>
    /// Tekst g³ównego bohatera.
    /// </summary>
    public string main_character_text = "";

    /// <summary>
    /// Audio dla tekstu g³ównego bohatera.
    /// </summary>
    public AudioClip main_character_audio;

    /// <summary>
    /// Czas opóŸnienia odpowiedzi NPC po wypowiedzi g³ównego bohatera.
    /// </summary>
    public float eventual_npc_response_time_delay = 0.0f;

    /// <summary>
    /// Lista odpowiedzi NPC, poniewa¿ NPC mo¿e mówiæ w kilku linijkach.
    /// </summary>
    public List<NpcResponses> responses;

    /// <summary>
    /// Lista wydarzeñ wywo³ywanych podczas dialogu.
    /// </summary>
    public List<DialogueEvent> invoked_events_list = new();

    /// <summary>
    /// Mo¿liwe nastêpne opcje dialogowe.
    /// </summary>
    public List<DialogueNodeSO> options = new();

    /// <summary>
    /// Identyfikator opcji dialogowej, do której mo¿na wróciæ po wybraniu tej opcji.
    /// </summary>
    public string id_of_dialogue_option_to_go_back_to;

    /// <summary>
    /// Flaga okreœlaj¹ca, czy ten dialog jest dostêpny.
    /// </summary>
    public bool is_available = true;

    /// <summary>
    /// Identyfikator dialogu.
    /// </summary>
    public string id = "";

    public DialogueNodeSO()
    {
        GlobalEvents.OnLookingForDialogueListWithGivenID += TryReturningOptions;
        GlobalEvents.OnMakingGivenDialogueOptionAvailableOrUnavailable += ChangeISAvailableValue;
    }

    /// <summary>
    /// Zmienia wartoœæ dostêpnoœci dialogu w zale¿noœci od argumentów.
    /// </summary>
    /// <param name="sender">Obiekt wywo³uj¹cy zdarzenie.</param>
    /// <param name="args">Argumenty zawieraj¹ce nowe ustawienie dostêpnoœci.</param>
    private void ChangeISAvailableValue(object sender, GlobalEvents.OnMakingGivenDialogueOptionAvailableOrUnavailableEventArgs args)
    {
        if (id == args.dialogue_id)
        {
            is_available = args.new_bool_value;
        }
    }

    /// <summary>
    /// Generuje unikalne identyfikatory dla tego dialogu oraz wszystkich jego opcji.
    /// </summary>
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

    /// <summary>
    /// Próbuj zwróciæ dostêpne opcje dialogowe dla danego ID.
    /// </summary>
    /// <param name="sender">Obiekt wywo³uj¹cy zdarzenie.</param>
    /// <param name="e">Argumenty zawieraj¹ce poszukiwane ID.</param>
    private void TryReturningOptions(object sender, GlobalEvents.OnLookingForDialogueListWithGivenIDEventArgs e)
    {
        if (id == e.given_id)
        {
            GlobalEvents.CallbackForOnLookingForDialogueListWithGivenIDEventArgs args = new(options);
            GlobalEvents.FireCallbackForOnLookingForDialogueListWithGivenID(this, args);
        }
    }

    [System.Serializable]
    /// <summary>
    /// Klasa reprezentuj¹ca odpowiedzi NPC, z opóŸnieniem czasowym.
    /// </summary>
    public class NpcResponses
    {
        /// <summary>
        /// Tekst odpowiedzi NPC.
        /// </summary>
        public string response = "";

        /// <summary>
        /// Audio odpowiedzi NPC.
        /// </summary>
        public AudioClip response_audio_clip;

        /// <summary>
        /// Czas opóŸnienia odpowiedzi NPC.
        /// </summary>
        public float eventual_response_time_delay = 0.0f;
    }

    [System.Serializable]
    /// <summary>
    /// Enums dla ró¿nych zdarzeñ wywo³ywanych podczas dialogu.
    /// </summary>
    public enum DialogueEvent
    {
        EndDialogue,
        GoBackToCertainDialogueOption,
        StartFightingTournament,
        StartBlackJackGameForMoney,
        StartBlackJackGameForPickaxe
    }
}
