using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Klasa odpowiedzialna za movement gracza
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
    /// <summary>
    /// Maksymalny k¹t rotacji kamery w osi X.
    /// </summary>
    private const float MAX_X_CAMERA_ROTATION = 89.0f;

    /// <summary>
    /// Minimalny k¹t rotacji kamery w osi X.
    /// </summary>
    private const float MIN_X_CAMERA_ROTATION = -89.0f;

    /// <summary>
    /// Komponent CharacterController gracza.
    /// </summary>
    [SerializeField] private CharacterController player_character_controller;

    /// <summary>
    /// Obiekt trzymaj¹cy kamerê.
    /// </summary>
    [SerializeField] private GameObject camera_holder_go;

    /// <summary>
    /// Obiekt gracza w grze.
    /// </summary>
    [SerializeField] private GameObject player_go;

    /// <summary>
    /// Obiekt wykrywaj¹cy cele interakcji.
    /// </summary>
    [SerializeField] private GameObject interactable_targets_detector;

    /// <summary>
    /// Lista transformacji, które s³u¿¹ do detekcji wspinania siê.
    /// </summary>
    [SerializeField] private List<Transform> did_climb_raycast_transforms;

    /// <summary>
    /// Prêdkoœci ruchu gracza w ró¿nych trybach (normalny, sprint, kucanie).
    /// </summary>
    [Header("Movement related variables")]
    [SerializeField] private float move_speed;
    [SerializeField] private float sprint_speed_multiplier;
    [SerializeField] private float crouch_speed_multiplier;
    [SerializeField] private float gravity;
    [SerializeField] private float jump_force;
    [SerializeField] private float climb_speed;

    /// <summary>
    /// Obiekt wejœcia gracza (PlayerInput).
    /// </summary>
    private PlayerInput player_input;

    /// <summary>
    /// Flaga wskazuj¹ca, czy gracz biega.
    /// </summary>
    private bool is_sprinting = false;

    /// <summary>
    /// Flaga wskazuj¹ca, czy gracz siê kuca.
    /// </summary>
    private bool is_crouching = false;

    /// <summary>
    /// Flaga wskazuj¹ca, czy gracz skoczy³ podczas sprintu.
    /// </summary>
    private bool did_jump_while_sprinted = false;

    /// <summary>
    /// Flaga pozwalaj¹ca na poruszanie siê gracza.
    /// </summary>
    private bool can_move = true;

    /// <summary>
    /// K¹t rotacji kamery w osi X.
    /// </summary>
    private float camera_x_rotation = 0.0f;

    /// <summary>
    /// Prêdkoœæ spadania gracza.
    /// </summary>
    private float y_velocity = 0.0f;

    /// <summary>
    /// Warstwa wykorzystywana do wykrywania obiektów wspinaczkowych.
    /// </summary>
    private int climbable_layer;

    // Deklaracja zmiennych do wykorzystywania w metodzie Update
    private Vector2 move_dir_normalized;
    private Vector3 move_dir;
    private Vector2 camera_rotation;
    private float mouse_x;
    private float mouse_y;

    /// <summary>
    /// Inicjalizacja komponentów gracza i warstwy wspinaczkowej.
    /// </summary>
    private void Awake()
    {
        climbable_layer = LayerMask.GetMask("Climbable");
    }

    /// <summary>
    /// Obs³uguje aktualizacjê ruchu, grawitacji oraz rotacji kamery.
    /// </summary>
    private void Update()
    {
        if (can_move)
        {
            HandleMovement();
            HandleGravity();
            HandleCameraRotation();
        }
    }

    /// <summary>
    /// Subskrybuje zdarzenia zwi¹zane z wejœciem gracza.
    /// </summary>
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        player_input.MovementPlayerInput.Enable();

        player_input.MovementPlayerInput.Jump.performed += JumpPerformed;
        player_input.MovementPlayerInput.Sprint.started += SprintStarted;
        player_input.MovementPlayerInput.Sprint.canceled += SprintCanceled;
        player_input.MovementPlayerInput.Crouch.started += CrouchStarted;
        player_input.MovementPlayerInput.Crouch.canceled += CrouchCanceled;

        GlobalEvents.OnAnyUIOpen += DisableMovementPlayerInput;
        GlobalEvents.OnAnyUIClose += EnableMovementPlayerInput;

        GlobalEvents.OnAnyUIOpen += UnlockCursor;
        GlobalEvents.OnAnyUIClose += LockCursor;

        GlobalEvents.OnStartingBlackJackGameForMoney += SetcameraRotationOnStartingBlackjackGame;
        GlobalEvents.OnStartingBlackJackGameForPickaxe += SetcameraRotationOnStartingBlackjackGame;
        GlobalEvents.OnEndingBlackjackGame += SetCanMoveToTrue;
    }

    /// <summary>
    /// Odsubskrybowuje zdarzenia, gdy obiekt jest dezaktywowany.
    /// </summary>
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;

        player_input.MovementPlayerInput.Disable();

        player_input.MovementPlayerInput.Jump.performed -= JumpPerformed;
        player_input.MovementPlayerInput.Sprint.started -= SprintStarted;
        player_input.MovementPlayerInput.Sprint.canceled -= SprintCanceled;
        player_input.MovementPlayerInput.Crouch.started -= CrouchStarted;
        player_input.MovementPlayerInput.Crouch.canceled -= CrouchCanceled;

        GlobalEvents.OnAnyUIOpen -= DisableMovementPlayerInput;
        GlobalEvents.OnAnyUIClose -= EnableMovementPlayerInput;

        GlobalEvents.OnAnyUIOpen -= UnlockCursor;
        GlobalEvents.OnAnyUIClose -= LockCursor;

        GlobalEvents.OnStartingBlackJackGameForMoney -= SetcameraRotationOnStartingBlackjackGame;
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= SetcameraRotationOnStartingBlackjackGame;
        GlobalEvents.OnEndingBlackjackGame -= SetCanMoveToTrue;
    }

    /// <summary>
    /// Ustawia mo¿liwoœæ poruszania siê po zakoñczeniu gry w blackjacka.
    /// </summary>
    private void SetCanMoveToTrue(object sender, EventArgs e)
    {
        can_move = true;
    }

    /// <summary>
    /// Ustawia rotacjê kamery na pocz¹tek gry w blackjacka.
    /// </summary>
    private void SetcameraRotationOnStartingBlackjackGame(object sender, EventArgs e)
    {
        can_move = false;
        camera_holder_go.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
    }

    /// <summary>
    /// Wy³¹cza wejœcie gracza do momentu zamkniêcia UI.
    /// </summary>
    private void DisableMovementPlayerInput(object sender, EventArgs e)
    {
        player_input.MovementPlayerInput.Disable();
    }

    /// <summary>
    /// W³¹cza wejœcie gracza po zamkniêciu UI.
    /// </summary>
    private void EnableMovementPlayerInput(object sender, EventArgs e)
    {
        player_input.MovementPlayerInput.Enable();
    }

    /// <summary>
    /// Zamyka kursor, gdy UI jest otwarte.
    /// </summary>
    private void LockCursor(object sender, EventArgs e)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Otwiera kursor, gdy UI jest zamkniête.
    /// </summary>
    private void UnlockCursor(object sender, EventArgs e)
    {
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Obs³uguje rozpoczêcie kucania.
    /// </summary>
    private void CrouchStarted(InputAction.CallbackContext context)
    {
        is_crouching = true;
        camera_holder_go.transform.position = new(camera_holder_go.transform.position.x, camera_holder_go.transform.position.y - 1.2f, camera_holder_go.transform.position.z);
    }

    /// <summary>
    /// Obs³uguje zakoñczenie kucania.
    /// </summary>
    private void CrouchCanceled(InputAction.CallbackContext context)
    {
        is_crouching = false;
        camera_holder_go.transform.position = new(camera_holder_go.transform.position.x, camera_holder_go.transform.position.y + 1.2f, camera_holder_go.transform.position.z);
    }

    /// <summary>
    /// Obs³uguje rozpoczêcie skoku.
    /// </summary>
    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (player_character_controller.isGrounded && can_move)
        {
            y_velocity = jump_force;
            Debug.Log(y_velocity + " tried jumping");
        }

        did_jump_while_sprinted = is_sprinting;
    }

    /// <summary>
    /// Obs³uguje rozpoczêcie sprintu.
    /// </summary>
    private void SprintStarted(InputAction.CallbackContext context)
    {
        is_sprinting = true;
    }

    /// <summary>
    /// Obs³uguje zakoñczenie sprintu.
    /// </summary>
    private void SprintCanceled(InputAction.CallbackContext context)
    {
        is_sprinting = false;
    }

    /// <summary>
    /// Próbuje rozpocz¹æ wspinaczkê, jeœli gracz nacisn¹³ odpowiedni przycisk.
    /// </summary>
    public void TryClimbingUp()
    {
        bool is_pressing_w = Mathf.Abs(player_input.MovementPlayerInput.Move.ReadValue<Vector2>().y - 1.0f) < 0.1f;

        if (is_pressing_w)
        {
            StartCoroutine(Climb());
        }
    }

    /// <summary>
    /// Procedura wspinaczki, która wykonuje ruch w górê, dopóki gracz nie trafi na powierzchniê.
    /// </summary>
    private IEnumerator Climb()
    {
        can_move = false;

        Vector3 climb_dir;

        while (true)
        {
            climb_dir = Vector3.up;
            climb_dir.z = 1f;
            climb_dir *= climb_speed * Time.deltaTime;
            climb_dir = player_go.transform.TransformDirection(climb_dir);
            player_character_controller.Move(climb_dir);

            if (DidAnyClimbDetectorsFindLandableObject())
            {
                break;
            }

            yield return null;
        }

        y_velocity = 0.0f;
        can_move = true;
    }

    /// <summary>
    /// Sprawdza, czy którykolwiek detektor wspinaczki trafi³ na powierzchniê wspinaczkow¹.
    /// </summary>
    private bool DidAnyClimbDetectorsFindLandableObject()
    {
        foreach (Transform did_climb_detector in did_climb_raycast_transforms)
        {
            if (Physics.Raycast(did_climb_detector.position, Vector3.down, 2.0f, climbable_layer))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Obs³uguje ruch gracza na podstawie wciœniêtych klawiszy oraz stanu sprintu i kucania.
    /// </summary>
    private void HandleMovement()
    {
        move_dir_normalized = player_input.MovementPlayerInput.Move.ReadValue<Vector2>();
        move_dir = new(move_dir_normalized.x, 0.0f, move_dir_normalized.y);
        move_dir = move_speed * Time.deltaTime * move_dir;

        if (player_character_controller.isGrounded)
        {
            move_dir = !is_sprinting || is_crouching ? move_dir : move_dir * sprint_speed_multiplier;
            move_dir = !is_crouching ? move_dir : move_dir * crouch_speed_multiplier;
        }
        else if (did_jump_while_sprinted)
        {
            move_dir *= sprint_speed_multiplier;
        }

        move_dir = player_go.transform.TransformDirection(move_dir);

        player_character_controller.Move(move_dir);
    }

    /// <summary>
    /// Obs³uguje rotacjê kamery w zale¿noœci od ruchów myszki.
    /// </summary>
    private void HandleCameraRotation()
    {
        camera_rotation = player_input.MovementPlayerInput.LookAround.ReadValue<Vector2>();

        mouse_x = camera_rotation.x * Settings.GetSensitivity() * Time.deltaTime;
        mouse_y = camera_rotation.y * Settings.GetSensitivity() * Time.deltaTime;

        camera_x_rotation -= mouse_y;
        camera_x_rotation = Mathf.Clamp(camera_x_rotation, MIN_X_CAMERA_ROTATION, MAX_X_CAMERA_ROTATION);

        camera_holder_go.transform.localRotation = Quaternion.Euler(camera_x_rotation, 0f, 0f);
        interactable_targets_detector.transform.localRotation = Quaternion.Euler(camera_x_rotation, 0f, 0f);

        player_go.transform.Rotate(Vector3.up * mouse_x);
    }

    /// <summary>
    /// Obs³uguje grawitacjê, przyci¹gaj¹c gracza w dó³.
    /// </summary>
    private void HandleGravity()
    {
        player_character_controller.Move(new(0.000001f, y_velocity, 0.0f));
        //Debug.Log(y_velocity);
        if (player_character_controller.isGrounded && y_velocity < 0.0f)
        {
            y_velocity = -0.01f;
            did_jump_while_sprinted = false;
        }
        else
        {
            y_velocity += Time.deltaTime * Time.deltaTime * gravity;
        }
    }

    /// <summary>
    /// Ustawia obiekt wejœcia gracza.
    /// </summary>
    /// <param name="input">Obiekt wejœcia gracza (PlayerInput).</param>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
