using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private Button left_arrow_button;

    [SerializeField] private PlayerAssetsUI player_assets_UI;

    private void Start()
    {
        left_arrow_button.onClick.AddListener(OnRightArrowButtonClick);
    }

    private void OnRightArrowButtonClick()
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);
    }
}
