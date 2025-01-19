using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    [SerializeField] private Button right_arrow_button;  // Przycisk strza³ki w prawo, u¿ywany do nawigacji
    [SerializeField] private PlayerAssetsUI player_assets_UI;  // Referencja do interfejsu zarz¹dzaj¹cego zasobami gracza

    /// <summary>
    /// Inicjalizuje nas³uchiwanie na klikniêcie przycisku strza³ki w prawo, aby otworzyæ odpowiedni UI.
    /// </summary>
    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
    }

    /// <summary>
    /// Obs³uguje klikniêcie przycisku strza³ki w prawo i otwiera interfejs zasobów gracza.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);  // Otwiera UI dla ekwipunku gracza
    }
}
