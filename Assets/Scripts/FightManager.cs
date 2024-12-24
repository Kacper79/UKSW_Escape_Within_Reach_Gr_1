using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private const int NUMBER_OF_ENEMY_TO_DEFEAT_TO_WIN_TOURNAMENT = 3;

    [SerializeField] private GameObject ring_collider;
    [SerializeField] private GameObject enemy_prefab;
    [SerializeField] private Transform place_to_tp_player_to_when_starting_tournament;
    [SerializeField] private Transform enemy_starting_position;

    private int defeated_enemy_in_tournament_so_far = 0;

    private GameObject player_go;

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
        Debug.Log("siema");
        TeleportPlayerToRing();
        ring_collider.SetActive(true);
        Invoke(nameof(SpawnEnemy), 2.0f);
    }

    private void EndTournament()
    {
        ring_collider.SetActive(false);
        QuestManager.Instance.MarkQuestCompleted(0);
    }

    private void TeleportPlayerToRing()
    {
        player_go.transform.position = place_to_tp_player_to_when_starting_tournament.position;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy_prefab, enemy_starting_position.position, Quaternion.identity);
    }
}
