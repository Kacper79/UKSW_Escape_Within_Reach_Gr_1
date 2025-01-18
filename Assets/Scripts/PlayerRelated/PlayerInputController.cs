using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie wej�ciem gracza i powi�zanie komponent�w wej�cia z r�nymi kontrolerami gracza.
/// Obs�uguje w��czanie i wy��czanie kontroler�w w zale�no�ci od sytuacji w grze, np. w czasie dialog�w czy podczas czytania strony.
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Obiekt przechowuj�cy dane wej�cia gracza (PlayerInput).
    /// </summary>
    [SerializeField] private PlayerInput player_input;

    /// <summary>
    /// Inicjalizuje obiekt, ustawia odpowiednie referencje do kontroler�w gracza i zarz�dza przypisaniem komponentu wej�cia.
    /// </summary>
    private void Awake()
    {
        if (GameObject.Find("RebindGO") == null)
        {
            GameObject reb = new("RebindGO");
            reb.AddComponent<RebindManager>();
        }
        player_input = GameObject.Find("RebindGO").GetComponent<RebindManager>().player_input;

        // Ustawienie wej�cia dla r�nych kontroler�w gracza
        GetComponentInChildren<PlayerInteractionController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerMovementController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerAttackingController>().SetPlayerInput(player_input);
        GetComponentInChildren<FunctionalPlayerInputController>().SetPlayerInput(player_input);
        GetComponentInChildren<InPlayerAssetsUIPlayerInputController>().SetPlayerInput(player_input);
    }

    /// <summary>
    /// Subskrybuje zdarzenia globalne do w��czania i wy��czania kontroler�w podczas r�nych faz gry.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnStartingDialogue += DisableAllControllers;
        GlobalEvents.OnEndingDialogue += EnableAllControllers;

        GlobalEvents.OnReadingPage += DisableAllControllers;
        GlobalEvents.OnStoppingReadingPage += EnableAllControllers;
    }

    /// <summary>
    /// Odsubskrybowuje zdarzenia globalne przy dezaktywacji obiektu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingDialogue -= DisableAllControllers;
        GlobalEvents.OnEndingDialogue -= EnableAllControllers;

        GlobalEvents.OnReadingPage -= DisableAllControllers;
        GlobalEvents.OnStoppingReadingPage -= EnableAllControllers;
    }

    /// <summary>
    /// W��cza wszystkie kontrolery gracza.
    /// </summary>
    private void EnableAllControllers(object sender, System.EventArgs e)
    {
        EnableAllControllers();
    }

    /// <summary>
    /// Wy��cza wszystkie kontrolery gracza.
    /// </summary>
    private void DisableAllControllers(object sender, System.EventArgs e)
    {
        DisableAllControllers();
    }

    /// <summary>
    /// Wy��cza wszystkie kontrolery gracza, takie jak ruch, atak i interakcje.
    /// </summary>
    private void DisableAllControllers()
    {
        DisableMovementController();
        DisableAttackingController();
        DisableInteractionController();
    }

    /// <summary>
    /// W��cza wszystkie kontrolery gracza, takie jak ruch, atak i interakcje.
    /// </summary>
    private void EnableAllControllers()
    {
        EnableMovementController();
        EnableAttackingController();
        EnableInteractionController();
    }

    /// <summary>
    /// Wy��cza kontroler ruchu.
    /// </summary>
    private void DisableMovementController()
    {
        player_input.MovementPlayerInput.Disable();
    }

    /// <summary>
    /// W��cza kontroler ruchu.
    /// </summary>
    private void EnableMovementController()
    {
        player_input.MovementPlayerInput.Enable();
    }

    /// <summary>
    /// Wy��cza kontroler ataku.
    /// </summary>
    private void DisableAttackingController()
    {
        player_input.AttackPlayerInput.Disable();
    }

    /// <summary>
    /// W��cza kontroler ataku.
    /// </summary>
    private void EnableAttackingController()
    {
        player_input.AttackPlayerInput.Enable();
    }

    /// <summary>
    /// Wy��cza kontroler interakcji.
    /// </summary>
    private void DisableInteractionController()
    {
        player_input.InteractPlayerInput.Disable();
    }

    /// <summary>
    /// W��cza kontroler interakcji.
    /// </summary>
    private void EnableInteractionController()
    {
        player_input.InteractPlayerInput.Enable();
    }
}
