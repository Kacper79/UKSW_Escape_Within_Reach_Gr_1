using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz¹dzanie paskiem zdrowia wroga, jego rotacjê w stronê gracza
/// oraz aktualizacjê wartoœci paska w zale¿noœci od stanu zdrowia wroga.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    /// <summary>
    /// Obiekt reprezentuj¹cy sam pasek zdrowia wroga.
    /// </summary>
    [SerializeField] private GameObject bar;

    /// <summary>
    /// G³ówny obiekt wroga, dla którego wyœwietlany jest pasek zdrowia.
    /// </summary>
    [SerializeField] private GameObject enemy_main_go;

    /// <summary>
    /// Obiekt gracza, którego pozycja jest wykorzystywana do orientacji paska zdrowia.
    /// </summary>
    private GameObject player_go;

    /// <summary>
    /// Inicjalizacja obiektu gracza.
    /// </summary>
    private void Awake()
    {
        player_go = FindObjectOfType<PlayerInputController>().gameObject;
    }

    /// <summary>
    /// Aktualizuje rotacjê paska zdrowia w celu skierowania go w stronê gracza.
    /// </summary>
    private void Update()
    {
        HandleRotation();
    }

    /// <summary>
    /// Obs³uguje rotacjê paska zdrowia w kierunku gracza, uwzglêdniaj¹c k¹t wzglêdem wroga.
    /// </summary>
    private void HandleRotation()
    {
        float angle = Vector3.Angle(enemy_main_go.transform.forward, player_go.transform.position - enemy_main_go.transform.position);

        // Okreœla, czy gracz znajduje siê z lewej czy z prawej strony wroga
        angle = Vector3.Angle(enemy_main_go.transform.right, player_go.transform.position - enemy_main_go.transform.position) <= 90.0f ? angle : -angle;

        // Ustawia rotacjê paska zdrowia, tak aby zawsze by³ zwrócony w stronê gracza
        transform.localRotation = Quaternion.Euler(0.0f,
                                                   180.0f + angle,
                                                   0.0f);
    }

    /// <summary>
    /// Zmienia wartoœæ paska zdrowia w zale¿noœci od maksymalnej i aktualnej wartoœci zdrowia.
    /// </summary>
    /// <param name="max_value">Maksymalna iloœæ zdrowia wroga.</param>
    /// <param name="current_value">Aktualna iloœæ zdrowia wroga.</param>
    public void ChangeBarValue(int max_value, int current_value)
    {
        bar.transform.localScale = new(CalculateXScale(max_value, current_value), bar.transform.localScale.y, bar.transform.localScale.z);
    }

    /// <summary>
    /// Oblicza now¹ wartoœæ skali paska zdrowia na osi X, w zale¿noœci od aktualnego stanu zdrowia.
    /// </summary>
    /// <param name="max_value">Maksymalna iloœæ zdrowia wroga.</param>
    /// <param name="current_value">Aktualna iloœæ zdrowia wroga.</param>
    /// <returns>Wartoœæ skali w osi X, która odzwierciedla aktualne zdrowie wroga.</returns>
    private float CalculateXScale(int max_value, int current_value)
    {
        return ((float)current_value / (float)max_value) * (-1);
    }
}
