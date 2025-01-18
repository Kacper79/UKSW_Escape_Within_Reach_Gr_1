using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za generowanie kamieni w grze.
/// Kamienie s� spawnowane po zniszczeniu poprzedniego kamienia przez gracza.
/// </summary>
public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rock_prefab; // Prefab kamienia, kt�ry ma by� generowany

    /// <summary>
    /// Spawnuje kamie� na pocz�tku gry.
    /// </summary>
    private void Start()
    {
        SpawnRock();
    }

    /// <summary>
    /// Subskrybuje zdarzenie zniszczenia kamienia, aby po zniszczeniu kolejnego kamienia spawnowa� nowy.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnDestroingRock += OnPlayerDestroyingRock;
    }

    /// <summary>
    /// Subskrypcja jest usuwana, gdy obiekt jest wy��czany.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnDestroingRock -= OnPlayerDestroyingRock;
    }

    /// <summary>
    /// Metoda wywo�ywana, gdy gracz zniszczy kamie�. Spawnuje nowy kamie� po kr�tkim op�nieniu.
    /// </summary>
    /// <param name="sender">Obiekt wysy�aj�cy zdarzenie.</param>
    /// <param name="e">Dodatkowe informacje o zdarzeniu.</param>
    private void OnPlayerDestroyingRock(object sender, System.EventArgs e)
    {
        // Op�nia spawnowanie nowego kamienia o 1 sekund�
        Invoke(nameof(SpawnRock), 1.0f);

        // Oznaczenie questu jako uko�czonego, np. za zniszczenie kamienia
        QuestManager.Instance.MarkQuestCompleted(10);
    }

    /// <summary>
    /// Tworzy nowy kamie� w miejscu spawnera.
    /// </summary>
    private void SpawnRock()
    {
        Instantiate(rock_prefab, this.transform.position, Quaternion.identity); // Spawnuje kamie� w lokalizacji spawnera
    }
}
