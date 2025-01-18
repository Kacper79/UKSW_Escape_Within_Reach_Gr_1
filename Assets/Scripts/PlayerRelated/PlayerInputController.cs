using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz¹dzanie wejœciem gracza i powi¹zanie komponentów wejœcia z ró¿nymi kontrolerami gracza.
/// Obs³uguje w³¹czanie i wy³¹czanie kontrolerów w zale¿noœci od sytuacji w grze, np. w czasie dialogów czy podczas czytania strony.
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Obiekt przechowuj¹cy dane wejœcia gracza (PlayerInput).
    /// </summary>
    [SerializeField] private PlayerInput player_input;

    /// <summary>
    /// Inicjalizuje obiekt, ustawia odpowiednie referencje do kontrolerów gracza i zarz¹dza przypisaniem komponentu wejœcia.
    /// </summary>
    private void Awake()
    {
        if (GameObject.Find("RebindGO") == null)
        {
            GameObject reb = new("RebindGO");
            reb.AddComponent<RebindManager>();
        }
        player_input = GameObject.Find("RebindGO").GetComponent<RebindManager>().player_input;

        // Ustawienie wejœcia dla ró¿nych kontrolerów gracza
        GetComponentInChildren<PlayerInteractionController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerMovementController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerAttackingController>().SetPlayerInput(player_input);
        GetComponentInChildren<FunctionalPlayerInputController>().SetPlayerInput(player_input);
        GetComponentInChildren<InPlayerAssetsUIPlayerInputController>().SetPlayerInput(player_input);
    }

    /// <summary>
    /// Subskrybuje zdarzenia globalne do w³¹czania i wy³¹czania kontrolerów podczas ró¿nych faz gry.
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
    /// W³¹cza wszystkie kontrolery gracza.
    /// </summary>
    private void EnableAllControllers(object sender, System.EventArgs e)
    {
        EnableAllControllers();
    }

    /// <summary>
    /// Wy³¹cza wszystkie kontrolery gracza.
    /// </summary>
    private void DisableAllControllers(object sender, System.EventArgs e)
    {
        DisableAllControllers();
    }

    /// <summary>
    /// Wy³¹cza wszystkie kontrolery gracza, takie jak ruch, atak i interakcje.
    /// </summary>
    private void DisableAllControllers()
    {
        DisableMovementController();
        DisableAttackingController();
        DisableInteractionController();
    }

    /// <summary>
    /// W³¹cza wszystkie kontrolery gracza, takie jak ruch, atak i interakcje.
    /// </summary>
    private void EnableAllControllers()
    {
        EnableMovementController();
        EnableAttackingController();
        EnableInteractionController();
    }

    /// <summary>
    /// Wy³¹cza kontroler ruchu.
    /// </summary>
    private void DisableMovementController()
    {
        player_input.MovementPlayerInput.Disable();
    }

    /// <summary>
    /// W³¹cza kontroler ruchu.
    /// </summary>
    private void EnableMovementController()
    {
        player_input.MovementPlayerInput.Enable();
    }

    /// <summary>
    /// Wy³¹cza kontroler ataku.
    /// </summary>
    private void DisableAttackingController()
    {
        player_input.AttackPlayerInput.Disable();
    }

    /// <summary>
    /// W³¹cza kontroler ataku.
    /// </summary>
    private void EnableAttackingController()
    {
        player_input.AttackPlayerInput.Enable();
    }

    /// <summary>
    /// Wy³¹cza kontroler interakcji.
    /// </summary>
    private void DisableInteractionController()
    {
        player_input.InteractPlayerInput.Disable();
    }

    /// <summary>
    /// W³¹cza kontroler interakcji.
    /// </summary>
    private void EnableInteractionController()
    {
        player_input.InteractPlayerInput.Enable();
    }
}
