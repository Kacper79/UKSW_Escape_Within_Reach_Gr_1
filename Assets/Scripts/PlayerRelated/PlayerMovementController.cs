using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private const float MAX_X_CAMERA_ROTATION = 89.0f;
    private const float MIN_X_CAMERA_ROTATION = -89.0f;

    [SerializeField] private CharacterController player_character_controller;
    [SerializeField] private GameObject camera_holder_go;
    //[SerializeField] private GameObject right_arm_go;
    [SerializeField] private GameObject player_go;

    [SerializeField] private List<Transform> did_climb_raycast_transforms;
    [Header("Movement related variables")]
    [SerializeField] private float move_speed;
    [SerializeField] private float sprint_speed_multiplier;
    [SerializeField] private float crouch_speed_multiplier;
    [SerializeField] private float mouse_sensitivity;
    [SerializeField] private float gravity;
    [SerializeField] private float jump_force;
    [SerializeField] private float climb_speed;

    private PlayerInput player_input;

    private bool is_sprinting = false;
    private bool is_crouching = false;
    private bool did_jump_while_sprinted = false;

    private bool can_move = true;

    private float camera_x_rotation = 0.0f;

    private float y_velocity = 0.0f;

    private int climbable_layer;

    //Declaring some variables here so there won't be a need to declare them over and over again in Update
    private Vector2 move_dir_normalized;
    private Vector3 move_dir;
    private Vector2 camera_rotation;
    private float mouse_x;
    private float mouse_y;

    private void Awake()
    {
        climbable_layer = LayerMask.GetMask("Climbable");
    }

    private void Update()
    {
        if (can_move)
        {
            HandleMovement();
            HandleGravity();
            HandleCameraRotation();
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        player_input.MovementPlayerInput.Enable();

        player_input.MovementPlayerInput.Jump.performed += JumpPerformed;
        player_input.MovementPlayerInput.Sprint.started += SprintStarted;
        player_input.MovementPlayerInput.Sprint.canceled += SprintCanceled;
        player_input.MovementPlayerInput.Crouch.started += CrouchStarted;
        player_input.MovementPlayerInput.Crouch.canceled += CrouchCanceled;

        GlobalEvents.OnReadingPage += OnReadingPage;
        GlobalEvents.OnStoppingReadingPage += OnStoppingReadingPage;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;

        player_input.MovementPlayerInput.Disable();

        player_input.MovementPlayerInput.Jump.performed -= JumpPerformed;
        player_input.MovementPlayerInput.Sprint.started -= SprintStarted;
        player_input.MovementPlayerInput.Sprint.canceled -= SprintCanceled;
        player_input.MovementPlayerInput.Crouch.started -= CrouchStarted;
        player_input.MovementPlayerInput.Crouch.canceled -= CrouchCanceled;

        GlobalEvents.OnReadingPage -= OnReadingPage;
        GlobalEvents.OnStoppingReadingPage -= OnStoppingReadingPage;
    }

    private void OnStoppingReadingPage(object sender, EventArgs e)
    {
        can_move = true;
    }

    private void OnReadingPage(object sender, EventArgs e)
    {
        can_move = false;
    }

    private void CrouchStarted(InputAction.CallbackContext context)
    {
        is_crouching = true;
        camera_holder_go.transform.position = new(camera_holder_go.transform.position.x, camera_holder_go.transform.position.y - 1.2f, camera_holder_go.transform.position.z);
    }

    private void CrouchCanceled(InputAction.CallbackContext context)
    {
        is_crouching = false;
        camera_holder_go.transform.position = new(camera_holder_go.transform.position.x, camera_holder_go.transform.position.y + 1.2f, camera_holder_go.transform.position.z);
    }

    private void JumpPerformed(InputAction.CallbackContext context)// TODO: changing directions midair
    {
        if(player_character_controller.isGrounded)// TODO: if pressed space during reading a page player auto jumped
        {
            y_velocity = jump_force;
        }

        did_jump_while_sprinted = is_sprinting;
    }

    private void SprintStarted(InputAction.CallbackContext context)
    {
        is_sprinting = true;
    }

    private void SprintCanceled(InputAction.CallbackContext context)
    {
        is_sprinting = false;
    }

    public void TryClimbingUp()
    {
        bool is_pressing_w = Mathf.Abs(player_input.MovementPlayerInput.Move.ReadValue<Vector2>().y - 1.0f) < 0.1f;
        
        if (is_pressing_w)
        {
            StartCoroutine(Climb());
        }
    }

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

            if(DidAnyClimbDetectorsFindLandableObject())
            {
                break;
            }
            
            yield return null;
        }
        
        y_velocity = 0.0f;
        can_move = true;
    }

    private bool DidAnyClimbDetectorsFindLandableObject()
    {
        foreach (Transform did_climb_detector in did_climb_raycast_transforms)
        {
            if(Physics.Raycast(did_climb_detector.position, Vector3.down, 2.0f, climbable_layer))
            {
                return true;
            }
        }

        return false;
    }

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
        else if(did_jump_while_sprinted)
        {
            move_dir *= sprint_speed_multiplier;
        }

        move_dir = player_go.transform.TransformDirection(move_dir);
        
        player_character_controller.Move(move_dir);
    }

    private void HandleCameraRotation()// TOFO: Interaction detector should move up and down as well
    {
        camera_rotation = player_input.MovementPlayerInput.LookAround.ReadValue<Vector2>();
        
        mouse_x = camera_rotation.x * mouse_sensitivity * Time.deltaTime;
        mouse_y = camera_rotation.y * mouse_sensitivity * Time.deltaTime;

        camera_x_rotation -= mouse_y;
        camera_x_rotation = Mathf.Clamp(camera_x_rotation, MIN_X_CAMERA_ROTATION, MAX_X_CAMERA_ROTATION);

        camera_holder_go.transform.localRotation = Quaternion.Euler(camera_x_rotation, 0f, 0f);
        //right_arm_go.transform.localRotation = Quaternion.Euler(camera_x_rotation, 0f, 0f);//prototype arm to see punches
        
        player_go.transform.Rotate(Vector3.up * mouse_x);
    }

    private void HandleGravity()
    {
        if (player_character_controller.isGrounded && y_velocity < 0.0f)
        {
            y_velocity = -0.01f;
            did_jump_while_sprinted = false;
        }
        else 
        { 
            y_velocity += Time.deltaTime * Time.deltaTime * gravity;
            player_character_controller.Move(new(0.0f, y_velocity, 0.0f));
        }
    }

    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
