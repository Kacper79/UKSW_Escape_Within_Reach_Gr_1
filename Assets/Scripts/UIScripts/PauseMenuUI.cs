using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button back_to_game_button;
    [SerializeField] private Button save_game_button;
    [SerializeField] private Button settings_button;
    [SerializeField] private Button back_to_main_menu_button;

    [SerializeField] private SettingsUI settings_UI;


    private void Start()
    {
        back_to_game_button.onClick.AddListener(OnBackToGameButtonClick);
        save_game_button.onClick.AddListener(OnSaveGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        back_to_main_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);
    }

    private void OnBackToGameButtonClick()
    {
        GlobalEvents.FireOnResumeGame(this);
        GlobalEvents.FireOnAnyUIClose(this);
        this.gameObject.SetActive(false);
    }
    private void OnSaveGameButtonClick()
    {
        // TODO: Save System 
    }
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void OnBackToMainMenuButtonClick()
    {
        SceneController.LoadScene(SceneController.MAIN_MENU_SCENE);
    }
}
