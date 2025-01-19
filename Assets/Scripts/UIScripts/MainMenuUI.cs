using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk, który rozpoczyna now¹ grê.
    /// </summary>
    [SerializeField] private Button start_game_button;

    /// <summary>
    /// Przycisk, który pozwala na za³adowanie zapisanej gry.
    /// </summary>
    [SerializeField] private Button load_game_button;

    /// <summary>
    /// Przycisk, który otwiera ekran ustawieñ gry.
    /// </summary>
    [SerializeField] private Button settings_button;

    /// <summary>
    /// Przycisk, który umo¿liwia zamkniêcie gry.
    /// </summary>
    [SerializeField] private Button exit_game_button;

    [Header("Scripts")]
    /// <summary>
    /// UI ustawieñ, które pojawi siê po klikniêciu przycisku ustawieñ.
    /// </summary>
    [SerializeField] private MainMenuSettingsUI settings_UI;


    /// <summary>
    /// Inicjalizuje nas³uchiwanie na klikniêcia przycisków g³ównego menu.
    /// </summary>
    private void Start()
    {
        // Nas³uchiwanie klikniêæ przycisków i przypisanie odpowiednich metod
        start_game_button.onClick.AddListener(OnStartGameButtonClick);
        load_game_button.onClick.AddListener(OnLoadGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        exit_game_button.onClick.AddListener(OnExitGameButtonClick);
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Start Game", ³adowanie sceny gry i resetowanie stanu kontynuacji.
    /// </summary>
    private void OnStartGameButtonClick()
    {
        SceneController.LoadScene(SceneController.GAME_SCENE);  // £adowanie sceny gry
        PlayerPrefs.SetInt("ContinueSave", 0);  // Ustawienie wartoœci "ContinueSave" na 0, co oznacza rozpoczêcie nowej gry
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Load Game", ³adowanie sceny gry i ustawianie kontynuacji.
    /// </summary>
    private void OnLoadGameButtonClick()
    {
        SceneController.LoadScene(SceneController.GAME_SCENE);  // £adowanie sceny gry
        PlayerPrefs.SetInt("ContinueSave", 1);  // Ustawienie wartoœci "ContinueSave" na 1, co oznacza ³adowanie zapisanej gry
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Settings", wyœwietlanie interfejsu ustawieñ.
    /// </summary>
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);  // Aktywowanie interfejsu ustawieñ
        this.gameObject.SetActive(false);  // Dezaktywowanie bie¿¹cego interfejsu g³ównego menu
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Exit", zamkniêcie aplikacji.
    /// </summary>
    private void OnExitGameButtonClick()
    {
        Application.Quit();  // Zamyka aplikacjê
    }
}
