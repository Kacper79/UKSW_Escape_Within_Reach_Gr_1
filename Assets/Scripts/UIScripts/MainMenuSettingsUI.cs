using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button back_to_pause_menu_button;

    [Header("Scripts")]
    [SerializeField] private MainMenuUI main_menu_UI;

    [Header("Resolution dropdown")]
    [SerializeField] private TMP_Dropdown resolution_dropdown;
    [SerializeField] private TextMeshProUGUI selected_resolution_option_text;

    [Header("Sliders")]
    [SerializeField] private Slider music_volume_slider;
    [SerializeField] private Slider audio_effects_volume_slider;
    [SerializeField] private Slider sensitivity_slider;

    [Header("Toggles")]
    [SerializeField] private Toggle fullscreen_toggle;


    void Start()
    {
        back_to_pause_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);

        resolution_dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        resolution_dropdown.value = Settings.GetResolutionIndex();

        music_volume_slider.onValueChanged.AddListener(OnMusicVolumeChanged);
        audio_effects_volume_slider.onValueChanged.AddListener(OnAudioEffectsVolumeChanged);

        fullscreen_toggle.isOn = Screen.fullScreen;
        fullscreen_toggle.onValueChanged.AddListener(SetFullscreen);

        sensitivity_slider.minValue = 5f;
        sensitivity_slider.maxValue = 30f;
        sensitivity_slider.value = Settings.GetSensitivity();
        sensitivity_slider.onValueChanged.AddListener(SetSensitivity);

    }

    private void SetSensitivity(float sens)
    {
        Settings.SetSensitivity(sens);
    }
    private void SetFullscreen(bool is_fullscreen)
    {
        Screen.fullScreen = is_fullscreen;
    }
    private void OnMusicVolumeChanged(float value)
    {
        Settings.SetMusicVolume(value);
    }

    private void OnAudioEffectsVolumeChanged(float value)
    {
        Settings.SetAudioEffectsVolume(value);
    }

    private void OnBackToMainMenuButtonClick()
    {
        main_menu_UI.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnDropdownValueChanged(int index)
    {
        Settings.SetResolutionIndex(index);
        selected_resolution_option_text.text = resolution_dropdown.options[index].text;
        Settings.SetResolutionParameters(selected_resolution_option_text.text);
        ChangeResolution();
    }

    private void ChangeResolution()
    {
        Screen.SetResolution(Settings.GetResolutionWidth(), Settings.GetResolutionHeight(), fullscreen_toggle.isOn);
    }
}
