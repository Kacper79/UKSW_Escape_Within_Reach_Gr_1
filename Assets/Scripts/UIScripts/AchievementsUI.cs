using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    [SerializeField] private Button right_arrow_button;  // Przycisk strza�ki w prawo, u�ywany do nawigacji
    [SerializeField] private PlayerAssetsUI player_assets_UI;  // Referencja do interfejsu zarz�dzaj�cego zasobami gracza

    /// <summary>
    /// Inicjalizuje nas�uchiwanie na klikni�cie przycisku strza�ki w prawo, aby otworzy� odpowiedni UI.
    /// </summary>
    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku strza�ki w prawo i otwiera interfejs zasob�w gracza.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);  // Otwiera UI dla ekwipunku gracza
    }
}
