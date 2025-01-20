using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie paskiem zdrowia wroga, jego rotacje w strone gracza
/// oraz aktualizacje wartosci paska w zaleznosci od stanu zdrowia wroga.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    /// <summary>
    /// Obiekt reprezentujacy sam pasek zdrowia wroga.
    /// </summary>
    [SerializeField] private GameObject bar;

    /// <summary>
    /// Glowny obiekt wroga, dla ktorego wyswietlany jest pasek zdrowia.
    /// </summary>
    [SerializeField] private GameObject enemy_main_go;

    /// <summary>
    /// Obiekt gracza, ktorego pozycja jest wykorzystywana do orientacji paska zdrowia.
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
    /// Aktualizuje rotacje paska zdrowia w celu skierowania go w strone gracza.
    /// </summary>
    private void Update()
    {
        HandleRotation();
    }

    /// <summary>
    /// Obsluguje rotacje paska zdrowia w kierunku gracza, uwzglêdniajac kat wzgledem wroga.
    /// </summary>
    private void HandleRotation()
    {
        float angle = Vector3.Angle(enemy_main_go.transform.forward, player_go.transform.position - enemy_main_go.transform.position);

        // Okresla, czy gracz znajduje sie z lewej czy z prawej strony wroga
        angle = Vector3.Angle(enemy_main_go.transform.right, player_go.transform.position - enemy_main_go.transform.position) <= 90.0f ? angle : -angle;

        // Ustawia rotacje paska zdrowia, tak aby zawsze byl zwrocony w strone gracza
        transform.localRotation = Quaternion.Euler(0.0f,
                                                   180.0f + angle,
                                                   0.0f);
    }

    /// <summary>
    /// Zmienia wartosc paska zdrowia w zaleznosci od maksymalnej i aktualnej wartosci zdrowia.
    /// </summary>
    /// <param name="max_value">Maksymalna ilosc zdrowia wroga.</param>
    /// <param name="current_value">Aktualna ilosc zdrowia wroga.</param>
    public void ChangeBarValue(int max_value, int current_value)
    {
        bar.transform.localScale = new(CalculateXScale(max_value, current_value), bar.transform.localScale.y, bar.transform.localScale.z);
    }

    /// <summary>
    /// Oblicza nowa wartosc skali paska zdrowia na osi X, w zaleznosci od aktualnego stanu zdrowia.
    /// </summary>
    /// <param name="max_value">Maksymalna ilosc zdrowia wroga.</param>
    /// <param name="current_value">Aktualna ilosc zdrowia wroga.</param>
    /// <returns>Wartosc skali w osi X, ktora odzwierciedla aktualne zdrowie wroga.</returns>
    private float CalculateXScale(int max_value, int current_value)
    {
        return ((float)current_value / (float)max_value) * (-1);
    }
}
