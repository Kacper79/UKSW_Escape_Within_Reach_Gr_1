using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FunctionalPlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Odwo³anie do komponentu PlayerInput.
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Odwo³anie do UI gracza.
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;

    /// <summary>
    /// Odwo³anie do UI pauzy.
    /// </summary>
    [SerializeField] private PauseMenuUI pause_menu_UI;


    /// <summary>
    /// Aktywuje wszystkie akcje przypisane do gracza.
    /// </summary>
    private void OnEnable()
    {
        player_input.FunctionalPlayerInput.Enable();  // W³¹cza wszystkie akcje wejœciowe

        // Subskrypcja do poszczególnych akcji (np. otwieranie ekwipunku)
        player_input.FunctionalPlayerInput.OpenInventory.performed += OpenInventoryPerformed;
        player_input.FunctionalPlayerInput.OpenAchievements.performed += OpenAchievementsPerformed;
        player_input.FunctionalPlayerInput.OpenQuestLog.performed += OpenQuestLogPerformed;
        player_input.FunctionalPlayerInput.Pause.performed += PausePerformed;
        player_input.FunctionalPlayerInput.ThrowCoin.started += ThrowCoin;
        player_input.FunctionalPlayerInput.UseCigaretts.started += UseCigartetts;
        player_input.FunctionalPlayerInput.FlushAnalytics.started += FlushAnalytics;

        // Nas³uchiwanie zdarzeñ otwierania i zamykania UI
        GlobalEvents.OnAnyUIOpen += DisableFunctionalPlayerInput;
        GlobalEvents.OnAnyUIClose += EnableFunctionalPlayerInput;
    }

    /// <summary>
    /// Dezaktywuje wszystkie akcje przypisane do gracza.
    /// </summary>
    private void OnDisable()
    {
        player_input.FunctionalPlayerInput.Disable();  // Wy³¹cza wszystkie akcje wejœciowe

        // Anulowanie subskrypcji do akcji
        player_input.FunctionalPlayerInput.OpenInventory.performed -= OpenInventoryPerformed;
        player_input.FunctionalPlayerInput.OpenAchievements.performed -= OpenAchievementsPerformed;
        player_input.FunctionalPlayerInput.OpenQuestLog.performed -= OpenQuestLogPerformed;
        player_input.FunctionalPlayerInput.Pause.performed -= PausePerformed;
        player_input.FunctionalPlayerInput.ThrowCoin.started -= ThrowCoin;
        player_input.FunctionalPlayerInput.UseCigaretts.started -= UseCigartetts;
        player_input.FunctionalPlayerInput.FlushAnalytics.started -= FlushAnalytics;

        // Anulowanie nas³uchiwania na zdarzenia UI
        GlobalEvents.OnAnyUIOpen -= DisableFunctionalPlayerInput;
        GlobalEvents.OnAnyUIClose -= EnableFunctionalPlayerInput;
    }

    /// <summary>
    /// Wy³¹cza mo¿liwoœæ wprowadzania danych przez gracza, gdy UI jest otwarte.
    /// </summary>
    private void DisableFunctionalPlayerInput(object sender, EventArgs e)
    {
        player_input.FunctionalPlayerInput.Disable();  // Wy³¹cza akcje wejœciowe
    }

    /// <summary>
    /// W³¹cza mo¿liwoœæ wprowadzania danych przez gracza, gdy UI jest zamkniête.
    /// </summary>
    private void EnableFunctionalPlayerInput(object sender, EventArgs e)
    {
        player_input.FunctionalPlayerInput.Enable();  // W³¹cza akcje wejœciowe
    }

    /// <summary>
    /// Akcja dla przycisku pauzy, wywo³uje menu pauzy.
    /// </summary>
    private void PausePerformed(InputAction.CallbackContext obj)
    {
        pause_menu_UI.gameObject.SetActive(true);  // Aktywuje menu pauzy
        GlobalEvents.FireOnPauseGame(this);  // Wysy³a zdarzenie pauzy
        GlobalEvents.FireOnAnyUIOpen(this);  // Wysy³a zdarzenie otwarcia UI
    }

    /// <summary>
    /// Akcja dla przycisku otwierania ekwipunku.
    /// </summary>
    private void OpenInventoryPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Inventory);  // Otwiera UI ekwipunku
    }

    /// <summary>
    /// Akcja dla przycisku otwierania dziennika misji.
    /// </summary>
    private void OpenQuestLogPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.QuestLog);  // Otwiera UI dziennika misji
    }

    /// <summary>
    /// Akcja dla przycisku otwierania osi¹gniêæ.
    /// </summary>
    private void OpenAchievementsPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Achievements);  // Otwiera UI osi¹gniêæ
    }

    /// <summary>
    /// Ustawia PlayerInput dla tego kontrolera.
    /// </summary>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;  // Ustawia komponent PlayerInput
    }

    /// <summary>
    /// Akcja dla rzucenia monet¹.
    /// </summary>
    public void ThrowCoin(InputAction.CallbackContext ctx)
    {
        GlobalEvents.FireOnThrowingCoin(this);  // Wysy³a zdarzenie rzucenia monet¹
    }

    /// <summary>
    /// Akcja dla palenia papierosów.
    /// </summary>
    public void UseCigartetts(InputAction.CallbackContext ctx)
    {
        GlobalEvents.FireOnUseCigs(this);  // Wysy³a zdarzenie u¿ycia papierosów
    }

    public void FlushAnalytics(InputAction.CallbackContext ctx)
    {
        GameAnalytics.Instance.ClearStatsOnFreshSave(true);
    }
}
