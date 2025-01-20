using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za generowanie kamieni w grze.
/// Kamienie sa spawnowane po zniszczeniu poprzedniego kamienia przez gracza.
/// </summary>
public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rock_prefab; // Prefab kamienia, ktory ma byc generowany

    /// <summary>
    /// Spawnuje kamien na poczatku gry.
    /// </summary>
    private void Start()
    {
        SpawnRock();
    }

    /// <summary>
    /// Subskrybuje zdarzenie zniszczenia kamienia, aby po zniszczeniu kolejnego kamienia spawnowac nowy.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnDestroingRock += OnPlayerDestroyingRock;
    }

    /// <summary>
    /// Subskrypcja jest usuwana, gdy obiekt jest wylaczany.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnDestroingRock -= OnPlayerDestroyingRock;
    }

    /// <summary>
    /// Metoda wywolywana, gdy gracz zniszczy kamien. Spawnuje nowy kamien po krotkim opoznieniu.
    /// </summary>
    /// <param name="sender">Obiekt wysylajacy zdarzenie.</param>
    /// <param name="e">Dodatkowe informacje o zdarzeniu.</param>
    private void OnPlayerDestroyingRock(object sender, System.EventArgs e)
    {
        // Opoznia spawnowanie nowego kamienia o 1 sekunde
        Invoke(nameof(SpawnRock), 1.0f);

        // Oznaczenie questu jako ukonczonego, np. za zniszczenie kamienia
        QuestManager.Instance.MarkQuestCompleted(10);
    }

    /// <summary>
    /// Tworzy nowy kamien w miejscu spawnera.
    /// </summary>
    private void SpawnRock()
    {
        Instantiate(rock_prefab, this.transform.position, Quaternion.identity); // Spawnuje kamien w lokalizacji spawnera
    }
}
