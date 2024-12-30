using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private const int NUMBER_OF_ENEMY_TO_DEFEAT_TO_WIN_TOURNAMENT = 3;
    private const float Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT = 170.0f;

    [SerializeField] private GameObject ring_collider;
    [SerializeField] private GameObject enemy_prefab;
    [SerializeField] private Transform place_to_tp_player_to_when_starting_tournament;
    [SerializeField] private Transform enemy_starting_position;

    private int defeated_enemy_in_tournament_so_far = 0;

    private GameObject player_go;

    private Vector3 player_position_before_entering_the_fight;

    private void Awake()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }

    private void OnEnable()
    {
        GlobalEvents.OnStartingFightTournament += StartTournament;
        
        GlobalEvents.OnBeatingEnemyInATournament += ManageWiningWithEnemyInTournament;
    }

    private void OnDisable()
    {
        GlobalEvents.OnStartingFightTournament -= StartTournament;
        
        GlobalEvents.OnBeatingEnemyInATournament -= ManageWiningWithEnemyInTournament;
    }

    private void ManageWiningWithEnemyInTournament(object sender, System.EventArgs e)
    {
        defeated_enemy_in_tournament_so_far++;

        if(defeated_enemy_in_tournament_so_far == NUMBER_OF_ENEMY_TO_DEFEAT_TO_WIN_TOURNAMENT)
        {
            EndTournament();
        }
        else
        {
            Invoke(nameof(SpawnEnemy), 2.0f);
        }
    }

    private void StartTournament(object sender, System.EventArgs e)
    {
        GlobalEvents.FireOnStartingTransition(this, new(0.3f));
        
        player_position_before_entering_the_fight = player_go.transform.position;
        
        Invoke(nameof(TeleportPlayerToRing), 0.3f);
        ring_collider.SetActive(true);
        SpawnEnemy();
    }

    private void EndTournament()
    {
        ring_collider.SetActive(false);
        QuestManager.Instance.MarkQuestCompleted(0);

        GlobalEvents.FireOnStartingTransition(this, new(0.5f));

        Invoke(nameof(TeleportPlayerToPostinionBeforeStartingAFight), 0.3f);
    }

    private void TeleportPlayerToRing()
    {
        player_go.transform.position = place_to_tp_player_to_when_starting_tournament.position;
    }

    private void TeleportPlayerToPostinionBeforeStartingAFight()
    {
        player_go.transform.position = player_position_before_entering_the_fight;
        player_go.transform.localRotation = Quaternion.Euler(0.0f, Y_VALUE_OF_PLAYER_ROTATION_AFETR_ENDING_THE_FIGHT, 0.0f);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy_prefab, enemy_starting_position.position, Quaternion.identity);
    }
}
