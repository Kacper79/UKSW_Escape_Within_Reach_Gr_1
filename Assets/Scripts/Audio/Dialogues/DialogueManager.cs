using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Zarz�dza systemem dialog�w w grze.
/// Obs�uguje wy�wietlanie opcji dialogowych, przetwarzanie wybor�w gracza oraz odtwarzanie d�wi�k�w i tekst�w.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Op�nienie pomi�dzy ka�d� procedur� dialogow�.
    /// </summary>
    private const float DELAY_EPSILON_BETWEEN_EACH_DIALOGUE_PROC = 0.02f;

    /// <summary>
    /// Nazwa g��wnego bohatera, wy�wietlana w dialogach.
    /// </summary>
    private const string MAIN_CHARACTER_NAME = "MAIN CHARACTER NAME";

    /// <summary>
    /// Statyczna instancja klasy DialogueManager (singleton).
    /// </summary>
    public static DialogueManager Instance { get; private set; }

    /// <summary>
    /// Korze� aktualnie odtwarzanego dialogu.
    /// </summary>
    private DialogueNodeSO current_dialogue_root;

    /// <summary>
    /// Lista aktualnych opcji dialogowych.
    /// </summary>
    private List<DialogueNodeSO> current_dialogue_list = new();

    /// <summary>
    /// Interfejs wy�wietlania opcji dialogowych.
    /// </summary>
    private DialogueOptionsUI dialogue_options_ui;

    /// <summary>
    /// Interfejs wy�wietlania tekstu dialogowego.
    /// </summary>
    private SpokenTextDisplayUI spoken_text_display_ui;

    /// <summary>
    /// Nazwa NPC, z kt�rym aktualnie rozmawiamy.
    /// </summary>
    private string current_npc_name;

    /// <summary>
    /// Inicjalizuje singleton oraz wyszukuje elementy UI zwi�zane z dialogiem.
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
    /// Subskrybuje globalne zdarzenia wymagane do obs�ugi dialog�w.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnChoosingCertainDialogueOption += ChosenGivenOption;
        GlobalEvents.CallbackForOnLookingForDialogueListWithGivenID += GoBackToCertainDialogueOptionsCallbacked;
    }

    /// <summary>
    /// Wyrejestrowuje globalne zdarzenia wymagane do obs�ugi dialog�w.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnChoosingCertainDialogueOption -= ChosenGivenOption;
        GlobalEvents.CallbackForOnLookingForDialogueListWithGivenID -= GoBackToCertainDialogueOptionsCallbacked;
    }

    /// <summary>
    /// Rozpoczyna dialog z danym korzeniem dialogowym i nazw� NPC.
    /// </summary>
    /// <param name="dialogue_root">Korze� dialogu do odtworzenia.</param>
    /// <param name="npc_name">Nazwa NPC, z kt�rym rozmawiamy.</param>
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
    /// Obs�uguje powr�t do okre�lonych opcji dialogowych.
    /// </summary>
    /// <param name="sender">Obiekt wywo�uj�cy zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawieraj�ce list� opcji dialogowych.</param>
    private void GoBackToCertainDialogueOptionsCallbacked(object sender, GlobalEvents.CallbackForOnLookingForDialogueListWithGivenIDEventArgs e)
    {
        current_dialogue_list = e.list_of_options;
        dialogue_options_ui.DisplayOptions(current_dialogue_list);
    }

    /// <summary>
    /// Obs�uguje wyb�r konkretnej opcji dialogowej przez gracza.
    /// </summary>
    /// <param name="sender">Obiekt wywo�uj�cy zdarzenie.</param>
    /// <param name="e">Argumenty zdarzenia zawieraj�ce ID wybranej opcji dialogowej.</param>
    private void ChosenGivenOption(object sender, GlobalEvents.OnChoosingCertainDialogueOptionEventArgs e)
    {
        DialogueNodeSO choosen_option = GetChoosenOptionBasedOnId(e.choosen_option_id);

        dialogue_options_ui.DisableAllOptions();

        StartCoroutine(DisplayDialogueOnScreen(choosen_option));
    }

    /// <summary>
    /// Wy�wietla dialog na ekranie, odtwarzaj�c odpowiedni d�wi�k i tekst.
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
    /// Obs�uguje dzia�ania po zako�czeniu dialogu przez NPC.
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
    /// Wywo�uje odpowiednie zdarzenia na podstawie wybranej opcji dialogowej.
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
            else
            {
                GlobalEvents.FireCertainDialogueEvent(this, each_event);
            }
        }
    }

    /// <summary>
    /// Zwraca opcj� dialogow� na podstawie jej ID.
    /// </summary>
    /// <param name="id">ID opcji dialogowej.</param>
    /// <returns>Opcja dialogowa odpowiadaj�ca podanemu ID lub null, je�li nie znaleziono.</returns>
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
