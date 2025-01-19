using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InPlayerAssetsUIPlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Przechowuje obiekt player_input, który odpowiada za wejœcia od gracza.
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Przypisany obiekt UI gracza (do zarz¹dzania interfejsem).
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;


    /// <summary>
    /// Aktywuje wejœcia i subskrybuje odpowiednie zdarzenia.
    /// </summary>
    private void OnEnable()
    {
        // W³¹cza wejœcia zwi¹zane z InPlayerAssetsUI
        player_input.InPlayerAssetsUIPlayerInput.Enable();

        // Subskrybuje zdarzenie zamkniêcia okna
        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed += CloseWindowPerformed;

        // Subskrybuje zdarzenia pauzy i wznowienia, by wstrzymaæ/ponownie aktywowaæ wejœcia
        GlobalEvents.OnPauseGame += DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame += EnableInPlayerAssetsUIPlayerInput;
    }

    /// <summary>
    /// Dezaktywuje wejœcia i anuluje subskrypcjê zdarzeñ.
    /// </summary>
    private void OnDisable()
    {
        // Dezaktywuje wejœcia InPlayerAssetsUI
        player_input.InPlayerAssetsUIPlayerInput.Disable();

        // Anuluje subskrypcjê zdarzenia zamkniêcia okna
        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed -= CloseWindowPerformed;

        // Anuluje subskrypcjê zdarzeñ pauzy i wznowienia
        GlobalEvents.OnPauseGame -= DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame -= EnableInPlayerAssetsUIPlayerInput;
    }

    /// <summary>
    /// Aktywuje wejœcia dla gracza, jeœli gra zosta³a wznowiona.
    /// </summary>
    private void EnableInPlayerAssetsUIPlayerInput(object sender, System.EventArgs e)
    {
        player_input.InPlayerAssetsUIPlayerInput.Enable();
    }

    /// <summary>
    /// Dezaktywuje wejœcia dla gracza, jeœli gra jest pauzowana.
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
        GlobalEvents.FireOnAnyUIClose(this);  // Wysy³a powiadomienie, ¿e UI zosta³o zamkniête
    }

    /// <summary>
    /// Ustawia obiekt player_input, aby umo¿liwiæ przypisanie wejœæ.
    /// </summary>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
