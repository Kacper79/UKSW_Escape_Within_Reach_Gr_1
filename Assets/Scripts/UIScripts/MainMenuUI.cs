using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk, kt�ry rozpoczyna now� gr�.
    /// </summary>
    [SerializeField] private Button start_game_button;

    /// <summary>
    /// Przycisk, kt�ry pozwala na za�adowanie zapisanej gry.
    /// </summary>
    [SerializeField] private Button load_game_button;

    /// <summary>
    /// Przycisk, kt�ry otwiera ekran ustawie� gry.
    /// </summary>
    [SerializeField] private Button settings_button;

    /// <summary>
    /// Przycisk, kt�ry umo�liwia zamkni�cie gry.
    /// </summary>
    [SerializeField] private Button exit_game_button;

    [Header("Scripts")]
    /// <summary>
    /// UI ustawie�, kt�re pojawi si� po klikni�ciu przycisku ustawie�.
    /// </summary>
    [SerializeField] private MainMenuSettingsUI settings_UI;


    /// <summary>
    /// Inicjalizuje nas�uchiwanie na klikni�cia przycisk�w g��wnego menu.
    /// </summary>
    private void Start()
    {
        // Nas�uchiwanie klikni�� przycisk�w i przypisanie odpowiednich metod
        start_game_button.onClick.AddListener(OnStartGameButtonClick);
        load_game_button.onClick.AddListener(OnLoadGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        exit_game_button.onClick.AddListener(OnExitGameButtonClick);
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Start Game", �adowanie sceny gry i resetowanie stanu kontynuacji.
    /// </summary>
    private void OnStartGameButtonClick()
    {
        SceneController.LoadScene(SceneController.GAME_SCENE);  // �adowanie sceny gry
        PlayerPrefs.SetInt("ContinueSave", 0);  // Ustawienie warto�ci "ContinueSave" na 0, co oznacza rozpocz�cie nowej gry
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Load Game", �adowanie sceny gry i ustawianie kontynuacji.
    /// </summary>
    private void OnLoadGameButtonClick()
    {
        SceneController.LoadScene(SceneController.GAME_SCENE);  // �adowanie sceny gry
        PlayerPrefs.SetInt("ContinueSave", 1);  // Ustawienie warto�ci "ContinueSave" na 1, co oznacza �adowanie zapisanej gry
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Settings", wy�wietlanie interfejsu ustawie�.
    /// </summary>
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);  // Aktywowanie interfejsu ustawie�
        this.gameObject.SetActive(false);  // Dezaktywowanie bie��cego interfejsu g��wnego menu
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Exit", zamkni�cie aplikacji.
    /// </summary>
    private void OnExitGameButtonClick()
    {
        Application.Quit();  // Zamyka aplikacj�
    }
}
