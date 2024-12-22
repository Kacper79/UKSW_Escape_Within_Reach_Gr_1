using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetsUI : MonoBehaviour
{
    [SerializeField] private InventoryUI inventory_UI;
    [SerializeField] private AchievementsUI achievements_UI;
    [SerializeField] private QuestLogUI quest_log_UI;

    [SerializeField] private RectTransform rect_transform;

    private const int WINDOW_X_OFFSET = -1920;
    private const int X_STARTING_POSITION = 0;

    public enum UIs
    {
        Achievements,
        Inventory,
        QuestLog
    }

    public Dictionary<UIs, int> offsets_for_each_ui = new();

    private void Awake()
    {
        FillUIsDicttionary();
    }

    private void OnEnable()
    {
        DisableUI();
    }
    private void EnableUI()
    {
        inventory_UI.gameObject.SetActive(true);
        quest_log_UI.gameObject.SetActive(true);
        achievements_UI.gameObject.SetActive(true);
    }
    private void DisableUI()
    {
        inventory_UI.gameObject.SetActive(false);
        quest_log_UI.gameObject.SetActive(false);
        achievements_UI.gameObject.SetActive(false);
    }

    public void OpenUI(UIs ui)
    {
        GlobalEvents.FireOnAnyUIOpen(this);
        ResetPosition();
        EnableUI();
        SetXPosition(offsets_for_each_ui[ui]);
    }

    private void FillUIsDicttionary()
    {
        offsets_for_each_ui.Add(UIs.Achievements, 0 * WINDOW_X_OFFSET);
        offsets_for_each_ui.Add(UIs.Inventory, 1 * WINDOW_X_OFFSET);
        offsets_for_each_ui.Add(UIs.QuestLog, 2 * WINDOW_X_OFFSET);
    }

    private void SetXPosition(int x_offset)
    {
        rect_transform.localPosition = new Vector3(rect_transform.localPosition.x + x_offset, rect_transform.localPosition.y, rect_transform.localPosition.z);
    }

    public void CloseUI()
    {
        ResetPosition();
        DisableUI();
    }

    private void ResetPosition()
    {
        rect_transform.localPosition = new(X_STARTING_POSITION, rect_transform.localPosition.y, rect_transform.localPosition.z);
    }
}
