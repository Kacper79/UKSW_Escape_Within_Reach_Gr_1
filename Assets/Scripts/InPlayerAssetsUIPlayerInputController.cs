using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InPlayerAssetsUIPlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Przechowuje obiekt player_input, kt�ry odpowiada za wej�cia od gracza.
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Przypisany obiekt UI gracza (do zarz�dzania interfejsem).
    /// </summary>
    [SerializeField] private PlayerAssetsUI player_assets_UI;


    /// <summary>
    /// Aktywuje wej�cia i subskrybuje odpowiednie zdarzenia.
    /// </summary>
    private void OnEnable()
    {
        // W��cza wej�cia zwi�zane z InPlayerAssetsUI
        player_input.InPlayerAssetsUIPlayerInput.Enable();

        // Subskrybuje zdarzenie zamkni�cia okna
        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed += CloseWindowPerformed;

        // Subskrybuje zdarzenia pauzy i wznowienia, by wstrzyma�/ponownie aktywowa� wej�cia
        GlobalEvents.OnPauseGame += DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame += EnableInPlayerAssetsUIPlayerInput;
    }

    /// <summary>
    /// Dezaktywuje wej�cia i anuluje subskrypcj� zdarze�.
    /// </summary>
    private void OnDisable()
    {
        // Dezaktywuje wej�cia InPlayerAssetsUI
        player_input.InPlayerAssetsUIPlayerInput.Disable();

        // Anuluje subskrypcj� zdarzenia zamkni�cia okna
        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed -= CloseWindowPerformed;

        // Anuluje subskrypcj� zdarze� pauzy i wznowienia
        GlobalEvents.OnPauseGame -= DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame -= EnableInPlayerAssetsUIPlayerInput;
    }

    /// <summary>
    /// Aktywuje wej�cia dla gracza, je�li gra zosta�a wznowiona.
    /// </summary>
    private void EnableInPlayerAssetsUIPlayerInput(object sender, System.EventArgs e)
    {
        player_input.InPlayerAssetsUIPlayerInput.Enable();
    }

    /// <summary>
    /// Dezaktywuje wej�cia dla gracza, je�li gra jest pauzowana.
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
        GlobalEvents.FireOnAnyUIClose(this);  // Wysy�a powiadomienie, �e UI zosta�o zamkni�te
    }

    /// <summary>
    /// Ustawia obiekt player_input, aby umo�liwi� przypisanie wej��.
    /// </summary>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
