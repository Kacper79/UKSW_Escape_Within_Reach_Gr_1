using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk, ktory rozpoczyna nowa gre.
    /// </summary>
    [SerializeField] private Button start_game_button;

    /// <summary>
    /// Przycisk, ktory pozwala na zaladowanie zapisanej gry.
    /// </summary>
    [SerializeField] private Button load_game_button;

    /// <summary>
    /// Przycisk, ktory otwiera ekran ustawien gry.
    /// </summary>
    [SerializeField] private Button settings_button;

    /// <summary>
    /// Przycisk, ktory umozliwia zamkniecie gry.
    /// </summary>
    [SerializeField] private Button exit_game_button;

    [Header("Scripts")]
    /// <summary>
    /// UI ustawien, ktore pojawi sie po kliknieciu przycisku ustawien.
    /// </summary>
    [SerializeField] private MainMenuSettingsUI settings_UI;


    /// <summary>
    /// Inicjalizuje nasluchiwanie na klikniecia przyciskow glownego menu.
    /// </summary>
    private void Start()
    {
        // Nasluchiwanie kliknieæ przyciskow i przypisanie odpowiednich metod
        start_game_button.onClick.AddListener(OnStartGameButtonClick);
        load_game_button.onClick.AddListener(OnLoadGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        exit_game_button.onClick.AddListener(OnExitGameButtonClick);
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Start Game", ladowanie sceny gry i resetowanie stanu kontynuacji.
    /// </summary>
    private void OnStartGameButtonClick()
    {
        SceneController.LoadScene(SceneController.GAME_SCENE);  // ladowanie sceny gry
        PlayerPrefs.SetInt("ContinueSave", 0);  // Ustawienie wartosci "ContinueSave" na 0, co oznacza rozpoczecie nowej gry
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Load Game", ladowanie sceny gry i ustawianie kontynuacji.
    /// </summary>
    private void OnLoadGameButtonClick()
    {
        SceneController.LoadScene(SceneController.GAME_SCENE);  // ladowanie sceny gry
        PlayerPrefs.SetInt("ContinueSave", 1);  // Ustawienie wartosci "ContinueSave" na 1, co oznacza ladowanie zapisanej gry
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Settings", wyswietlanie interfejsu ustawien.
    /// </summary>
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);  // Aktywowanie interfejsu ustawien
        this.gameObject.SetActive(false);  // Dezaktywowanie biezacego interfejsu glownego menu
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Exit", zamkniecie aplikacji.
    /// </summary>
    private void OnExitGameButtonClick()
    {
        Application.Quit();  // Zamyka aplikacje
    }
}
