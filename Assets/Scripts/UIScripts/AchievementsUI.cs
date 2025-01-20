using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    [SerializeField] private Button right_arrow_button;  // Przycisk strzalki w prawo, uzywany do nawigacji
    [SerializeField] private PlayerAssetsUI player_assets_UI;  // Referencja do interfejsu zarzadzajacego zasobami gracza

    /// <summary>
    /// Inicjalizuje nasluchiwanie na klikniecie przycisku strzalki w prawo, aby otworzyc odpowiedni UI.
    /// </summary>
    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
    }

    /// <summary>
    /// Obsluguje klikniecie przycisku strzalki w prawo i otwiera interfejs zasobow gracza.
    /// </summary>
    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);  // Otwiera UI dla ekwipunku gracza
    }
}
