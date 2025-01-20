using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InPlayerAssetsUIPlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Przechowuje obiekt player_input, ktory odpowiada za wejscia od gracza.
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Przypisany obiekt UI gracza (do zarzadzania interfejsem).
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;


    /// <summary>
    /// Aktywuje wejscia i subskrybuje odpowiednie zdarzenia.
    /// </summary>
    private void OnEnable()
    {
        // Wlacza wejscia zwiazane z InPlayerAssetsUI
        player_input.InPlayerAssetsUIPlayerInput.Enable();

        // Subskrybuje zdarzenie zamkniecia okna
        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed += CloseWindowPerformed;

        // Subskrybuje zdarzenia pauzy i wznowienia, by wstrzymac/ponownie aktywowac wejscia
        GlobalEvents.OnPauseGame += DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame += EnableInPlayerAssetsUIPlayerInput;
    }

    /// <summary>
    /// Dezaktywuje wejscia i anuluje subskrypcje zdarzen.
    /// </summary>
    private void OnDisable()
    {
        // Dezaktywuje wejscia InPlayerAssetsUI
        player_input.InPlayerAssetsUIPlayerInput.Disable();

        // Anuluje subskrypcje zdarzenia zamkniecia okna
        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed -= CloseWindowPerformed;

        // Anuluje subskrypcje zdarzen pauzy i wznowienia
        GlobalEvents.OnPauseGame -= DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame -= EnableInPlayerAssetsUIPlayerInput;
    }

    /// <summary>
    /// Aktywuje wejscia dla gracza, jesli gra zostala wznowiona.
    /// </summary>
    private void EnableInPlayerAssetsUIPlayerInput(object sender, System.EventArgs e)
    {
        player_input.InPlayerAssetsUIPlayerInput.Enable();
    }

    /// <summary>
    /// Dezaktywuje wejscia dla gracza, jesli gra jest pauzowana.
    /// </summary>
    private void DisableInPlayerAssetsUIPlayerInput(object sender, System.EventArgs e)
    {
        player_input.InPlayerAssetsUIPlayerInput.Disable();
    }

    /// <summary>
    /// Zamyka UI, gdy wykonane jest odpowiednie polecenie.
    /// </summary>
    private void CloseWindowPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.CloseUI();  // Zamyka UI gracza
        GlobalEvents.FireOnAnyUIClose(this);  // Wysyla powiadomienie, ze UI zostalo zamkniete
    }

    /// <summary>
    /// Ustawia obiekt player_input, aby umozliwic przypisanie wejsc.
    /// </summary>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
