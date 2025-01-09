using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FunctionalPlayerInputController : MonoBehaviour
{
    private PlayerInput player_input;

    [SerializeField] private PlayerAssetsUI player_assets_UI;
    [SerializeField] private PauseMenuUI pause_menu_UI;

    private void OnEnable()
    {
        player_input.FunctionalPlayerInput.Enable();

        player_input.FunctionalPlayerInput.OpenInventory.performed += OpenInventoryPerformed;
        player_input.FunctionalPlayerInput.OpenAchievements.performed += OpenAchievementsPerformed;
        player_input.FunctionalPlayerInput.OpenQuestLog.performed += OpenQuestLogPerformed;
        player_input.FunctionalPlayerInput.Pause.performed += PausePerformed;
        player_input.FunctionalPlayerInput.ThrowCoin.started += ThrowCoin;
        player_input.FunctionalPlayerInput.UseCigaretts.started += UseCigartetts;

        GlobalEvents.OnAnyUIOpen += DisableFunctionalPlayerInput;
        GlobalEvents.OnAnyUIClose += EnableFunctionalPlayerInput;

    }   

    private void OnDisable()
    {
        player_input.FunctionalPlayerInput.Disable();

        player_input.FunctionalPlayerInput.OpenInventory.performed -= OpenInventoryPerformed;
        player_input.FunctionalPlayerInput.OpenAchievements.performed -= OpenAchievementsPerformed;
        player_input.FunctionalPlayerInput.OpenQuestLog.performed -= OpenQuestLogPerformed;
        player_input.FunctionalPlayerInput.Pause.performed -= PausePerformed;
        player_input.FunctionalPlayerInput.ThrowCoin.started -= ThrowCoin;
        player_input.FunctionalPlayerInput.UseCigaretts.started -= UseCigartetts;

        GlobalEvents.OnAnyUIOpen -= DisableFunctionalPlayerInput;
        GlobalEvents.OnAnyUIClose -= EnableFunctionalPlayerInput;

    }

    private void DisableFunctionalPlayerInput(object sender, EventArgs e)
    {
        player_input.FunctionalPlayerInput.Disable();
    }

    private void EnableFunctionalPlayerInput(object sender, EventArgs e)
    {
        player_input.FunctionalPlayerInput.Enable();
    }

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        pause_menu_UI.gameObject.SetActive(true);
        GlobalEvents.FireOnPauseGame(this);
        GlobalEvents.FireOnAnyUIOpen(this);
    }

    private void OpenInventoryPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);
    }

    private void OpenQuestLogPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.QuestLog);
    }

    private void OpenAchievementsPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Achievements);
    }
                
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }

    public void ThrowCoin(InputAction.CallbackContext ctx)
    {
        GlobalEvents.FireOnThrowingCoin(this);
    }

    public void UseCigartetts(InputAction.CallbackContext ctx)
    {
        GlobalEvents.FireOnUseCigs(this);
    }
}
