using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetsUI : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// UI dla ekwipunku gracza, zarzadza wyswietlaniem ekwipunku.
    /// </summary>
    private InventoryUI inventory_UI;

    [SerializeField]
    /// <summary>
    /// UI dla osiagniec gracza, zarzadza wyswietlaniem osiagniec.
    /// </summary>
    private AchievementsUI achievements_UI;

    [SerializeField]
    /// <summary>
    /// UI dla dziennika misji gracza, zarzadza wyswietlaniem misji.
    /// </summary>
    private QuestLogUI quest_log_UI;

    [SerializeField]
    /// <summary>
    /// Transformacja dla UI gracza, okresla pozycje i skalowanie UI.
    /// </summary>
    private RectTransform rect_transform;

    private const int WINDOW_X_OFFSET = -1920;
    /// <summary>
    /// Przesuniecie X dla roznych ekranow UI.
    /// </summary>

    private const int X_STARTING_POSITION = 0;
    /// <summary>
    /// Poczatkowa pozycja X UI.
    /// </summary>


    // Enum dla okreslenia dostepnych ekranow UI
    public enum UIs
    {
        Achievements,  // Ekran osiagniec
        Inventory,     // Ekran ekwipunku
        QuestLog       // Ekran dziennika misji
    }

    // Slownik przechowujacy przesuniecia dla kazdego typu UI
    public Dictionary<UIs, int> offsets_for_each_ui = new();

    /// <summary>
    /// Inicjalizuje slownik przechowujacy przesuniecia dla roznych ekranow UI.
    /// </summary>
    private void Awake()
    {
        FillUIsDicttionary();  // Wypelnia slownik offsetow
    }

    /// <summary>
    /// Ustawia UI na nieaktywne przy wlaczeniu obiektu.
    /// </summary>
    private void OnEnable()
    {
        DisableUI();  // Dezaktywuje wszystkie UI po wlaczeniu
    }

    /// <summary>
    /// Aktywuje wszystkie dostepne UI.
    /// </summary>
    private void EnableUI()
    {
        inventory_UI.gameObject.SetActive(true);  // Aktywuje ekran ekwipunku
        quest_log_UI.gameObject.SetActive(true);  // Aktywuje ekran dziennika misji
        achievements_UI.gameObject.SetActive(true);  // Aktywuje ekran osiagniec
    }

    /// <summary>
    /// Dezaktywuje wszystkie dostepne UI.
    /// </summary>
    private void DisableUI()
    {
        inventory_UI.gameObject.SetActive(false);  // Dezaktywuje ekran ekwipunku
        quest_log_UI.gameObject.SetActive(false);  // Dezaktywuje ekran dziennika misji
        achievements_UI.gameObject.SetActive(false);  // Dezaktywuje ekran osiagniec
    }

    /// <summary>
    /// Otwiera okreslony ekran UI, ustawia jego pozycje na ekranie oraz wywoluje zdarzenie otwarcia UI.
    /// </summary>
    /// <param name="ui">Typ UI do otwarcia</param>
    public void OpenUI(UIs ui)
    {
        GlobalEvents.FireOnAnyUIOpen(this);  // Wysyla zdarzenie o otwarciu UI
        ResetPosition();  // Resetuje pozycje UI
        EnableUI();  // Aktywuje wszystkie UI
        SetXPosition(offsets_for_each_ui[ui]);  // Ustawia pozycje X na podstawie przesuniecia
    }

    /// <summary>
    /// Wypelnia slownik `offsets_for_each_ui` przesunieciami dla kazdego ekranu UI.
    /// </summary>
    private void FillUIsDicttionary()
    {
        offsets_for_each_ui.Add(UIs.Achievements, 0 * WINDOW_X_OFFSET);  // Ekran osiagniec
        offsets_for_each_ui.Add(UIs.Inventory, 1 * WINDOW_X_OFFSET);  // Ekran ekwipunku
        offsets_for_each_ui.Add(UIs.QuestLog, 2 * WINDOW_X_OFFSET);  // Ekran dziennika misji
    }

    /// <summary>
    /// Ustawia pozycje X UI na podstawie podanego offsetu.
    /// </summary>
    /// <param name="x_offset">Przesuniecie X do zastosowania</param>
    private void SetXPosition(int x_offset)
    {
        rect_transform.localPosition = new Vector3(rect_transform.localPosition.x + x_offset, rect_transform.localPosition.y, rect_transform.localPosition.z);
    }

    /// <summary>
    /// Zamyka wszystkie UI i resetuje ich pozycje.
    /// </summary>
    public void CloseUI()
    {
        ResetPosition();  // Resetuje pozycje UI
        DisableUI();  // Dezaktywuje wszystkie UI
    }

    /// <summary>
    /// Resetuje pozycje UI do poczatkowej wartosci.
    /// </summary>
    private void ResetPosition()
    {
        rect_transform.localPosition = new Vector3(X_STARTING_POSITION, rect_transform.localPosition.y, rect_transform.localPosition.z);  // Resetuje pozycje X
    }
}
