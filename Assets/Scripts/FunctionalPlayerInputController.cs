using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FunctionalPlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Odwolanie do komponentu PlayerInput.
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Odwolanie do UI gracza.
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;

    /// <summary>
    /// Odwolanie do UI pauzy.
    /// </summary>
    [SerializeField] private PauseMenuUI pause_menu_UI;


    /// <summary>
    /// Aktywuje wszystkie akcje przypisane do gracza.
    /// </summary>
    private void OnEnable()
    {
        player_input.FunctionalPlayerInput.Enable();  // Wlacza wszystkie akcje wejsciowe

        // Subskrypcja do poszczegolnych akcji (np. otwieranie ekwipunku)
        player_input.FunctionalPlayerInput.OpenInventory.performed += OpenInventoryPerformed;
        player_input.FunctionalPlayerInput.OpenAchievements.performed += OpenAchievementsPerformed;
        player_input.FunctionalPlayerInput.OpenQuestLog.performed += OpenQuestLogPerformed;
        player_input.FunctionalPlayerInput.Pause.performed += PausePerformed;
        player_input.FunctionalPlayerInput.ThrowCoin.started += ThrowCoin;
        player_input.FunctionalPlayerInput.UseCigaretts.started += UseCigartetts;
        player_input.FunctionalPlayerInput.FlushAnalytics.started += FlushAnalytics;

        // Nasluchiwanie zdarzen otwierania i zamykania UI
        GlobalEvents.OnAnyUIOpen += DisableFunctionalPlayerInput;
        GlobalEvents.OnAnyUIClose += EnableFunctionalPlayerInput;
    }

    /// <summary>
    /// Dezaktywuje wszystkie akcje przypisane do gracza.
    /// </summary>
    private void OnDisable()
    {
        player_input.FunctionalPlayerInput.Disable();  // Wylacza wszystkie akcje wejsciowe

        // Anulowanie subskrypcji do akcji
        player_input.FunctionalPlayerInput.OpenInventory.performed -= OpenInventoryPerformed;
        player_input.FunctionalPlayerInput.OpenAchievements.performed -= OpenAchievementsPerformed;
        player_input.FunctionalPlayerInput.OpenQuestLog.performed -= OpenQuestLogPerformed;
        player_input.FunctionalPlayerInput.Pause.performed -= PausePerformed;
        player_input.FunctionalPlayerInput.ThrowCoin.started -= ThrowCoin;
        player_input.FunctionalPlayerInput.UseCigaretts.started -= UseCigartetts;
        player_input.FunctionalPlayerInput.FlushAnalytics.started -= FlushAnalytics;

        // Anulowanie nasluchiwania na zdarzenia UI
        GlobalEvents.OnAnyUIOpen -= DisableFunctionalPlayerInput;
        GlobalEvents.OnAnyUIClose -= EnableFunctionalPlayerInput;
    }

    /// <summary>
    /// Wylacza mozliwosc wprowadzania danych przez gracza, gdy UI jest otwarte.
    /// </summary>
    private void DisableFunctionalPlayerInput(object sender, EventArgs e)
    {
        player_input.FunctionalPlayerInput.Disable();  // Wylacza akcje wejsciowe
    }

    /// <summary>
    /// Wlacza mozliwosc wprowadzania danych przez gracza, gdy UI jest zamkniete.
    /// </summary>
    private void EnableFunctionalPlayerInput(object sender, EventArgs e)
    {
        player_input.FunctionalPlayerInput.Enable();  // Wlacza akcje wejsciowe
    }

    /// <summary>
    /// Akcja dla przycisku pauzy, wywoluje menu pauzy.
    /// </summary>
    private void PausePerformed(InputAction.CallbackContext obj)
    {
        pause_menu_UI.gameObject.SetActive(true);  // Aktywuje menu pauzy
        GlobalEvents.FireOnPauseGame(this);  // Wysyla zdarzenie pauzy
        GlobalEvents.FireOnAnyUIOpen(this);  // Wysyla zdarzenie otwarcia UI
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
    /// Akcja dla przycisku otwierania osiagniec.
    /// </summary>
    private void OpenAchievementsPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.OpenUI(PlayerAssetsUI.UIs.Achievements);  // Otwiera UI osiagniec
    }

    /// <summary>
    /// Ustawia PlayerInput dla tego kontrolera.
    /// </summary>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;  // Ustawia komponent PlayerInput
    }

    /// <summary>
    /// Akcja dla rzucenia moneta.
    /// </summary>
    public void ThrowCoin(InputAction.CallbackContext ctx)
    {
        GlobalEvents.FireOnThrowingCoin(this);  // Wysyla zdarzenie rzucenia moneta
    }

    /// <summary>
    /// Akcja dla palenia papierosow.
    /// </summary>
    public void UseCigartetts(InputAction.CallbackContext ctx)
    {
        GlobalEvents.FireOnUseCigs(this);  // Wysyla zdarzenie uzycia papierosow
    }

    public void FlushAnalytics(InputAction.CallbackContext ctx)
    {
        GameAnalytics.Instance.ClearStatsOnFreshSave(true);
    }
}
