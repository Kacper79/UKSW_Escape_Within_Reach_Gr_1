using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie akcjami atakow, blokowania i dashowania gracza.
/// Obsluguje wirtualne ataki lekkie i silne, a takze mechanike dashowania oraz blokowania.
/// </summary>
public class PlayerAttackingController : MonoBehaviour
{
    private const float MIN_TIME_BETWEEN_PUNCHES = 0.4f; // Minimalny czas miedzy dwoma ciosami
    private const float LIGHT_PUNCH_CAST_TIME = 0.166f; // Czas trwania ciosu lekkiego
    private const float STRONG_PUNCH_CAST_TIME = 0.166f; // Czas trwania ciosu silnego
    private const int LIGHT_PUNCH_DAMAGE = 7; // Obrazenia za cios lekki
    private const int STRONG_PUNCH_DAMAGE = 14; // Obrazenia za cios silny

    private const float DASH_TIME = 0.1f; // Czas trwania dashowania
    private const float DASH_COOLDOWN = 0.1f; // Czas odnowienia dashowania

    //[SerializeField] private PlayerAnimationController player_animation_controller; // Animacje gracza (do pozniejszego uzycia)
    [SerializeField] private PunchableTargetsDetector punchable_targets_detector; // Detektor celow do uderzenia
    [SerializeField] private GameObject player_go; // Grajacy obiekt gracza
    [SerializeField] private CharacterController player_character_controller; // Kontroler postaci

    [SerializeField] private float dash_speed; // Predkosc dashowania

    private PlayerInput player_input;

    public bool is_blocking = false; // Czy gracz blokuje
    private bool can_punch = true; // Czy gracz moze wykonac cios
    private bool can_dash = true; // Czy gracz moze wykonac dash

    private float time_since_punched = 0.26f; // Czas od ostatniego ciosu

    /// <summary>
    /// Sprawdza czas od ostatniego ciosu i wznawia mozliwosc uderzenia po odpowiednim czasie.
    /// </summary>
    private void Update()
    {
        if (!can_punch)
        {
            time_since_punched += Time.deltaTime;

            if (time_since_punched > MIN_TIME_BETWEEN_PUNCHES)
            {
                can_punch = true;
            }
        }
    }

    /// <summary>
    /// Rejestruje akcje wejscia, umozliwiajac atakowanie i blokowanie.
    /// </summary>
    private void OnEnable()
    {
        player_input.AttackPlayerInput.Enable();

        player_input.AttackPlayerInput.LightPunch.performed += LightPunchPerformed;
        player_input.AttackPlayerInput.StrongPunch.performed += StrongPunchPerformed;
        player_input.AttackPlayerInput.Block.started += BlockStarted;
        player_input.AttackPlayerInput.Block.canceled += BlockCanceled;
        player_input.AttackPlayerInput.LightPunch.started += LightPunchStarted;
        player_input.AttackPlayerInput.Dash.started += DashStarted;
    }

    /// <summary>
    /// Odlacza zdarzenia wejsciowe przy dezaktywacji.
    /// </summary>
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

    /// <summary>
    /// Reakcja na rozpoczecie lekkiego ciosu. Moze wywolac animacje (na pozniej).
    /// </summary>
    private void LightPunchStarted(InputAction.CallbackContext context)
    {
        if (can_punch && !is_blocking)
        {
            //player_animation_controller.AnimateCharge();
        }
    }

    /// <summary>
    /// Wykonanie lekkiego ciosu. Uzywa korutyny do sprawdzenia trafienia celu.
    /// </summary>
    private void LightPunchPerformed(InputAction.CallbackContext context)
    {
        if (!is_blocking && can_punch)
        {
            can_punch = false;
            time_since_punched = 0.0f;
            //player_animation_controller.AnimateLightPunch();
            StartCoroutine(TryHittingSomeone(LIGHT_PUNCH_DAMAGE, LIGHT_PUNCH_CAST_TIME));
        }
    }

    /// <summary>
    /// Wykonanie silnego ciosu. Uzywa korutyny do sprawdzenia trafienia celu.
    /// </summary>
    private void StrongPunchPerformed(InputAction.CallbackContext context)
    {
        if (!is_blocking && can_punch)
        {
            can_punch = false;
            time_since_punched = 0.0f;
            //player_animation_controller.AnimateStrongPunch();
            Debug.Log("Strong punch!");
            StartCoroutine(TryHittingSomeone(STRONG_PUNCH_DAMAGE, STRONG_PUNCH_CAST_TIME));
        }
    }

    /// <summary>
    /// Rozpoczecie akcji dashowania. Gracz moze dashowac tylko, jesli spelnia odpowiednie warunki.
    /// </summary>
    private void DashStarted(InputAction.CallbackContext context)
    {
        if (can_dash && IsAbleToDash() && (Vector2.Distance(player_input.MovementPlayerInput.Move.ReadValue<Vector2>(), Vector2.zero) >= 0.1))
        {
            can_dash = false;

            Vector2 move_dir_normalized = player_input.MovementPlayerInput.Move.ReadValue<Vector2>();
            Vector3 move_dir = new(move_dir_normalized.x, 0.0f, move_dir_normalized.y);
            move_dir = dash_speed * Time.fixedDeltaTime * move_dir;
            move_dir = player_go.transform.TransformDirection(move_dir);

            StartCoroutine(Dash(move_dir));
        }
    }

    /// <summary>
    /// Rozpoczecie blokowania. Gracz zaczyna blokowanie.
    /// </summary>
    private void BlockStarted(InputAction.CallbackContext context)
    {
        is_blocking = true;
        //player_animation_controller.AnimateBlock();
    }

    /// <summary>
    /// Zakonczenie blokowania. Gracz przestaje blokowac.
    /// </summary>
    private void BlockCanceled(InputAction.CallbackContext context)
    {
        is_blocking = false;
        //player_animation_controller.StopBlocking();
    }

    /// <summary>
    /// Korutyna odpowiedzialna za sprawdzenie, czy gracz trafil w cel po wykonaniu ciosu.
    /// </summary>
    private IEnumerator TryHittingSomeone(int damage, float time)
    {
        yield return new WaitForSeconds(time);

        punchable_targets_detector.TryDamagingEnemies(damage);
    }

    /// <summary>
    /// Sprawdza, czy gracz moze wykonac dashowanie (np. czy posiada wystarczajaca ilosc staminy).
    /// </summary>
    private bool IsAbleToDash()
    {
        return true; // W przyszlosci moze zostac dodana mechanika wytrzymalosci.
    }

    /// <summary>
    /// Korutyna odpowiedzialna za wykonanie dashowania, przemieszczajac gracza w wybranym kierunku.
    /// </summary>
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

    /// <summary>
    /// Ustawia referencje do wejscia gracza (PlayerInput).
    /// </summary>
    public void SetPlayerInput(PlayerInput input)
    {
        player_input = input;
    }
}
