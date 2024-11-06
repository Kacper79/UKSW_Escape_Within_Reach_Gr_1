using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAttackingController : MonoBehaviour
{
    private const float MIN_TIME_BETWEEN_PUNCHES = 0.25f;
    private const float LIGHT_PUNCH_CAST_TIME = 0.166f;
    private const float STRONG_PUNCH_CAST_TIME = 0.166f;
    private const int LIGHT_PUNCH_DAMAGE = 7;
    private const int STRONG_PUNCH_DAMAGE = 14;

    private const float DASH_TIME = 0.1f;
    private const float DASH_COOLDOWN = 0.1f;

    //[SerializeField] private PlayerAnimationController player_animation_controller;//for later animations
    [SerializeField] private PunchableTargetsDetector punchable_targets_detector;
    [SerializeField] private GameObject player_go;
    [SerializeField] private CharacterController player_character_controller;

    [SerializeField] private float dash_speed;

    private PlayerInput player_input;

    private bool is_blocking = false;
    private bool can_punch = true;
    private bool can_dash = true;

    private float time_since_punched = 0.26f;

    private void Update()
    {
        if(!can_punch)
        {
            time_since_punched += Time.deltaTime;
            
            if (time_since_punched > MIN_TIME_BETWEEN_PUNCHES)
            {
                can_punch = true;
            }
        }
    }

    private void OnEnable()
    {
        player_input.AttackPlayerInput.Enable();//Might wanna Enable MovementPlayerInput as well, should be enabled, but in case of bugs try player_input.MovementPlayerInput.Enable();

        player_input.AttackPlayerInput.LightPunch.performed += LightPunchPerformed;
        player_input.AttackPlayerInput.StrongPunch.performed += StrongPunchPerformed;
        player_input.AttackPlayerInput.Block.started += BlockStarted;
        player_input.AttackPlayerInput.Block.canceled += BlockCanceled;
        player_input.AttackPlayerInput.LightPunch.started += LightPunchStarted;
        player_input.AttackPlayerInput.Dash.started += DashStarted;
    }

    private void OnDisable()
    {
        player_input.AttackPlayerInput.Disable();

        player_input.AttackPlayerInput.LightPunch.performed -= LightPunchPerformed;
        player_input.AttackPlayerInput.StrongPunch.performed -= StrongPunchPerformed;
        player_input.AttackPlayerInput.Block.started -= BlockStarted;
        player_input.AttackPlayerInput.Block.canceled -= BlockCanceled;
        player_input.AttackPlayerInput.LightPunch.started -= LightPunchStarted;
        player_input.AttackPlayerInput.Dash.started -= DashStarted;
    }

    private void LightPunchStarted(InputAction.CallbackContext context)//started only for light punch since it means strong punch started as well
    {
        if (can_punch && !is_blocking)
        {
            //player_animation_controller.AnimateCharge();
        }
    }

    private void LightPunchPerformed(InputAction.CallbackContext context)
    {
        if(!is_blocking && can_punch/* && player_animation_controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("ChargingThePunch")*/) 
        {
            can_punch = false;
            time_since_punched = 0.0f;
            //player_animation_controller.AnimateLightPunch();
            Debug.Log("Light punch!");
            StartCoroutine(TryHittingSomeone(LIGHT_PUNCH_DAMAGE, LIGHT_PUNCH_CAST_TIME));
        }
    }

    private void StrongPunchPerformed(InputAction.CallbackContext context)
    {
        if (!is_blocking && can_punch/* && player_animation_controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("ChargingThePunch")*/)
        {
            can_punch = false;
            time_since_punched = 0.0f;
            //player_animation_controller.AnimateStrongPunch();
            Debug.Log("Strong punch!");
            StartCoroutine(TryHittingSomeone(STRONG_PUNCH_DAMAGE, STRONG_PUNCH_CAST_TIME));
        }
    }

    private void DashStarted(InputAction.CallbackContext context)
    {
        if(can_dash && IsAbleToDash() && (Vector2.Distance(player_input.MovementPlayerInput.Move.ReadValue<Vector2>(), Vector2.zero) >= 0.1))
        {
            can_dash = false;

            Vector2 move_dir_normalized = player_input.MovementPlayerInput.Move.ReadValue<Vector2>();
            Vector3 move_dir = new(move_dir_normalized.x, 0.0f, move_dir_normalized.y);
            move_dir = dash_speed * Time.fixedDeltaTime * move_dir;
            move_dir = player_go.transform.TransformDirection(move_dir);

            StartCoroutine(Dash(move_dir));
        }
    }

    private void BlockStarted(InputAction.CallbackContext context)
    {
        is_blocking = true;
        //player_animation_controller.AnimateBlock();
    }

    private void BlockCanceled(InputAction.CallbackContext context)
    {
        is_blocking = false;
        //player_animation_controller.StopBlocking();
    }

    private IEnumerator TryHittingSomeone(int damage, float time)
    {
        yield return new WaitForSeconds(time);

        punchable_targets_detector.TryDamagingEnemies(damage);
    }

    private bool IsAbleToDash()
    {
        return true;//Stamina or sth in the future
    }

    private IEnumerator Dash(Vector3 move_dir)
    {
        Vector3 start_position = player_go.transform.position;
        Vector3 target_position = start_position + move_dir;
        float start_time = Time.time;
        float t;

        while (Time.time - start_time < DASH_TIME)
        {
            t = (Time.time - start_time) / DASH_TIME;
            Vector3 newPosition = Vector3.Lerp(start_position, target_position, t);

            player_character_controller.Move(newPosition - player_character_controller.transform.position);

            yield return null;
        }

        player_character_controller.Move(target_position - player_character_controller.transform.position);

        yield return new WaitForSeconds(DASH_COOLDOWN);

        can_dash = true;
    }

    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
