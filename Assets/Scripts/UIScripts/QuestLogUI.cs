using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private Button left_arrow_button;  // Przycisk nawigacji (lewa strza�ka)

    [SerializeField] private PlayerAssetsUI player_assets_UI;  // Odwo�anie do UI gracza
    [SerializeField] private QuestManager quest_manager;  // Mened�er misji, zarz�dza post�pem

    [SerializeField] private Sprite quest_completed_icon;  // Ikona uko�czonej misji
    [SerializeField] private Sprite quest_uncompleted_icon;  // Ikona nieuko�czonej misji

    [SerializeField] private List<QuestUI> quests;  // Lista element�w UI dla misji

    /// <summary>
    /// Dodaje nas�uchiwanie na przycisk lewej strza�ki, kt�ry otworzy ekran ekwipunku.
    /// </summary>
    private void Start()
    {
        left_arrow_button.onClick.AddListener(OnRightArrowButtonClick);  // Przyciski do zmiany UI
    }

    /// <summary>
    /// �aduje list� misji z mened�era, wy�wietla opis i ikony uko�czenia misji w UI.
    /// </summary>
    private void OnEnable()
    {
        List<Quest> quest_list = QuestManager.Instance.GetDefaultQuestsList();  // Pobiera list� domy�lnych misji

        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].SetQuestHeader(quest_list[i].GetDescription());  // Ustawia opis misji w UI
            if (quest_manager.IsQuestCompleted(i))  // Sprawdza, czy misja jest uko�czona
            {
                quests[i].SetComplitionIcon(quest_completed_icon);  // Ustawia ikon� uko�czonej misji
            }
            else
            {
                quests[i].SetComplitionIcon(quest_uncompleted_icon);  // Ustawia ikon� nieuko�czonej misji
            }
        }
    }

    /// <summary>
    /// Zmienia ekran UI na ekran ekwipunku po klikni�ciu przycisku strza�ki.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);  // Otwiera UI ekwipunku
    }
}
