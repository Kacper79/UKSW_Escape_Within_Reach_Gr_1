using Assets.Scripts.AI;
using Assets.Scripts.PlayerRelated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa zarz�dzaj�ca walk� w turnieju. Obs�uguje rozpocz�cie turnieju, dodawanie wrog�w,
/// zarz�dzanie stanem turnieju oraz zako�czenie turnieju po pokonaniu wszystkich wrog�w.
/// </summary>
public class FightManager : MonoBehaviour
{
    /// <summary>
    /// Liczba wrog�w, kt�rych trzeba pokona�, aby wygra� turniej.
    /// </summary>
    private const int NUMBER_OF_ENEMY_TO_DEFEAT_TO_WIN_TOURNAMENT = 3;

    /// <summary>
    /// K�t rotacji gracza po zako�czeniu walki.
    /// </summary>
    private const float Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT = 170.0f;

    /// <summary>
    /// Collider pier�cienia, kt�ry aktywuje walk�.
    /// </summary>
    [SerializeField] private GameObject ring_collider;

    /// <summary>
    /// Prefab wroga, kt�ry zostanie zainstantiowany podczas walki.
    /// </summary>
    [SerializeField] private GameObject enemy_prefab;

    /// <summary>
    /// Miejsce, do kt�rego zostanie przeniesiony gracz, gdy rozpocznie turniej.
    /// </summary>
    [SerializeField] private Transform place_to_tp_player_to_when_starting_tournament;

    /// <summary>
    /// Pocz�tkowa pozycja wroga, w kt�rym b�dzie si� znajdowa� na pocz�tku walki.
    /// </summary>
    [SerializeField] private Transform enemy_starting_position;

    /// <summary>
    /// Liczba pokonanych wrog�w w trakcie trwania turnieju.
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
    /// Pozycja gracza przed wej�ciem do walki.
    /// </summary>
    private Vector3 player_position_before_entering_the_fight;

    /// <summary>
    /// Inicjalizacja obiekt�w gracza w grze.
    /// </summary>
    private void Awake()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }

    /// <summary>
    /// Subskrypcja zdarze� zwi�zanych z rozpocz�ciem i zako�czeniem turnieju.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnStartingFightTournament += StartTournament;

        GlobalEvents.OnBeatingEnemyInATournament += ManageWiningWithEnemyInTournament;
    }

    /// <summary>
    /// Odpina subskrypcje zdarze�.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingFightTournament -= StartTournament;

        GlobalEvents.OnBeatingEnemyInATournament -= ManageWiningWithEnemyInTournament;
    }

    /// <summary>
    /// Zarz�dza sytuacj� po pokonaniu wroga w turnieju. Je�li gracz pokona� ostatniego wroga,
    /// ko�czy turniej, w przeciwnym razie spawn'uje kolejnego wroga.
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
    /// Zdarzenie wywo�ywane, gdy gracz umiera w trakcie turnieju.
    /// Zatrzymuje walk� i przenosi gracza na pocz�tkow� pozycj�.
    /// </summary>
    public void PlayerDiedInTournament()
    {
        ring_collider.SetActive(false);

        GlobalEvents.FireOnStartingTransition(this, new(0.5f));
        enemy_go.GetComponentInChildren<FightNPC>().EndFight();

        FindAnyObjectByType<PlayerAttackAbsorber>().SetIsInFightTournament(false);
    }

    /// <summary>
    /// Ko�czy turniej po pokonaniu wszystkich wrog�w. Zatrzymuje walk� i zwraca gracza do poprzedniej pozycji.
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
    /// Teleportuje gracza na ring, gdzie rozpocznie walk�.
    /// </summary>
    private void TeleportPlayerToRing()
    {
        player_go.transform.position = place_to_tp_player_to_when_starting_tournament.position;
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Teleportuje gracza do jego poprzedniej pozycji po zako�czeniu walki.
    /// </summary>
    private void TeleportPlayerToPostinionBeforeStartingAFight()
    {
        player_go.transform.position = player_position_before_entering_the_fight;
        player_go.transform.localRotation = Quaternion.Euler(0.0f, Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT, 0.0f);
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Spawn'uje wroga w miejscu wyznaczonym na pocz�tek walki.
    /// </summary>
    private void SpawnEnemy()
    {
        enemy_go = Instantiate(enemy_prefab, enemy_starting_position.position, Quaternion.identity);
        enemy_go.GetComponentInChildren<FightNPC>().BeginFight(player_go.transform);
    }
}
