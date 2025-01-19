using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetsUI : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// UI dla ekwipunku gracza, zarz¹dza wyœwietlaniem ekwipunku.
    /// </summary>
    private InventoryUI inventory_UI;

    [SerializeField]
    /// <summary>
    /// UI dla osi¹gniêæ gracza, zarz¹dza wyœwietlaniem osi¹gniêæ.
    /// </summary>
    private AchievementsUI achievements_UI;

    [SerializeField]
    /// <summary>
    /// UI dla dziennika misji gracza, zarz¹dza wyœwietlaniem misji.
    /// </summary>
    private QuestLogUI quest_log_UI;

    [SerializeField]
    /// <summary>
    /// Transformacja dla UI gracza, okreœla pozycjê i skalowanie UI.
    /// </summary>
    private RectTransform rect_transform;

    private const int WINDOW_X_OFFSET = -1920;
    /// <summary>
    /// Przesuniêcie X dla ró¿nych ekranów UI.
    /// </summary>

    private const int X_STARTING_POSITION = 0;
    /// <summary>
    /// Pocz¹tkowa pozycja X UI.
    /// </summary>


    // Enum dla okreœlenia dostêpnych ekranów UI
    public enum UIs
    {
        Achievements,  // Ekran osi¹gniêæ
        Inventory,     // Ekran ekwipunku
        QuestLog       // Ekran dziennika misji
    }

    // S³ownik przechowuj¹cy przesuniêcia dla ka¿dego typu UI
    public Dictionary<UIs, int> offsets_for_each_ui = new();

    /// <summary>
    /// Inicjalizuje s³ownik przechowuj¹cy przesuniêcia dla ró¿nych ekranów UI.
    /// </summary>
    private void Awake()
    {
        FillUIsDicttionary();  // Wype³nia s³ownik offsetów
    }

    /// <summary>
    /// Ustawia UI na nieaktywne przy w³¹czeniu obiektu.
    /// </summary>
    private void OnEnable()
    {
        DisableUI();  // Dezaktywuje wszystkie UI po w³¹czeniu
    }

    /// <summary>
    /// Aktywuje wszystkie dostêpne UI.
    /// </summary>
    private void EnableUI()
    {
        inventory_UI.gameObject.SetActive(true);  // Aktywuje ekran ekwipunku
        quest_log_UI.gameObject.SetActive(true);  // Aktywuje ekran dziennika misji
        achievements_UI.gameObject.SetActive(true);  // Aktywuje ekran osi¹gniêæ
    }

    /// <summary>
    /// Dezaktywuje wszystkie dostêpne UI.
    /// </summary>
    private void DisableUI()
    {
        inventory_UI.gameObject.SetActive(false);  // Dezaktywuje ekran ekwipunku
        quest_log_UI.gameObject.SetActive(false);  // Dezaktywuje ekran dziennika misji
        achievements_UI.gameObject.SetActive(false);  // Dezaktywuje ekran osi¹gniêæ
    }

    /// <summary>
    /// Otwiera okreœlony ekran UI, ustawia jego pozycjê na ekranie oraz wywo³uje zdarzenie otwarcia UI.
    /// </summary>
    /// <param name="ui">Typ UI do otwarcia</param>
    public void OpenUI(UIs ui)
    {
        GlobalEvents.FireOnAnyUIOpen(this);  // Wysy³a zdarzenie o otwarciu UI
        ResetPosition();  // Resetuje pozycjê UI
        EnableUI();  // Aktywuje wszystkie UI
        SetXPosition(offsets_for_each_ui[ui]);  // Ustawia pozycjê X na podstawie przesuniêcia
    }

    /// <summary>
    /// Wype³nia s³ownik `offsets_for_each_ui` przesuniêciami dla ka¿dego ekranu UI.
    /// </summary>
    private void FillUIsDicttionary()
    {
        offsets_for_each_ui.Add(UIs.Achievements, 0 * WINDOW_X_OFFSET);  // Ekran osi¹gniêæ
        offsets_for_each_ui.Add(UIs.Inventory, 1 * WINDOW_X_OFFSET);  // Ekran ekwipunku
        offsets_for_each_ui.Add(UIs.QuestLog, 2 * WINDOW_X_OFFSET);  // Ekran dziennika misji
    }

    /// <summary>
    /// Ustawia pozycjê X UI na podstawie podanego offsetu.
    /// </summary>
    /// <param name="x_offset">Przesuniêcie X do zastosowania</param>
    private void SetXPosition(int x_offset)
    {
        rect_transform.localPosition = new Vector3(rect_transform.localPosition.x + x_offset, rect_transform.localPosition.y, rect_transform.localPosition.z);
    }

    /// <summary>
    /// Zamyka wszystkie UI i resetuje ich pozycjê.
    /// </summary>
    public void CloseUI()
    {
        ResetPosition();  // Resetuje pozycjê UI
        DisableUI();  // Dezaktywuje wszystkie UI
    }

    /// <summary>
    /// Resetuje pozycjê UI do pocz¹tkowej wartoœci.
    /// </summary>
    private void ResetPosition()
    {
        rect_transform.localPosition = new Vector3(X_STARTING_POSITION, rect_transform.localPosition.y, rect_transform.localPosition.z);  // Resetuje pozycjê X
    }
}
