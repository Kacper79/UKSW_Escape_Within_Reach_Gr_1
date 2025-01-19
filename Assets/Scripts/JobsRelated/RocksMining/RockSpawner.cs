using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za generowanie kamieni w grze.
/// Kamienie s¹ spawnowane po zniszczeniu poprzedniego kamienia przez gracza.
/// </summary>
public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rock_prefab; // Prefab kamienia, który ma byæ generowany

    /// <summary>
    /// Spawnuje kamieñ na pocz¹tku gry.
    /// </summary>
    private void Start()
    {
        SpawnRock();
    }

    /// <summary>
    /// Subskrybuje zdarzenie zniszczenia kamienia, aby po zniszczeniu kolejnego kamienia spawnowaæ nowy.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnDestroingRock += OnPlayerDestroyingRock;
    }

    /// <summary>
    /// Subskrypcja jest usuwana, gdy obiekt jest wy³¹czany.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnDestroingRock -= OnPlayerDestroyingRock;
    }

    /// <summary>
    /// Metoda wywo³ywana, gdy gracz zniszczy kamieñ. Spawnuje nowy kamieñ po krótkim opóŸnieniu.
    /// </summary>
    /// <param name="sender">Obiekt wysy³aj¹cy zdarzenie.</param>
    /// <param name="e">Dodatkowe informacje o zdarzeniu.</param>
    private void OnPlayerDestroyingRock(object sender, System.EventArgs e)
    {
        // OpóŸnia spawnowanie nowego kamienia o 1 sekundê
        Invoke(nameof(SpawnRock), 1.0f);

        // Oznaczenie questu jako ukoñczonego, np. za zniszczenie kamienia
        QuestManager.Instance.MarkQuestCompleted(10);
    }

    /// <summary>
    /// Tworzy nowy kamieñ w miejscu spawnera.
    /// </summary>
    private void SpawnRock()
    {
        Instantiate(rock_prefab, this.transform.position, Quaternion.identity); // Spawnuje kamieñ w lokalizacji spawnera
    }
}
