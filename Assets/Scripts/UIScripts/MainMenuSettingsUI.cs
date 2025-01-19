using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk, który umo¿liwia powrót do g³ównego menu.
    /// </summary>
    [SerializeField] private Button back_to_pause_menu_button;

    [Header("Scripts")]
    /// <summary>
    /// Referencja do obiektu UI g³ównego menu.
    /// </summary>
    [SerializeField] private MainMenuUI main_menu_UI;

    [Header("Resolution dropdown")]
    /// <summary>
    /// Dropdown do wyboru rozdzielczoœci ekranu.
    /// </summary>
    [SerializeField] private TMP_Dropdown resolution_dropdown;

    /// <summary>
    /// Wyœwietlacz aktualnie wybranej rozdzielczoœci.
    /// </summary>
    [SerializeField] private TextMeshProUGUI selected_resolution_option_text;

    [Header("Sliders")]
    /// <summary>
    /// Suwak, który pozwala na ustawienie g³oœnoœci muzyki.
    /// </summary>
    [SerializeField] private Slider music_volume_slider;

    /// <summary>
    /// Suwak, który pozwala na ustawienie g³oœnoœci efektów dŸwiêkowych.
    /// </summary>
    [SerializeField] private Slider audio_effects_volume_slider;

    /// <summary>
    /// Suwak, który pozwala na ustawienie czu³oœci myszy.
    /// </summary>
    [SerializeField] private Slider sensitivity_slider;

    [Header("Toggles")]
    /// <summary>
    /// Prze³¹cznik, który pozwala na w³¹czenie lub wy³¹czenie trybu pe³noekranowego.
    /// </summary>
    [SerializeField] private Toggle fullscreen_toggle;


    /// <summary>
    /// Inicjalizuje nas³uchiwanie na zmiany ustawieñ w interfejsie.
    /// Ustawia pocz¹tkowe wartoœci suwaków, dropdownów oraz prze³¹czników.
    /// </summary>
    void Start()
    {
        // Nas³uchiwanie klikniêcia przycisku powrotu do g³ównego menu
        back_to_pause_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);

        // Nas³uchiwanie zmiany wartoœci w dropdownie rozdzielczoœci
        resolution_dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        resolution_dropdown.value = Settings.GetResolutionIndex();  // Ustawienie pocz¹tkowej wartoœci rozdzielczoœci

        // Nas³uchiwanie zmiany wartoœci suwaków
        music_volume_slider.onValueChanged.AddListener(OnMusicVolumeChanged);
        audio_effects_volume_slider.onValueChanged.AddListener(OnAudioEffectsVolumeChanged);

        // Ustawienie stanu prze³¹cznika fullscreen na podstawie aktualnego ustawienia
        fullscreen_toggle.isOn = Screen.fullScreen;
        fullscreen_toggle.onValueChanged.AddListener(SetFullscreen);

        // Ustawienie wartoœci suwaka czu³oœci na podstawie ustawienia
        sensitivity_slider.minValue = 5f;
        sensitivity_slider.maxValue = 30f;
        sensitivity_slider.value = Settings.GetSensitivity();
        sensitivity_slider.onValueChanged.AddListener(SetSensitivity);
    }

    /// <summary>
    /// Ustawia czu³oœæ myszy na wartoœæ podan¹ przez suwak.
    /// </summary>
    /// <param name="sens">Wartoœæ czu³oœci myszy.</param>
    private void SetSensitivity(float sens)
    {
        Settings.SetSensitivity(sens);
    }

    /// <summary>
    /// Ustawia tryb pe³noekranowy (fullscreen) na podstawie stanu prze³¹cznika.
    /// </summary>
    /// <param name="is_fullscreen">Czy tryb pe³noekranowy ma byæ w³¹czony?</param>
    private void SetFullscreen(bool is_fullscreen)
    {
        Screen.fullScreen = is_fullscreen;
    }

    /// <summary>
    /// Zmienia g³oœnoœæ muzyki na podstawie wartoœci suwaka.
    /// </summary>
    /// <param name="value">Nowa wartoœæ g³oœnoœci muzyki.</param>
    private void OnMusicVolumeChanged(float value)
    {
        Settings.SetMusicVolume(value);
    }

    /// <summary>
    /// Zmienia g³oœnoœæ efektów dŸwiêkowych na podstawie wartoœci suwaka.
    /// </summary>
    /// <param name="value">Nowa wartoœæ g³oœnoœci efektów dŸwiêkowych.</param>
    private void OnAudioEffectsVolumeChanged(float value)
    {
        Settings.SetAudioEffectsVolume(value);
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku powrotu do g³ównego menu, aktywuj¹c odpowiedni UI.
    /// </summary>
    private void OnBackToMainMenuButtonClick()
    {
        main_menu_UI.gameObject.SetActive(true);  // Aktywuje g³ówne menu
        this.gameObject.SetActive(false);  // Dezaktywuje bie¿¹cy ekran ustawieñ
    }

    /// <summary>
    /// Obs³uguje zmianê wybranej rozdzielczoœci z dropdowna.
    /// Ustawia now¹ rozdzielczoœæ ekranu i aktualizuje wyœwietlany tekst.
    /// </summary>
    /// <param name="index">Indeks wybranej rozdzielczoœci w dropdownie.</param>
    private void OnDropdownValueChanged(int index)
    {
        Settings.SetResolutionIndex(index);  // Ustawienie indeksu rozdzielczoœci
        selected_resolution_option_text.text = resolution_dropdown.options[index].text;  // Wyœwietlanie wybranej opcji
        Settings.SetResolutionParameters(selected_resolution_option_text.text);  // Ustawienie parametrów rozdzielczoœci
        ChangeResolution();  // Zmiana rozdzielczoœci ekranu
    }

    /// <summary>
    /// Zmienia rozdzielczoœæ ekranu na podstawie ustawieñ.
    /// </summary>
    private void ChangeResolution()
    {
        Screen.SetResolution(Settings.GetResolutionWidth(), Settings.GetResolutionHeight(), fullscreen_toggle.isOn);  // Ustawia now¹ rozdzielczoœæ
    }
}
