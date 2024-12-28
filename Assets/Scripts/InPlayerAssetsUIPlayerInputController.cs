using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InPlayerAssetsUIPlayerInputController : MonoBehaviour
{
    private PlayerInput player_input;

    [SerializeField] private PlayerAssetsUI player_assets_UI;

    private void OnEnable()
    {
        player_input.InPlayerAssetsUIPlayerInput.Enable();

        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed += CloseWindowPerformed;

        GlobalEvents.OnPauseGame += DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame += EnableInPlayerAssetsUIPlayerInput;
    }

    private void OnDisable()
    {
        player_input.InPlayerAssetsUIPlayerInput.Disable();

        player_input.InPlayerAssetsUIPlayerInput.CloseWindow.performed -= CloseWindowPerformed;

        GlobalEvents.OnPauseGame -= DisableInPlayerAssetsUIPlayerInput;
        GlobalEvents.OnResumeGame -= EnableInPlayerAssetsUIPlayerInput;
    }

    private void EnableInPlayerAssetsUIPlayerInput(object sender, System.EventArgs e)
    {
        player_input.InPlayerAssetsUIPlayerInput.Enable();
    }

    private void DisableInPlayerAssetsUIPlayerInput(object sender, System.EventArgs e)
    {
        player_input.InPlayerAssetsUIPlayerInput.Disable();
    }

    private void CloseWindowPerformed(InputAction.CallbackContext obj)
    {
        player_assets_UI.CloseUI();
        GlobalEvents.FireOnAnyUIClose(this);        
    }
    
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
