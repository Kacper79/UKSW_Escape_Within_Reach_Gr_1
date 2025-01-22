using Assets.Scripts.AI;
using Assets.Scripts.PlayerRelated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa zarzadzajaca walka w turnieju. Obsluguje rozpoczecie turnieju, dodawanie wrogow,
/// zarzadzanie stanem turnieju oraz zakonczenie turnieju po pokonaniu wszystkich wrogow.
/// </summary>
public class FightManager : MonoBehaviour
{
    /// <summary>
    /// Liczba wrogow, ktorych trzeba pokonac, aby wygrac turniej.
    /// </summary>
    private const int NUMBER_OF_ENEMY_TO_DEFEAT_TO_WIN_TOURNAMENT = 3;

    /// <summary>
    /// Kat rotacji gracza po zakonczeniu walki.
    /// </summary>
    private const float Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT = 170.0f;

    /// <summary>
    /// Collider pierscienia, ktory aktywuje walke.
    /// </summary>
    [SerializeField] private GameObject ring_collider;

    /// <summary>
    /// Prefab wroga, ktory zostanie zainstantiowany podczas walki.
    /// </summary>
    [SerializeField] private GameObject enemy_prefab;

    /// <summary>
    /// Miejsce, do ktorego zostanie przeniesiony gracz, gdy rozpocznie turniej.
    /// </summary>
    [SerializeField] private Transform place_to_tp_player_to_when_starting_tournament;

    /// <summary>
    /// Poczatkowa pozycja wroga, w ktorym bedzie sie znajdowac na poczatku walki.
    /// </summary>
    [SerializeField] private Transform enemy_starting_position;

    /// <summary>
    /// Liczba pokonanych wrogow w trakcie trwania turnieju.
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
    /// Pozycja gracza przed wejsciem do walki.
    /// </summary>
    private Vector3 player_position_before_entering_the_fight;

    /// <summary>
    /// Inicjalizacja obiektow gracza w grze.
    /// </summary>
    private void Awake()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }

    /// <summary>
    /// Subskrypcja zdarzen zwiazanych z rozpoczeciem i zakonczeniem turnieju.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnStartingFightTournament += StartTournament;

        GlobalEvents.OnBeatingEnemyInATournament += ManageWiningWithEnemyInTournament;
    }

    /// <summary>
    /// Odpina subskrypcje zdarzen.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingFightTournament -= StartTournament;

        GlobalEvents.OnBeatingEnemyInATournament -= ManageWiningWithEnemyInTournament;
    }

    /// <summary>
    /// Zarzadza sytuacja po pokonaniu wroga w turnieju. Jesli gracz pokonal ostatniego wroga,
    /// konczy turniej, w przeciwnym razie spawnuje kolejnego wroga.
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
    /// Rozpoczyna turniej walki, teleportuje gracza na ring i spawnuje wroga.
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
    /// Zdarzenie wywolywane, gdy gracz umiera w trakcie turnieju.
    /// Zatrzymuje walke i przenosi gracza na poczatkowa pozycje.
    /// </summary>
    public void PlayerDiedInTournament()
    {
        ring_collider.SetActive(false);

        GlobalEvents.FireOnStartingTransition(this, new(0.5f));
        enemy_go.GetComponentInChildren<FightNPC>().EndFight();

        FindAnyObjectByType<PlayerAttackAbsorber>().SetIsInFightTournament(false);
    }

    /// <summary>
    /// Konczy turniej po pokonaniu wszystkich wrogow. Zatrzymuje walke i zwraca gracza do poprzedniej pozycji.
    /// </summary>
    public void EndTournament()
    {
        ring_collider.SetActive(false);
        QuestManager.Instance.MarkQuestCompleted(0);

        GlobalEvents.FireOnStartingTransition(this, new(0.5f));
        Invoke(nameof(TeleportPlayerToPostinionBeforeStartingAFight), 0.3f);

        FindAnyObjectByType<PlayerAttackAbsorber>().SetIsInFightTournament(false);
        player_go.GetComponentInChildren<InventoryManager>().PickUpItem(this, new(player_go.GetComponentInChildren<InventoryManager>().allPosibleObjects[7]));
    }

    /// <summary>
    /// Teleportuje gracza na ring, gdzie rozpocznie walke.
    /// </summary>
    private void TeleportPlayerToRing()
    {
        player_go.transform.position = place_to_tp_player_to_when_starting_tournament.position;
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Teleportuje gracza do jego poprzedniej pozycji po zakonczeniu walki.
    /// </summary>
    private void TeleportPlayerToPostinionBeforeStartingAFight()
    {
        player_go.transform.position = player_position_before_entering_the_fight;
        player_go.transform.localRotation = Quaternion.Euler(0.0f, Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT, 0.0f);
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Spawnuje wroga w miejscu wyznaczonym na poczatek walki.
    /// </summary>
    private void SpawnEnemy()
    {
        enemy_go = Instantiate(enemy_prefab, enemy_starting_position.position, Quaternion.identity);
        enemy_go.GetComponentInChildren<FightNPC>().BeginFight(player_go.transform);
    }
}
