using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button back_to_pause_menu_button;  // Przycisk powrotu do menu pauzy

    [Header("Rebinding Buttons")]
    [SerializeField] private Button rebind_inventory;  // Przycisk do ponownego przypisania przycisku dla otwierania ekwipunku
    [SerializeField] private Button rebind_questlog;  // Przycisk do ponownego przypisania przycisku dla otwierania dziennika misji
    [SerializeField] private Button rebind_achievements;  // Przycisk do ponownego przypisania przycisku dla otwierania osiagniec
    [SerializeField] private Button rebind_interaction;  // Przycisk do ponownego przypisania przycisku interakcji
    [SerializeField] private Button rebind_smoking;  // Przycisk do ponownego przypisania przycisku palenia
    [SerializeField] private Button rebind_coin_throw;  // Przycisk do ponownego przypisania przycisku rzutu moneta
    [SerializeField] private Button rebind_default;  // Przycisk do resetowania wszystkich przypisanych przyciskow

    [Header("Scripts")]
    [SerializeField] private PauseMenuUI pause_menu_UI;  // Odwolanie do menu pauzy

    [Header("Resolution dropdown")]
    [SerializeField] private TMP_Dropdown resolution_dropdown;  // Rozwijane menu do wyboru rozdzielczosci
    [SerializeField] private TextMeshProUGUI selected_resolution_option_text;  // Tekst wybranego elementu rozdzielczosci

    [Header("Sliders")]
    [SerializeField] private Slider music_volume_slider;  // Suwak do regulacji glosnosci muzyki
    [SerializeField] private Slider audio_effects_volume_slider;  // Suwak do regulacji glosnosci efektow dzwiekowych
    [SerializeField] private Slider sensitivity_slider;  // Suwak do regulacji czulosci

    [Header("Toggles")]
    [SerializeField] private Toggle fullscreen_toggle;  // Przelacznik do zmiany trybu pelnoekranowego

    private bool is_rebinding = false;  // Flaga do sprawdzania, czy trwa proces ponownego przypisania

    private const string INVENTORY_ACTION = "OpenInventory";  // Akcja przypisana do otwierania ekwipunku
    private const string QUESTLOG_ACTION = "OpenQuestLog";  // Akcja przypisana do otwierania dziennika misji
    private const string ACHIEVEMENTS_ACTION = "OpenAchievements";  // Akcja przypisana do otwierania osiagniec
    private const string INTERACTION_ACTION = "Interact";  // Akcja przypisana do interakcji
    private const string SMOKING_ACTION = "UseCigaretts";  // Akcja przypisana do palenia
    private const string COITHROW_ACTION = "ThrowCoin";  // Akcja przypisana do rzutu moneta

    /// <summary>
    /// Ustawia nasluchiwanie na wszystkie przyciski, suwaki i przelaczniki.
    /// </summary>
    void Start()
    {
        back_to_pause_menu_button.onClick.AddListener(OnBackToPauseMenuButtonClick);  // Przyciski ustawiajace akcje po klikni�ciu

        rebind_inventory.onClick.AddListener(() => StartRebinding(INVENTORY_ACTION, rebind_inventory));
        rebind_questlog.onClick.AddListener(() => StartRebinding(QUESTLOG_ACTION, rebind_questlog));
        rebind_achievements.onClick.AddListener(() => StartRebinding(ACHIEVEMENTS_ACTION, rebind_achievements));
        rebind_interaction.onClick.AddListener(() => StartRebinding(INTERACTION_ACTION, rebind_interaction));
        rebind_smoking.onClick.AddListener(() => StartRebinding(SMOKING_ACTION, rebind_smoking));
        rebind_coin_throw.onClick.AddListener(() => StartRebinding(COITHROW_ACTION, rebind_coin_throw));

        rebind_default.onClick.AddListener(ResetBindings);  // Resetowanie przypisan

        resolution_dropdown.onValueChanged.AddListener(OnDropdownValueChanged);  // Nasluchiwanie na zmiany w rozwijanym menu
        resolution_dropdown.value = Settings.GetResolutionIndex();  // Ustawienie aktualnego indeksu rozdzielczosci

        music_volume_slider.onValueChanged.AddListener(OnMusicVolumeChanged);  // Nasluchiwanie na zmiany glosnosci muzyki
        audio_effects_volume_slider.onValueChanged.AddListener(OnAudioEffectsVolumeChanged);  // Nasluchiwanie na zmiany glosnosci efektow

        fullscreen_toggle.isOn = Screen.fullScreen;  // Ustawienie statusu pelnoekranowego
        fullscreen_toggle.onValueChanged.AddListener(SetFullscreen);  // Nasluchiwanie na zmiany statusu pelnoekranowego

        sensitivity_slider.minValue = 5f;  // Minimalna wartosc czulosci
        sensitivity_slider.maxValue = 30f;  // Maksymalna wartosc czulosci
        sensitivity_slider.value = Settings.GetSensitivity();  // Ustawienie aktualnej wartosci czulosci
        sensitivity_slider.onValueChanged.AddListener(SetSensitivity);  // Nasluchiwanie na zmiany czulosci

        SetBindingsKeysText();  // Inicjalizowanie tekstow przypisanych akcji
    }

    /// <summary>
    /// Resetuje wszystkie przypisane klawisze do domy�lnych.
    /// </summary>
    public void ResetBindings()
    {
        RebindManager.Instance.ResetBindingsOverride();  // Resetuje przypisania
        SetBindingsKeysText();  // Ustawia domyslne teksty dla przyciskow
    }

    /// <summary>
    /// Ustawia tekst przyciskow zgodnie z aktualnie przypisanymi klawiszami.
    /// </summary>
    private void SetBindingsKeysText()
    {
        SetButtonTextForAction(INVENTORY_ACTION, rebind_inventory);  // Ustawia tekst dla przycisku ekwipunku
        SetButtonTextForAction(QUESTLOG_ACTION, rebind_questlog);  // Ustawia tekst dla przycisku dziennika misji
        SetButtonTextForAction(ACHIEVEMENTS_ACTION, rebind_achievements);  // Ustawia tekst dla przycisku osiagniec
        SetButtonTextForAction(INTERACTION_ACTION, rebind_interaction);  // Ustawia tekst dla przycisku interakcji
        SetButtonTextForAction(SMOKING_ACTION, rebind_smoking);  // Ustawia tekst dla przycisku palenia
        SetButtonTextForAction(COITHROW_ACTION, rebind_coin_throw);  // Ustawia tekst dla przycisku rzutu moneta
    }

    /// <summary>
    /// Ustawia tekst przycisku na aktualnie przypisany klawisz do danej akcji.
    /// </summary>
    private void SetButtonTextForAction(string actionName, Button button)
    {
        var action = RebindManager.Instance.player_input.FindAction(actionName);  // Pobiera akcje
        if (action != null && action.bindings.Count > 0)
        {
            string bindingDisplayString = action.GetBindingDisplayString();  // Pobiera tekst przypisanego klawisza
            button.GetComponentInChildren<TMP_Text>().text = bindingDisplayString;  // Ustawia tekst na przycisku
        }
    }

    /// <summary>
    /// Rozpoczyna proces ponownego przypisania klawisza dla danej akcji.
    /// </summary>
    private void StartRebinding(string action_name, Button button)
    {
        button.GetComponentInChildren<TMP_Text>().text = "...";  // Ustawia tekst, gdy trwa ponowne przypisywanie
        RebindManager.Instance.ListenAndRebindControl(action_name, () => SetBindingsKeysText());  // Nasluchuje ponowne przypisanie
    }

    /// <summary>
    /// Ustawia czulosc w ustawieniach.
    /// </summary>
    private void SetSensitivity(float sens)
    {
        Settings.SetSensitivity(sens);
    }

    /// <summary>
    /// Ustawia tryb pelnoekranowy.
    /// </summary>
    private void SetFullscreen(bool is_fullscreen)
    {
        Screen.fullScreen = is_fullscreen;
    }

    /// <summary>
    /// Ustawia glosnosc muzyki w ustawieniach.
    /// </summary>
    private void OnMusicVolumeChanged(float value)
    {
        Settings.SetMusicVolume(value);
    }

    /// <summary>
    /// Ustawia glosnosc efektow dzwiekowych w ustawieniach.
    /// </summary>
    private void OnAudioEffectsVolumeChanged(float value)
    {
        Settings.SetAudioEffectsVolume(value);
    }

    /// <summary>
    /// Przechodzi z powrotem do menu pauzy.
    /// </summary>
    private void OnBackToPauseMenuButtonClick()
    {
        pause_menu_UI.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Zmienia wybrana rozdzielczosc na podstawie zmiany w rozwijanym menu.
    /// </summary>
    private void OnDropdownValueChanged(int index)
    {
        Settings.SetResolutionIndex(index);  // Ustawia wybrany indeks rozdzielczosci
        selected_resolution_option_text.text = resolution_dropdown.options[index].text;  // Wyswietla wybrana rozdzielczosc
        Settings.SetResolutionParameters(selected_resolution_option_text.text);  // Ustawia parametry rozdzielczosci
        ChangeResolution();  // Zmienia rozdzielczosc ekranu
    }

    /// <summary>
    /// Zmienia rozdzielczosc ekranu na podstawie wybranych ustawien.
    /// </summary>
    private void ChangeResolution()
    {
        Screen.SetResolution(Settings.GetResolutionWidth(), Settings.GetResolutionHeight(), fullscreen_toggle.isOn);  // Zmienia rozdzielczosc
    }
}
