using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk, ktory umozliwia powrot do glownego menu.
    /// </summary>
    [SerializeField] private Button back_to_pause_menu_button;

    [Header("Scripts")]
    /// <summary>
    /// Referencja do obiektu UI glownego menu.
    /// </summary>
    [SerializeField] private MainMenuUI main_menu_UI;

    [Header("Resolution dropdown")]
    /// <summary>
    /// Dropdown do wyboru rozdzielczosci ekranu.
    /// </summary>
    [SerializeField] private TMP_Dropdown resolution_dropdown;

    /// <summary>
    /// Wyswietlacz aktualnie wybranej rozdzielczosci.
    /// </summary>
    [SerializeField] private TextMeshProUGUI selected_resolution_option_text;

    [Header("Sliders")]
    /// <summary>
    /// Suwak, ktory pozwala na ustawienie glosnosci muzyki.
    /// </summary>
    [SerializeField] private Slider music_volume_slider;

    /// <summary>
    /// Suwak, ktory pozwala na ustawienie glosnosci efektow dzwiekowych.
    /// </summary>
    [SerializeField] private Slider audio_effects_volume_slider;

    /// <summary>
    /// Suwak, ktory pozwala na ustawienie czulosci myszy.
    /// </summary>
    [SerializeField] private Slider sensitivity_slider;

    [Header("Toggles")]
    /// <summary>
    /// Przelacznik, ktory pozwala na wlaczenie lub wylaczenie trybu pelnoekranowego.
    /// </summary>
    [SerializeField] private Toggle fullscreen_toggle;


    /// <summary>
    /// Inicjalizuje nasluchiwanie na zmiany ustawien w interfejsie.
    /// Ustawia poczatkowe wartosci suwakow, dropdownow oraz przelacznikow.
    /// </summary>
    void Start()
    {
        // Nasluchiwanie klikniecia przycisku powrotu do glownego menu
        back_to_pause_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);

        // Nasluchiwanie zmiany wartosci w dropdownie rozdzielczosci
        resolution_dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        resolution_dropdown.value = Settings.GetResolutionIndex();  // Ustawienie poczatkowej wartosci rozdzielczosci

        // Nasluchiwanie zmiany wartosci suwakow
        music_volume_slider.onValueChanged.AddListener(OnMusicVolumeChanged);
        audio_effects_volume_slider.onValueChanged.AddListener(OnAudioEffectsVolumeChanged);

        // Ustawienie stanu przelacznika fullscreen na podstawie aktualnego ustawienia
        fullscreen_toggle.isOn = Screen.fullScreen;
        fullscreen_toggle.onValueChanged.AddListener(SetFullscreen);

        // Ustawienie wartosci suwaka czulosci na podstawie ustawienia
        sensitivity_slider.minValue = 5f;
        sensitivity_slider.maxValue = 30f;
        sensitivity_slider.value = Settings.GetSensitivity();
        sensitivity_slider.onValueChanged.AddListener(SetSensitivity);
    }

    /// <summary>
    /// Ustawia czulosc myszy na wartosc podana przez suwak.
    /// </summary>
    /// <param name="sens">Wartosc czulosci myszy.</param>
    private void SetSensitivity(float sens)
    {
        Settings.SetSensitivity(sens);
    }

    /// <summary>
    /// Ustawia tryb pelnoekranowy (fullscreen) na podstawie stanu przelacznika.
    /// </summary>
    /// <param name="is_fullscreen">Czy tryb pelnoekranowy ma byc wlaczony?</param>
    private void SetFullscreen(bool is_fullscreen)
    {
        Screen.fullScreen = is_fullscreen;
    }

    /// <summary>
    /// Zmienia glosnosc muzyki na podstawie wartosci suwaka.
    /// </summary>
    /// <param name="value">Nowa wartosc glosnosci muzyki.</param>
    private void OnMusicVolumeChanged(float value)
    {
        Settings.SetMusicVolume(value);
    }

    /// <summary>
    /// Zmienia glosnosc efektow dzwiekowych na podstawie wartosci suwaka.
    /// </summary>
    /// <param name="value">Nowa wartosc glosnosci efektow dzwiekowych.</param>
    private void OnAudioEffectsVolumeChanged(float value)
    {
        Settings.SetAudioEffectsVolume(value);
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku powrotu do glownego menu, aktywujac odpowiedni UI.
    /// </summary>
    private void OnBackToMainMenuButtonClick()
    {
        main_menu_UI.gameObject.SetActive(true);  // Aktywuje glowne menu
        this.gameObject.SetActive(false);  // Dezaktywuje biezacy ekran ustawien
    }

    /// <summary>
    /// Obsluguje zmianê wybranej rozdzielczosci z dropdowna.
    /// Ustawia nowa rozdzielczosc ekranu i aktualizuje wyswietlany tekst.
    /// </summary>
    /// <param name="index">Indeks wybranej rozdzielczosci w dropdownie.</param>
    private void OnDropdownValueChanged(int index)
    {
        Settings.SetResolutionIndex(index);  // Ustawienie indeksu rozdzielczosci
        selected_resolution_option_text.text = resolution_dropdown.options[index].text;  // Wyswietlanie wybranej opcji
        Settings.SetResolutionParameters(selected_resolution_option_text.text);  // Ustawienie parametrow rozdzielczosci
        ChangeResolution();  // Zmiana rozdzielczosci ekranu
    }

    /// <summary>
    /// Zmienia rozdzielczosc ekranu na podstawie ustawien.
    /// </summary>
    private void ChangeResolution()
    {
        Screen.SetResolution(Settings.GetResolutionWidth(), Settings.GetResolutionHeight(), fullscreen_toggle.isOn);  // Ustawia nowa rozdzielczosc
    }
}
