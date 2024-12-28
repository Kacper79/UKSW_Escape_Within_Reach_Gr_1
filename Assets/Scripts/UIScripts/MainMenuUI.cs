using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button start_game_button;
    [SerializeField] private Button load_game_button;
    [SerializeField] private Button settings_button;
    [SerializeField] private Button exit_game_button;

    [Header("Scripts")]
    [SerializeField] private MainMenuSettingsUI settings_UI;

    private void Start()
    {
        start_game_button.onClick.AddListener(OnStartGameButtonClick);
        load_game_button.onClick.AddListener(OnLoadGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        exit_game_button.onClick.AddListener(OnExitGameButtonClick);

    }

    private void OnStartGameButtonClick()
    {
        SceneController.LoadScene(SceneController.GAME_SCENE);
    }

    private void OnLoadGameButtonClick()
    {
        //TODO
    }
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void OnExitGameButtonClick()
    {
        Application.Quit();
    }

}
