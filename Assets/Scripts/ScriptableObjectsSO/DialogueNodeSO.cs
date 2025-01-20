using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialoguesForEachCharacterSO", menuName = "Scriptable Objects/Dialogues")]
/// <summary>
/// Klasa reprezentujaca dane dialogowe dla kazdego bohatera, zawierajaca tekst, odpowiedzi NPC i inne ustawienia.
/// </summary>
public class DialogueNodeSO : ScriptableObject
{
    /// <summary>
    /// Tekst glownego bohatera.
    /// </summary>
    public string main_character_text = "";

    /// <summary>
    /// Audio dla tekstu glownego bohatera.
    /// </summary>
    public AudioClip main_character_audio;

    /// <summary>
    /// Czas oponienia odpowiedzi NPC po wypowiedzi glownego bohatera.
    /// </summary>
    public float eventual_npc_response_time_delay = 0.0f;

    /// <summary>
    /// Lista odpowiedzi NPC, poniewaz NPC moze mowic w kilku linijkach.
    /// </summary>
    public List<NpcResponses> responses;

    /// <summary>
    /// Lista wydarzen wywolywanych podczas dialogu.
    /// </summary>
    public List<DialogueEvent> invoked_events_list = new();

    /// <summary>
    /// Mozliwe nastepne opcje dialogowe.
    /// </summary>
    public List<DialogueNodeSO> options = new();

    /// <summary>
    /// Identyfikator opcji dialogowej, do ktorej mozna wrocic po wybraniu tej opcji.
    /// </summary>
    public string id_of_dialogue_option_to_go_back_to;

    /// <summary>
    /// Flaga okreslajaca, czy ten dialog jest dostepny.
    /// </summary>
    public bool is_available = true;

    /// <summary>
    /// Identyfikator dialogu.
    /// </summary>
    public string id = "";

    public int payoffAmount; //cost of the payoff if this dialogue is a payoff
    public int questNumber; //number of the completed quest if this dialogue completes quest

    public DialogueNodeSO()
    {
        GlobalEvents.OnLookingForDialogueListWithGivenID += TryReturningOptions;
        GlobalEvents.OnMakingGivenDialogueOptionAvailableOrUnavailable += ChangeISAvailableValue;
    }

    /// <summary>
    /// Zmienia wartosc dostepnosci dialogu w zaleznosci od argumentow.
    /// </summary>
    /// <param name="sender">Obiekt wywolujacy zdarzenie.</param>
    /// <param name="args">Argumenty zawierajace nowe ustawienie dostepnosci.</param>
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
    /// Probuj zwrocic dostepne opcje dialogowe dla danego ID.
    /// </summary>
    /// <param name="sender">Obiekt wywolujacy zdarzenie.</param>
    /// <param name="e">Argumenty zawierajace poszukiwane ID.</param>
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
    /// Klasa reprezentujaca odpowiedzi NPC, z opoznieniem czasowym.
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
        /// Czas oponienia odpowiedzi NPC.
        /// </summary>
        public float eventual_response_time_delay = 0.0f;
    }

    [System.Serializable]
    /// <summary>
    /// Enums dla roznych zdarzen wywolywanych podczas dialogu.
    /// </summary>
    public enum DialogueEvent
    {
        EndDialogue,
        GoBackToCertainDialogueOption,
        StartFightingTournament,
        StartBlackJackGameForMoney,
        StartBlackJackGameForPickaxe,
        CompleteQuest,
        MakePayoff
    }
}
