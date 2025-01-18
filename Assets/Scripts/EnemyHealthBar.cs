using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie paskiem zdrowia wroga, jego rotacj� w stron� gracza
/// oraz aktualizacj� warto�ci paska w zale�no�ci od stanu zdrowia wroga.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    /// <summary>
    /// Obiekt reprezentuj�cy sam pasek zdrowia wroga.
    /// </summary>
    [SerializeField] private GameObject bar;

    /// <summary>
    /// G��wny obiekt wroga, dla kt�rego wy�wietlany jest pasek zdrowia.
    /// </summary>
    [SerializeField] private GameObject enemy_main_go;

    /// <summary>
    /// Obiekt gracza, kt�rego pozycja jest wykorzystywana do orientacji paska zdrowia.
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
    /// Aktualizuje rotacj� paska zdrowia w celu skierowania go w stron� gracza.
    /// </summary>
    private void Update()
    {
        HandleRotation();
    }

    /// <summary>
    /// Obs�uguje rotacj� paska zdrowia w kierunku gracza, uwzgl�dniaj�c k�t wzgl�dem wroga.
    /// </summary>
    private void HandleRotation()
    {
        float angle = Vector3.Angle(enemy_main_go.transform.forward, player_go.transform.position - enemy_main_go.transform.position);

        // Okre�la, czy gracz znajduje si� z lewej czy z prawej strony wroga
        angle = Vector3.Angle(enemy_main_go.transform.right, player_go.transform.position - enemy_main_go.transform.position) <= 90.0f ? angle : -angle;

        // Ustawia rotacj� paska zdrowia, tak aby zawsze by� zwr�cony w stron� gracza
        transform.localRotation = Quaternion.Euler(0.0f,
                                                   180.0f + angle,
                                                   0.0f);
    }

    /// <summary>
    /// Zmienia warto�� paska zdrowia w zale�no�ci od maksymalnej i aktualnej warto�ci zdrowia.
    /// </summary>
    /// <param name="max_value">Maksymalna ilo�� zdrowia wroga.</param>
    /// <param name="current_value">Aktualna ilo�� zdrowia wroga.</param>
    public void ChangeBarValue(int max_value, int current_value)
    {
        bar.transform.localScale = new(CalculateXScale(max_value, current_value), bar.transform.localScale.y, bar.transform.localScale.z);
    }

    /// <summary>
    /// Oblicza now� warto�� skali paska zdrowia na osi X, w zale�no�ci od aktualnego stanu zdrowia.
    /// </summary>
    /// <param name="max_value">Maksymalna ilo�� zdrowia wroga.</param>
    /// <param name="current_value">Aktualna ilo�� zdrowia wroga.</param>
    /// <returns>Warto�� skali w osi X, kt�ra odzwierciedla aktualne zdrowie wroga.</returns>
    private float CalculateXScale(int max_value, int current_value)
    {
        return ((float)current_value / (float)max_value) * (-1);
    }
}
