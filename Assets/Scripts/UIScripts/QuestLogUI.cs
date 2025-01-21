using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private Button left_arrow_button;  // Przycisk nawigacji (lewa strzalka)

    [SerializeField] private PlayerAssetsUI player_assets_UI;  // Odwolanie do UI gracza

    [SerializeField] private Sprite quest_completed_icon;  // Ikona ukonczonej misji
    [SerializeField] private Sprite quest_uncompleted_icon;  // Ikona nieukonczonej misji

    [SerializeField] private List<QuestUI> quests;  // Lista elementow UI dla misji

    /// <summary>
    /// Dodaje nasluchiwanie na przycisk lewej strzalki, ktory otworzy ekran ekwipunku.
    /// </summary>
    private void Start()
    {
        left_arrow_button.onClick.AddListener(OnRightArrowButtonClick);  // Przyciski do zmiany UI
    }

    /// <summary>
    /// Laduje liste misji z menedzera, wyswietla opis i ikony ukonczenia misji w UI.
    /// </summary>
    private void OnEnable()
    {
        List<Quest> quest_list = QuestManager.Instance.GetDefaultQuestsList();  // Pobiera liste domyœlnych misji
        List<Quest> active_quest_list = QuestManager.Instance.GetActiveQuests;

        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].SetQuestHeader(quest_list[i].GetDescription());  // Ustawia opis misji w UI
            if (!active_quest_list.Contains(quest_list[i]))  // Sprawdza, czy misja jest ukonczona
            {
                quests[i].SetComplitionIcon(quest_completed_icon);  // Ustawia ikone ukonczonej misji
            }
            else
            {
                quests[i].SetComplitionIcon(quest_uncompleted_icon);  // Ustawia ikone nieukonczonej misji
            }
        }
    }

    /// <summary>
    /// Zmienia ekran UI na ekran ekwipunku po kliknieciu przycisku strzalki.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);  // Otwiera UI ekwipunku
    }
}
