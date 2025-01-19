using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk, kt�ry umo�liwia powr�t do g��wnego menu.
    /// </summary>
    [SerializeField] private Button back_to_pause_menu_button;

    [Header("Scripts")]
    /// <summary>
    /// Referencja do obiektu UI g��wnego menu.
    /// </summary>
    [SerializeField] private MainMenuUI main_menu_UI;

    [Header("Resolution dropdown")]
    /// <summary>
    /// Dropdown do wyboru rozdzielczo�ci ekranu.
    /// </summary>
    [SerializeField] private TMP_Dropdown resolution_dropdown;

    /// <summary>
    /// Wy�wietlacz aktualnie wybranej rozdzielczo�ci.
    /// </summary>
    [SerializeField] private TextMeshProUGUI selected_resolution_option_text;

    [Header("Sliders")]
    /// <summary>
    /// Suwak, kt�ry pozwala na ustawienie g�o�no�ci muzyki.
    /// </summary>
    [SerializeField] private Slider music_volume_slider;

    /// <summary>
    /// Suwak, kt�ry pozwala na ustawienie g�o�no�ci efekt�w d�wi�kowych.
    /// </summary>
    [SerializeField] private Slider audio_effects_volume_slider;

    /// <summary>
    /// Suwak, kt�ry pozwala na ustawienie czu�o�ci myszy.
    /// </summary>
    [SerializeField] private Slider sensitivity_slider;

    [Header("Toggles")]
    /// <summary>
    /// Prze��cznik, kt�ry pozwala na w��czenie lub wy��czenie trybu pe�noekranowego.
    /// </summary>
    [SerializeField] private Toggle fullscreen_toggle;


    /// <summary>
    /// Inicjalizuje nas�uchiwanie na zmiany ustawie� w interfejsie.
    /// Ustawia pocz�tkowe warto�ci suwak�w, dropdown�w oraz prze��cznik�w.
    /// </summary>
    void Start()
    {
        // Nas�uchiwanie klikni�cia przycisku powrotu do g��wnego menu
        back_to_pause_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);

        // Nas�uchiwanie zmiany warto�ci w dropdownie rozdzielczo�ci
        resolution_dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        resolution_dropdown.value = Settings.GetResolutionIndex();  // Ustawienie pocz�tkowej warto�ci rozdzielczo�ci

        // Nas�uchiwanie zmiany warto�ci suwak�w
        music_volume_slider.onValueChanged.AddListener(OnMusicVolumeChanged);
        audio_effects_volume_slider.onValueChanged.AddListener(OnAudioEffectsVolumeChanged);

        // Ustawienie stanu prze��cznika fullscreen na podstawie aktualnego ustawienia
        fullscreen_toggle.isOn = Screen.fullScreen;
        fullscreen_toggle.onValueChanged.AddListener(SetFullscreen);

        // Ustawienie warto�ci suwaka czu�o�ci na podstawie ustawienia
        sensitivity_slider.minValue = 5f;
        sensitivity_slider.maxValue = 30f;
        sensitivity_slider.value = Settings.GetSensitivity();
        sensitivity_slider.onValueChanged.AddListener(SetSensitivity);
    }

    /// <summary>
    /// Ustawia czu�o�� myszy na warto�� podan� przez suwak.
    /// </summary>
    /// <param name="sens">Warto�� czu�o�ci myszy.</param>
    private void SetSensitivity(float sens)
    {
        Settings.SetSensitivity(sens);
    }

    /// <summary>
    /// Ustawia tryb pe�noekranowy (fullscreen) na podstawie stanu prze��cznika.
    /// </summary>
    /// <param name="is_fullscreen">Czy tryb pe�noekranowy ma by� w��czony?</param>
    private void SetFullscreen(bool is_fullscreen)
    {
        Screen.fullScreen = is_fullscreen;
    }

    /// <summary>
    /// Zmienia g�o�no�� muzyki na podstawie warto�ci suwaka.
    /// </summary>
    /// <param name="value">Nowa warto�� g�o�no�ci muzyki.</param>
    private void OnMusicVolumeChanged(float value)
    {
        Settings.SetMusicVolume(value);
    }

    /// <summary>
    /// Zmienia g�o�no�� efekt�w d�wi�kowych na podstawie warto�ci suwaka.
    /// </summary>
    /// <param name="value">Nowa warto�� g�o�no�ci efekt�w d�wi�kowych.</param>
    private void OnAudioEffectsVolumeChanged(float value)
    {
        Settings.SetAudioEffectsVolume(value);
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku powrotu do g��wnego menu, aktywuj�c odpowiedni UI.
    /// </summary>
    private void OnBackToMainMenuButtonClick()
    {
        main_menu_UI.gameObject.SetActive(true);  // Aktywuje g��wne menu
        this.gameObject.SetActive(false);  // Dezaktywuje bie��cy ekran ustawie�
    }

    /// <summary>
    /// Obs�uguje zmian� wybranej rozdzielczo�ci z dropdowna.
    /// Ustawia now� rozdzielczo�� ekranu i aktualizuje wy�wietlany tekst.
    /// </summary>
    /// <param name="index">Indeks wybranej rozdzielczo�ci w dropdownie.</param>
    private void OnDropdownValueChanged(int index)
    {
        Settings.SetResolutionIndex(index);  // Ustawienie indeksu rozdzielczo�ci
        selected_resolution_option_text.text = resolution_dropdown.options[index].text;  // Wy�wietlanie wybranej opcji
        Settings.SetResolutionParameters(selected_resolution_option_text.text);  // Ustawienie parametr�w rozdzielczo�ci
        ChangeResolution();  // Zmiana rozdzielczo�ci ekranu
    }

    /// <summary>
    /// Zmienia rozdzielczo�� ekranu na podstawie ustawie�.
    /// </summary>
    private void ChangeResolution()
    {
        Screen.SetResolution(Settings.GetResolutionWidth(), Settings.GetResolutionHeight(), fullscreen_toggle.isOn);  // Ustawia now� rozdzielczo��
    }
}
