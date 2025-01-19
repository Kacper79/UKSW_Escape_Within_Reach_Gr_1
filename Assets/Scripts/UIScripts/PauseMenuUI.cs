using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    /// <summary>
    /// Przycisk umo¿liwiaj¹cy powrót do gry.
    /// </summary>
    [SerializeField] private Button back_to_game_button;

    /// <summary>
    /// Przycisk zapisania stanu gry.
    /// </summary>
    [SerializeField] private Button save_game_button;

    /// <summary>
    /// Przycisk otwarcia ekranu ustawieñ gry.
    /// </summary>
    [SerializeField] private Button settings_button;

    /// <summary>
    /// Przycisk umo¿liwiaj¹cy powrót do menu g³ównego.
    /// </summary>
    [SerializeField] private Button back_to_main_menu_button;

    [SerializeField]
    /// <summary>
    /// UI ustawieñ, które zostanie wyœwietlone po klikniêciu przycisku ustawieñ.
    /// </summary>
    private SettingsUI settings_UI;


    /// <summary>
    /// Inicjalizuje nas³uchiwanie na klikniêcia przycisków w menu pauzy.
    /// </summary>
    private void Start()
    {
        // Nas³uchiwanie klikniêæ przycisków i przypisanie odpowiednich metod
        back_to_game_button.onClick.AddListener(OnBackToGameButtonClick);
        save_game_button.onClick.AddListener(OnSaveGameButtonClick);
        settings_button.onClick.AddListener(OnSettingsButtonClick);
        back_to_main_menu_button.onClick.AddListener(OnBackToMainMenuButtonClick);
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Back to Game", wznawia grê oraz zamyka menu pauzy.
    /// </summary>
    private void OnBackToGameButtonClick()
    {
        GlobalEvents.FireOnResumeGame(this);  // Wysy³a zdarzenie o wznowieniu gry
        GlobalEvents.FireOnAnyUIClose(this);  // Wysy³a zdarzenie o zamkniêciu wszystkich UI
        this.gameObject.SetActive(false);  // Dezaktywuje menu pauzy
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Save Game", zapisuje stan gry (brak implementacji zapisu w tej wersji).
    /// </summary>
    private void OnSaveGameButtonClick()
    {
        // TODO: Save System  // Zapis gry (do implementacji)
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Settings", wyœwietla interfejs ustawieñ.
    /// </summary>
    private void OnSettingsButtonClick()
    {
        settings_UI.gameObject.SetActive(true);  // Aktywowanie interfejsu ustawieñ
        this.gameObject.SetActive(false);  // Dezaktywowanie menu pauzy
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku "Back to Main Menu", ³aduje scenê menu g³ównego.
    /// </summary>
    private void OnBackToMainMenuButtonClick()
    {
        SceneController.LoadScene(SceneController.MAIN_MENU_SCENE);  // £adowanie sceny menu g³ównego
    }
}
