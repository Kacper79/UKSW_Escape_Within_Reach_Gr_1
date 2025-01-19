using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private Button left_arrow_button;  // Przycisk nawigacji (lewa strza³ka)

    [SerializeField] private PlayerAssetsUI player_assets_UI;  // Odwo³anie do UI gracza
    [SerializeField] private QuestManager quest_manager;  // Mened¿er misji, zarz¹dza postêpem

    [SerializeField] private Sprite quest_completed_icon;  // Ikona ukoñczonej misji
    [SerializeField] private Sprite quest_uncompleted_icon;  // Ikona nieukoñczonej misji

    [SerializeField] private List<QuestUI> quests;  // Lista elementów UI dla misji

    /// <summary>
    /// Dodaje nas³uchiwanie na przycisk lewej strza³ki, który otworzy ekran ekwipunku.
    /// </summary>
    private void Start()
    {
        left_arrow_button.onClick.AddListener(OnRightArrowButtonClick);  // Przyciski do zmiany UI
    }

    /// <summary>
    /// £aduje listê misji z mened¿era, wyœwietla opis i ikony ukoñczenia misji w UI.
    /// </summary>
    private void OnEnable()
    {
        List<Quest> quest_list = QuestManager.Instance.GetDefaultQuestsList();  // Pobiera listê domyœlnych misji

        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].SetQuestHeader(quest_list[i].GetDescription());  // Ustawia opis misji w UI
            if (quest_manager.IsQuestCompleted(i))  // Sprawdza, czy misja jest ukoñczona
            {
                quests[i].SetComplitionIcon(quest_completed_icon);  // Ustawia ikonê ukoñczonej misji
            }
            else
            {
                quests[i].SetComplitionIcon(quest_uncompleted_icon);  // Ustawia ikonê nieukoñczonej misji
            }
        }
    }

    /// <summary>
    /// Zmienia ekran UI na ekran ekwipunku po klikniêciu przycisku strza³ki.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);  // Otwiera UI ekwipunku
    }
}
