using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk umozliwiajacy powrot do gry.
    /// </summary>
    [SerializeField] private Button back_to_game_button;

    /// <summary>
    /// Przycisk zapisania stanu gry.
    /// </summary>
    [SerializeField] private Button save_game_button;

    /// <summary>
    /// Przycisk otwarcia ekranu ustawien gry.
    /// </summary>
    [SerializeField] private Button settings_button;

    /// <summary>
    /// Przycisk umozliwiajacy powrot do menu glownego.
    /// </summary>
    [SerializeField] private Button back_to_main_menu_button;

    [SerializeField]
    /// <summary>
    /// UI ustawien, ktore zostanie wyswietlone po kliknieciu przycisku ustawien.
    /// </summary>
    private SettingsUI settings_UI;


    /// <summary>
    /// Inicjalizuje nasluchiwanie na klikniecia przyciskow w menu pauzy.
    /// </summary>
    private void Start()
    {
        // Nasluchiwanie kliknieæ przyciskow i przypisanie odpowiednich metod
        back_to_game_button.onClick.AddListener(OnBackToGameButtonClick);
        save_game_button.onClick.AddListener(OnSaveGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        back_to_main_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Back to Game", wznawia gre oraz zamyka menu pauzy.
    /// </summary>
    private void OnBackToGameButtonClick()
    {
        GlobalEvents.FireOnResumeGame(this);  // Wysyla zdarzenie o wznowieniu gry
        GlobalEvents.FireOnAnyUIClose(this);  // Wysyla zdarzenie o zamknieciu wszystkich UI
        this.gameObject.SetActive(false);  // Dezaktywuje menu pauzy
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Save Game", zapisuje stan gry (brak implementacji zapisu w tej wersji).
    /// </summary>
    private void OnSaveGameButtonClick()
    {
        // TODO: Save System  // Zapis gry (do implementacji)
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Settings", wyswietla interfejs ustawien.
    /// </summary>
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);  // Aktywowanie interfejsu ustawien
        this.gameObject.SetActive(false);  // Dezaktywowanie menu pauzy
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku "Back to Main Menu", ³aduje scene menu glownego.
    /// </summary>
    private void OnBackToMainMenuButtonClick()
    {
        SceneController.LoadScene(SceneController.MAIN_MENU_SCENE);  // ladowanie sceny menu glownego
    }
}
