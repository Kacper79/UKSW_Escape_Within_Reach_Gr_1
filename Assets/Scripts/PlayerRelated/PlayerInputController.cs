using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie wejsciem gracza i powiazanie komponentow wejscia z roznymi kontrolerami gracza.
/// Obsluguje wlaczanie i wylaczanie kontrolerow w zaleznosci od sytuacji w grze, np. w czasie dialogow czy podczas czytania strony.
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// Obiekt przechowujacy dane wejscia gracza (PlayerInput).
    /// </summary>
    [SerializeField] private PlayerInput player_input;

    /// <summary>
    /// Inicjalizuje obiekt, ustawia odpowiednie referencje do kontrolerow gracza i zarzadza przypisaniem komponentu wejscia.
    /// </summary>
    private void Awake()
    {
        if (GameObject.Find("RebindGO") == null)
        {
            GameObject reb = new("RebindGO");
            reb.AddComponent<RebindManager>();
        }
        player_input = GameObject.Find("RebindGO").GetComponent<RebindManager>().player_input;

        // Ustawienie wejscia dla roznych kontrolerow gracza
        GetComponentInChildren<PlayerInteractionController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerMovementController>().SetPlayerInput(player_input);
        GetComponentInChildren<PlayerAttackingController>().SetPlayerInput(player_input);
        GetComponentInChildren<FunctionalPlayerInputController>().SetPlayerInput(player_input);
        GetComponentInChildren<InPlayerAssetsUIPlayerInputController>().SetPlayerInput(player_input);
        GetComponentInChildren<QTEManager>().SetPlayerInput(player_input);

    }
    /// <summary>
    /// Subskrybuje zdarzenia globalne do wlaczania i wylaczania kontrolerow podczas roznych faz gry.
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
    /// Wlacza wszystkie kontrolery gracza.
    /// </summary>
    private void EnableAllControllers(object sender, System.EventArgs e)
    {
        EnableAllControllers();
    }

    /// <summary>
    /// Wylacza wszystkie kontrolery gracza.
    /// </summary>
    private void DisableAllControllers(object sender, System.EventArgs e)
    {
        DisableAllControllers();
    }

    /// <summary>
    /// Wylacza wszystkie kontrolery gracza, takie jak ruch, atak i interakcje.
    /// </summary>
    private void DisableAllControllers()
    {
        DisableMovementController();
        DisableAttackingController();
        DisableInteractionController();
    }

    /// <summary>
    /// Wlacza wszystkie kontrolery gracza, takie jak ruch, atak i interakcje.
    /// </summary>
    private void EnableAllControllers()
    {
        EnableMovementController();
        EnableAttackingController();
        EnableInteractionController();
    }

    /// <summary>
    /// Wylacza kontroler ruchu.
    /// </summary>
    private void DisableMovementController()
    {
        player_input.MovementPlayerInput.Disable();
    }

    /// <summary>
    /// Wlacza kontroler ruchu.
    /// </summary>
    private void EnableMovementController()
    {
        player_input.MovementPlayerInput.Enable();
    }

    /// <summary>
    /// Wylacza kontroler ataku.
    /// </summary>
    private void DisableAttackingController()
    {
        player_input.AttackPlayerInput.Disable();
    }

    /// <summary>
    /// Wlacza kontroler ataku.
    /// </summary>
    private void EnableAttackingController()
    {
        player_input.AttackPlayerInput.Enable();
    }

    /// <summary>
    /// Wylacza kontroler interakcji.
    /// </summary>
    private void DisableInteractionController()
    {
        player_input.InteractPlayerInput.Disable();
    }

    /// <summary>
    /// Wlacza kontroler interakcji.
    /// </summary>
    private void EnableInteractionController()
    {
        player_input.InteractPlayerInput.Enable();
    }
}
