using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rock_prefab;

    private void Start()
    {
        SpawnRock();
    }

    private void OnEnable()
    {
        GlobalEvents.OnDestroingRock += OnPlayerDestroyingRock;
    }

    private void OnDisable()
    {
        GlobalEvents.OnDestroingRock -= OnPlayerDestroyingRock;
    }

    private void OnPlayerDestroyingRock(object sender, System.EventArgs e)
    {
        Invoke(nameof(SpawnRock), 1.0f);
        QuestManager.Instance.MarkQuestCompleted(0);
    }

    private void SpawnRock()
    {
        Instantiate(rock_prefab, this.transform.position, Quaternion.identity);
    }
}
