using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk umo�liwiaj�cy powr�t do gry.
    /// </summary>
    [SerializeField] private Button back_to_game_button;

    /// <summary>
    /// Przycisk zapisania stanu gry.
    /// </summary>
    [SerializeField] private Button save_game_button;

    /// <summary>
    /// Przycisk otwarcia ekranu ustawie� gry.
    /// </summary>
    [SerializeField] private Button settings_button;

    /// <summary>
    /// Przycisk umo�liwiaj�cy powr�t do menu g��wnego.
    /// </summary>
    [SerializeField] private Button back_to_main_menu_button;

    [SerializeField]
    /// <summary>
    /// UI ustawie�, kt�re zostanie wy�wietlone po klikni�ciu przycisku ustawie�.
    /// </summary>
    private SettingsUI settings_UI;


    /// <summary>
    /// Inicjalizuje nas�uchiwanie na klikni�cia przycisk�w w menu pauzy.
    /// </summary>
    private void Start()
    {
        // Nas�uchiwanie klikni�� przycisk�w i przypisanie odpowiednich metod
        back_to_game_button.onClick.AddListener(OnBackToGameButtonClick);
        save_game_button.onClick.AddListener(OnSaveGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        back_to_main_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Back to Game", wznawia gr� oraz zamyka menu pauzy.
    /// </summary>
    private void OnBackToGameButtonClick()
    {
        GlobalEvents.FireOnResumeGame(this);  // Wysy�a zdarzenie o wznowieniu gry
        GlobalEvents.FireOnAnyUIClose(this);  // Wysy�a zdarzenie o zamkni�ciu wszystkich UI
        this.gameObject.SetActive(false);  // Dezaktywuje menu pauzy
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Save Game", zapisuje stan gry (brak implementacji zapisu w tej wersji).
    /// </summary>
    private void OnSaveGameButtonClick()
    {
        // TODO: Save System  // Zapis gry (do implementacji)
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Settings", wy�wietla interfejs ustawie�.
    /// </summary>
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);  // Aktywowanie interfejsu ustawie�
        this.gameObject.SetActive(false);  // Dezaktywowanie menu pauzy
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Back to Main Menu", �aduje scen� menu g��wnego.
    /// </summary>
    private void OnBackToMainMenuButtonClick()
    {
        SceneController.LoadScene(SceneController.MAIN_MENU_SCENE);  // �adowanie sceny menu g��wnego
    }
}
