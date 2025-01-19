using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetsUI : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// UI dla ekwipunku gracza, zarz�dza wy�wietlaniem ekwipunku.
    /// </summary>
    private InventoryUI inventory_UI;

    [SerializeField]
    /// <summary>
    /// UI dla osi�gni�� gracza, zarz�dza wy�wietlaniem osi�gni��.
    /// </summary>
    private AchievementsUI achievements_UI;

    [SerializeField]
    /// <summary>
    /// UI dla dziennika misji gracza, zarz�dza wy�wietlaniem misji.
    /// </summary>
    private QuestLogUI quest_log_UI;

    [SerializeField]
    /// <summary>
    /// Transformacja dla UI gracza, okre�la pozycj� i skalowanie UI.
    /// </summary>
    private RectTransform rect_transform;

    private const int WINDOW_X_OFFSET = -1920;
    /// <summary>
    /// Przesuni�cie X dla r�nych ekran�w UI.
    /// </summary>

    private const int X_STARTING_POSITION = 0;
    /// <summary>
    /// Pocz�tkowa pozycja X UI.
    /// </summary>


    // Enum dla okre�lenia dost�pnych ekran�w UI
    public enum UIs
    {
        Achievements,  // Ekran osi�gni��
        Inventory,     // Ekran ekwipunku
        QuestLog       // Ekran dziennika misji
    }

    // S�ownik przechowuj�cy przesuni�cia dla ka�dego typu UI
    public Dictionary<UIs, int> offsets_for_each_ui = new();

    /// <summary>
    /// Inicjalizuje s�ownik przechowuj�cy przesuni�cia dla r�nych ekran�w UI.
    /// </summary>
    private void Awake()
    {
        FillUIsDicttionary();  // Wype�nia s�ownik offset�w
    }

    /// <summary>
    /// Ustawia UI na nieaktywne przy w��czeniu obiektu.
    /// </summary>
    private void OnEnable()
    {
        DisableUI();  // Dezaktywuje wszystkie UI po w��czeniu
    }

    /// <summary>
    /// Aktywuje wszystkie dost�pne UI.
    /// </summary>
    private void EnableUI()
    {
        inventory_UI.gameObject.SetActive(true);  // Aktywuje ekran ekwipunku
        quest_log_UI.gameObject.SetActive(true);  // Aktywuje ekran dziennika misji
        achievements_UI.gameObject.SetActive(true);  // Aktywuje ekran osi�gni��
    }

    /// <summary>
    /// Dezaktywuje wszystkie dost�pne UI.
    /// </summary>
    private void DisableUI()
    {
        inventory_UI.gameObject.SetActive(false);  // Dezaktywuje ekran ekwipunku
        quest_log_UI.gameObject.SetActive(false);  // Dezaktywuje ekran dziennika misji
        achievements_UI.gameObject.SetActive(false);  // Dezaktywuje ekran osi�gni��
    }

    /// <summary>
    /// Otwiera okre�lony ekran UI, ustawia jego pozycj� na ekranie oraz wywo�uje zdarzenie otwarcia UI.
    /// </summary>
    /// <param name="ui">Typ UI do otwarcia</param>
    public void OpenUI(UIs ui)
    {
        GlobalEvents.FireOnAnyUIOpen(this);  // Wysy�a zdarzenie o otwarciu UI
        ResetPosition();  // Resetuje pozycj� UI
        EnableUI();  // Aktywuje wszystkie UI
        SetXPosition(offsets_for_each_ui[ui]);  // Ustawia pozycj� X na podstawie przesuni�cia
    }

    /// <summary>
    /// Wype�nia s�ownik `offsets_for_each_ui` przesuni�ciami dla ka�dego ekranu UI.
    /// </summary>
    private void FillUIsDicttionary()
    {
        offsets_for_each_ui.Add(UIs.Achievements, 0 * WINDOW_X_OFFSET);  // Ekran osi�gni��
        offsets_for_each_ui.Add(UIs.Inventory, 1 * WINDOW_X_OFFSET);  // Ekran ekwipunku
        offsets_for_each_ui.Add(UIs.QuestLog, 2 * WINDOW_X_OFFSET);  // Ekran dziennika misji
    }

    /// <summary>
    /// Ustawia pozycj� X UI na podstawie podanego offsetu.
    /// </summary>
    /// <param name="x_offset">Przesuni�cie X do zastosowania</param>
    private void SetXPosition(int x_offset)
    {
        rect_transform.localPosition = new Vector3(rect_transform.localPosition.x + x_offset, rect_transform.localPosition.y, rect_transform.localPosition.z);
    }

    /// <summary>
    /// Zamyka wszystkie UI i resetuje ich pozycj�.
    /// </summary>
    public void CloseUI()
    {
        ResetPosition();  // Resetuje pozycj� UI
        DisableUI();  // Dezaktywuje wszystkie UI
    }

    /// <summary>
    /// Resetuje pozycj� UI do pocz�tkowej warto�ci.
    /// </summary>
    private void ResetPosition()
    {
        rect_transform.localPosition = new Vector3(X_STARTING_POSITION, rect_transform.localPosition.y, rect_transform.localPosition.z);  // Resetuje pozycj� X
    }
}
