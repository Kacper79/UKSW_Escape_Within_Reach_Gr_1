using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Zarzadza systemem dialogow w grze.
/// Obsluguje wyswietlanie opcji dialogowych, przetwarzanie wyborow gracza oraz odtwarzanie dzwiekow i tekstow.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Opoznienie pomiedzy kazda procedura dialogowa.
    /// </summary>
    private const float DELAY_EPSILON_BETWEEN_EACH_DIALOGUE_PROC = 0.02f;

    /// <summary>
    /// Nazwa glownego bohatera, wyswietlana w dialogach.
    /// </summary>
    private const string MAIN_CHARACTER_NAME = "MAIN CHARACTER NAME";

    /// <summary>
    /// Statyczna instancja klasy DialogueManager (singleton).
    /// </summary>
    public static DialogueManager Instance { get; private set; }

    /// <summary>
    /// Korzen aktualnie odtwarzanego dialogu.
    /// </summary>
    private DialogueNodeSO current_dialogue_root;

    /// <summary>
    /// Lista aktualnych opcji dialogowych.
    /// </summary>
    private List<DialogueNodeSO> current_dialogue_list = new();

    /// <summary>
    /// Interfejs wyswietlania opcji dialogowych.
    /// </summary>
    private DialogueOptionsUI dialogue_options_ui;

    /// <summary>
    /// Interfejs wyswietlania tekstu dialogowego.
    /// </summary>
    private SpokenTextDisplayUI spoken_text_display_ui;

    /// <summary>
    /// Nazwa NPC, z ktorym aktualnie rozmawiamy.
    /// </summary>
    private string current_npc_name;

    /// <summary>
    /// Inicjalizuje singleton oraz wyszukuje elementy UI zwiazane z dialogiem.
    /// </summary>
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

    /// <summary>
    /// Subskrybuje globalne zdarzenia wymagane do obslugi dialogow.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnChoosingCertainDialogueOption += ChosenGivenOption;
        GlobalEvents.CallbackForOnLookingForDialogueListWithGivenID += GoBackToCertainDialogueOptionsCallbacked;
    }

    /// <summary>
    /// Wyrejestrowuje globalne zdarzenia wymagane do obslugi dialogow.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnChoosingCertainDialogueOption -= ChosenGivenOption;
        GlobalEvents.CallbackForOnLookingForDialogueListWithGivenID -= GoBackToCertainDialogueOptionsCallbacked;
    }

    /// <summary>
    /// Rozpoczyna dialog z danym korzeniem dialogowym i nazwa NPC.
    /// </summary>
    /// <param name="dialogue_root">Korzen dialogu do odtworzenia.</param>
    /// <param name="npc_name">Nazwa NPC, z ktorym rozmawiamy.</param>
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

    /// <summary>
    /// Obsluguje powrot do okreslonych opcji dialogowych.
    /// </summary>
    /// <param name="sender">Obiekt wywolujacy zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawierajace liste opcji dialogowych.</param>
    private void GoBackToCertainDialogueOptionsCallbacked(object sender, GlobalEvents.CallbackForOnLookingForDialogueListWithGivenIDEventArgs e)
    {
        current_dialogue_list = e.list_of_options;
        dialogue_options_ui.DisplayOptions(current_dialogue_list);
    }

    /// <summary>
    /// Obsluguje wybor konkretnej opcji dialogowej przez gracza.
    /// </summary>
    /// <param name="sender">Obiekt wywolujacy zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawierajace ID wybranej opcji dialogowej.</param>
    private void ChosenGivenOption(object sender, GlobalEvents.OnChoosingCertainDialogueOptionEventArgs e)
    {
        DialogueNodeSO choosen_option = GetChoosenOptionBasedOnId(e.choosen_option_id);

        dialogue_options_ui.DisableAllOptions();

        StartCoroutine(DisplayDialogueOnScreen(choosen_option));
    }

    /// <summary>
    /// Wyswietla dialog na ekranie, odtwarzajac odpowiedni dzwiek i tekst.
    /// </summary>
    /// <param name="choosen_option">Wybrana opcja dialogowa.</param>
    private IEnumerator DisplayDialogueOnScreen(DialogueNodeSO choosen_option)
    {
        spoken_text_display_ui.DisplayText(choosen_option.main_character_text, MAIN_CHARACTER_NAME);

        AudioManager.Instance.PlayGivenClip(choosen_option.main_character_audio);

        yield return new WaitForSeconds(choosen_option.main_character_audio.length + choosen_option.eventual_npc_response_time_delay + DELAY_EPSILON_BETWEEN_EACH_DIALOGUE_PROC);

        foreach (DialogueNodeSO.NpcResponses response in choosen_option.responses)
        {
            spoken_text_display_ui.DisplayText(response.response, current_npc_name);
            AudioManager.Instance.PlayGivenClip(response.response_audio_clip);
            yield return new WaitForSeconds(response.response_audio_clip.length + response.eventual_response_time_delay + DELAY_EPSILON_BETWEEN_EACH_DIALOGUE_PROC);
        }

        StuffToBeDoneAfterNpcFinishes(choosen_option);
    }

    /// <summary>
    /// Obsluguje dzialania po zakonczeniu dialogu przez NPC.
    /// </summary>
    /// <param name="choosen_option">Opcja dialogowa wybrana przez gracza.</param>
    private void StuffToBeDoneAfterNpcFinishes(DialogueNodeSO choosen_option)
    {
        spoken_text_display_ui.DisableAllComponents();
        if (choosen_option.options.Count != 0)
        {
            current_dialogue_list = choosen_option.options;
            dialogue_options_ui.DisplayOptions(current_dialogue_list);
        }
        InvokeEventsOnChoosingCertainOptions(choosen_option);
    }

    /// <summary>
    /// Wywoluje odpowiednie zdarzenia na podstawie wybranej opcji dialogowej.
    /// </summary>
    /// <param name="option">Wybrana opcja dialogowa.</param>
    private void InvokeEventsOnChoosingCertainOptions(DialogueNodeSO option)
    {
        foreach (DialogueNodeSO.DialogueEvent each_event in option.invoked_events_list)
        {
            if (each_event == DialogueNodeSO.DialogueEvent.GoBackToCertainDialogueOption)
            {
                if (option.id_of_dialogue_option_to_go_back_to == current_dialogue_root.id)
                {
                    current_dialogue_list = current_dialogue_root.options;
                    dialogue_options_ui.DisplayOptions(current_dialogue_list);
                }

                GlobalEvents.OnLookingForDialogueListWithGivenIDEventArgs args = new(option.id_of_dialogue_option_to_go_back_to);
                GlobalEvents.FireOnLookingForDialogueListWithGivenID(this, args);
            }
            else if (each_event == DialogueNodeSO.DialogueEvent.EndDialogue)
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

    /// <summary>
    /// Zwraca opcje dialogowa na podstawie jej ID.
    /// </summary>
    /// <param name="id">ID opcji dialogowej.</param>
    /// <returns>Opcja dialogowa odpowiadajaca podanemu ID lub null, jesli nie znaleziono.</returns>
    private DialogueNodeSO GetChoosenOptionBasedOnId(string id)
    {
        foreach (DialogueNodeSO option in current_dialogue_list)
        {
            if (option.id == id)
            {
                return option;
            }
        }

        Debug.Log("Dupa, nie powinno Cie tu byc :/");
        return null;
    }
}
