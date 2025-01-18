using Assets.Scripts.AI;
using Assets.Scripts.PlayerRelated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa zarz¹dzaj¹ca walk¹ w turnieju. Obs³uguje rozpoczêcie turnieju, dodawanie wrogów,
/// zarz¹dzanie stanem turnieju oraz zakoñczenie turnieju po pokonaniu wszystkich wrogów.
/// </summary>
public class FightManager : MonoBehaviour
{
    /// <summary>
    /// Liczba wrogów, których trzeba pokonaæ, aby wygraæ turniej.
    /// </summary>
    private const int NUMBER_OF_ENEMY_TO_DEFEAT_TO_WIN_TOURNAMENT = 3;

    /// <summary>
    /// K¹t rotacji gracza po zakoñczeniu walki.
    /// </summary>
    private const float Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT = 170.0f;

    /// <summary>
    /// Collider pierœcienia, który aktywuje walkê.
    /// </summary>
    [SerializeField] private GameObject ring_collider;

    /// <summary>
    /// Prefab wroga, który zostanie zainstantiowany podczas walki.
    /// </summary>
    [SerializeField] private GameObject enemy_prefab;

    /// <summary>
    /// Miejsce, do którego zostanie przeniesiony gracz, gdy rozpocznie turniej.
    /// </summary>
    [SerializeField] private Transform place_to_tp_player_to_when_starting_tournament;

    /// <summary>
    /// Pocz¹tkowa pozycja wroga, w którym bêdzie siê znajdowa³ na pocz¹tku walki.
    /// </summary>
    [SerializeField] private Transform enemy_starting_position;

    /// <summary>
    /// Liczba pokonanych wrogów w trakcie trwania turnieju.
    /// </summary>
    private int defeated_enemy_in_tournament_so_far = 0;

    /// <summary>
    /// Obiekt gracza w grze.
    /// </summary>
    private GameObject player_go;

    /// <summary>
    /// Obiekt wroga w grze.
    /// </summary>
    private GameObject enemy_go;

    /// <summary>
    /// Pozycja gracza przed wejœciem do walki.
    /// </summary>
    private Vector3 player_position_before_entering_the_fight;

    /// <summary>
    /// Inicjalizacja obiektów gracza w grze.
    /// </summary>
    private void Awake()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }

    /// <summary>
    /// Subskrypcja zdarzeñ zwi¹zanych z rozpoczêciem i zakoñczeniem turnieju.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnStartingFightTournament += StartTournament;

        GlobalEvents.OnBeatingEnemyInATournament += ManageWiningWithEnemyInTournament;
    }

    /// <summary>
    /// Odpina subskrypcje zdarzeñ.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingFightTournament -= StartTournament;

        GlobalEvents.OnBeatingEnemyInATournament -= ManageWiningWithEnemyInTournament;
    }

    /// <summary>
    /// Zarz¹dza sytuacj¹ po pokonaniu wroga w turnieju. Jeœli gracz pokona³ ostatniego wroga,
    /// koñczy turniej, w przeciwnym razie spawn'uje kolejnego wroga.
    /// </summary>
    private void ManageWiningWithEnemyInTournament(object sender, System.EventArgs e)
    {
        defeated_enemy_in_tournament_so_far++;

        if (defeated_enemy_in_tournament_so_far == NUMBER_OF_ENEMY_TO_DEFEAT_TO_WIN_TOURNAMENT)
        {
            EndTournament();
        }
        else
        {
            Invoke(nameof(SpawnEnemy), 2.0f);
        }
    }

    /// <summary>
    /// Rozpoczyna turniej walki, teleportuje gracza na ring i spawn'uje wroga.
    /// </summary>
    public void StartTournament(object sender, System.EventArgs e)
    {
        GlobalEvents.FireOnStartingTransition(this, new(0.3f));

        FindAnyObjectByType<PlayerAttackAbsorber>().SetIsInFightTournament(true);

        player_position_before_entering_the_fight = player_go.transform.position;

        Invoke(nameof(TeleportPlayerToRing), 0.3f);
        ring_collider.SetActive(true);
        SpawnEnemy();
    }

    /// <summary>
    /// Zdarzenie wywo³ywane, gdy gracz umiera w trakcie turnieju.
    /// Zatrzymuje walkê i przenosi gracza na pocz¹tkow¹ pozycjê.
    /// </summary>
    public void PlayerDiedInTournament()
    {
        ring_collider.SetActive(false);

        GlobalEvents.FireOnStartingTransition(this, new(0.5f));
        enemy_go.GetComponentInChildren<FightNPC>().EndFight();

        FindAnyObjectByType<PlayerAttackAbsorber>().SetIsInFightTournament(false);
    }

    /// <summary>
    /// Koñczy turniej po pokonaniu wszystkich wrogów. Zatrzymuje walkê i zwraca gracza do poprzedniej pozycji.
    /// </summary>
    public void EndTournament()
    {
        ring_collider.SetActive(false);
        QuestManager.Instance.MarkQuestCompleted(8);

        GlobalEvents.FireOnStartingTransition(this, new(0.5f));
        Invoke(nameof(TeleportPlayerToPostinionBeforeStartingAFight), 0.3f);

        FindAnyObjectByType<PlayerAttackAbsorber>().SetIsInFightTournament(false);
    }

    /// <summary>
    /// Teleportuje gracza na ring, gdzie rozpocznie walkê.
    /// </summary>
    private void TeleportPlayerToRing()
    {
        player_go.transform.position = place_to_tp_player_to_when_starting_tournament.position;
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Teleportuje gracza do jego poprzedniej pozycji po zakoñczeniu walki.
    /// </summary>
    private void TeleportPlayerToPostinionBeforeStartingAFight()
    {
        player_go.transform.position = player_position_before_entering_the_fight;
        player_go.transform.localRotation = Quaternion.Euler(0.0f, Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT, 0.0f);
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Spawn'uje wroga w miejscu wyznaczonym na pocz¹tek walki.
    /// </summary>
    private void SpawnEnemy()
    {
        enemy_go = Instantiate(enemy_prefab, enemy_starting_position.position, Quaternion.identity);
        enemy_go.GetComponentInChildren<FightNPC>().BeginFight(player_go.transform);
    }
}
