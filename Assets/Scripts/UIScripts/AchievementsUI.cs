using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    [SerializeField] private Button right_arrow_button;

    [SerializeField] private PlayerAssetsUI player_assets_UI;

    private void Start()
    {
        right_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
    }

    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);
    }
}
