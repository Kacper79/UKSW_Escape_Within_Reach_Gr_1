using System.Collections;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class QTEManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText; // UI text to display prompt
    [SerializeField] private GameObject player_go;
    [SerializeField] private CharacterController player_character_controller; // Kontroler postaci
    [SerializeField] private Transform dash_destination_transform;
    private bool qte_active = false; // Is the QTE active?
    private float qtetimer;
    private PlayerInput player_input;
    private const float DASH_TIME = 0.3f; // Czas trwania dashowania
    private float qte_duration; // Time to complete QTE


    private void OnEnable()
    {
        player_input.QuickTimeEventPlayerInput.Enable();
        player_input.QuickTimeEventPlayerInput.QuickTimeEventAction.performed += OnQuickTimeAction;
    }

    private void OnDisable()
    {
        player_input.QuickTimeEventPlayerInput.Disable();
        player_input.QuickTimeEventPlayerInput.QuickTimeEventAction.performed -= OnQuickTimeAction;
    }

    private void Update()
    {
        if (qte_active)
        {
            qte_duration -= Time.deltaTime;
            if (qte_duration <= 0)
            {
                EndQTE(false);// Fail if time runs out
            }
        }
    }
    public void StartQTE(string prompt)
    {
        qte_duration = 1.5f;
        qte_active = true;
        promptText.text = prompt;
        promptText.gameObject.SetActive(true);
        player_input.MovementPlayerInput.Disable();

    }

    private void OnQuickTimeAction(InputAction.CallbackContext context)
    {
        if (!qte_active)
        {
            return;
        }

        EndQTE(true); // Success when input is performed
    }

    private void EndQTE(bool success)
    {
        qte_active = false;
        player_input.MovementPlayerInput.Enable();
        promptText.gameObject.SetActive(false);
        if (success)
        {
            StartCoroutine(Dash(dash_destination_transform.forward));
            Debug.Log("QTE success");
        }
        else
        {
            player_go.GetComponent<DeathScript>().TeleportToCell();
            Debug.Log("QTE failed");
        }
    }

    public void SetPlayerInput(PlayerInput player_input_)
    {
        player_input = player_input_;
    }

    private IEnumerator Dash(Vector3 move_dir)
    {
        Vector3 start_position = player_go.transform.position;
        Vector3 target_position = start_position + 10*move_dir;
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
    }
}
