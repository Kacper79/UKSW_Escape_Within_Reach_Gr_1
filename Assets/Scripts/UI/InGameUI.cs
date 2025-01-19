using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PlayerRelated;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    /// <summary>
    /// Odwo�anie do wy�wietlania aktualnego czasu w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI time_of_day_display;

    /// <summary>
    /// Odwo�anie do wy�wietlania aktualnych punkt�w �ycia w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI current_hp_display;

    /// <summary>
    /// Odwo�anie do wy�wietlania ilo�ci z�ota w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gold_display;

    /// <summary>
    /// Odwo�anie do paska post�pu przedstawiaj�cego poziom stresu.
    /// </summary>
    [SerializeField] private ProgressBar stress_progress_bar;

    /// <summary>
    /// Odwo�anie do paska post�pu przedstawiaj�cego punkty �ycia.
    /// </summary>
    [SerializeField] private ProgressBar hp_bar;

    /// <summary>
    /// Odwo�anie do obiektu gracza.
    /// </summary>
    private GameObject player_go;


    /// <summary>
    /// Inicjalizuje referencje do obiekt�w gracza.
    /// </summary>
    private void Start()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }

    /// <summary>
    /// Rejestruje subskrypcj� do zdarze� systemowych, kt�re zmieniaj� stan UI.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnChangingTime += ChangeTimeLabel;
        GlobalEvents.OnTimeStart += EnableUI;
        GlobalEvents.OnTimeStop += DisableUI;

        GlobalEvents.OnAnyUIOpen += DisableUI;
        GlobalEvents.OnAnyUIClose += EnableUI;
    }

    /// <summary>
    /// Aktualizuje warto�ci wy�wietlane w UI na podstawie danych gracza.
    /// </summary>
    private void Update()
    {
        stress_progress_bar.SetProgressBarValues(player_go.GetComponent<PlayerStress>().CurrentStressLevel, player_go.GetComponent<PlayerStress>().maxStressLevel);
        hp_bar.SetProgressBarValues(player_go.GetComponent<PlayerAttackAbsorber>().GetHp(), player_go.GetComponent<PlayerAttackAbsorber>().MaxHP);
        gold_display.text = player_go.GetComponent<InventoryManager>().GetGoldAmount().ToString();
        DisplayCurrentHp();
    }

    /// <summary>
    /// Usuwa subskrypcj� do zdarze� systemowych, aby unikn�� wyciek�w pami�ci.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnChangingTime -= ChangeTimeLabel;
        GlobalEvents.OnTimeStart -= EnableUI;
        GlobalEvents.OnTimeStop -= DisableUI;

        GlobalEvents.OnAnyUIOpen -= DisableUI;
        GlobalEvents.OnAnyUIClose -= EnableUI;
    }

    /// <summary>
    /// W��cza wszystkie elementy UI, kiedy gra zaczyna si� lub czas zmienia.
    /// </summary>
    private void EnableUI(object sender, System.EventArgs e)
    {
        time_of_day_display.enabled = true;
        stress_progress_bar.gameObject.SetActive(true);
        hp_bar.gameObject.SetActive(true);
        current_hp_display.gameObject.SetActive(true);
        gold_display.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wy��cza wszystkie elementy UI, gdy gra wstrzymana lub inny UI jest otwarty.
    /// </summary>
    private void DisableUI(object sender, System.EventArgs e)
    {
        time_of_day_display.enabled = false;
        stress_progress_bar.gameObject.SetActive(false);
        hp_bar.gameObject.SetActive(false);
        current_hp_display.gameObject.SetActive(false);
        gold_display.gameObject.SetActive(false);
    }

    /// <summary>
    /// Aktualizuje etykiet� czasu w UI na podstawie wydarzenia zmiany czasu.
    /// </summary>
    private void ChangeTimeLabel(object sender, EventArgs e)
    {
        if (e is GlobalEvents.OnChangingTimeArgs args)
        {
            time_of_day_display.text = ChangeTimeIntoString(args.minutes);
        }
        else
        {
            Debug.Log("Bug in InGameUI, received EventArgs not of type GlobalEvents.OnChangingTimeArgs");
        }
    }

    /// <summary>
    /// Zmienia czas wyra�ony w minutach na format "hh:mm".
    /// </summary>
    /// <param name="time_in_minutes">Czas w minutach, kt�ry ma zosta� przekszta�cony na format godzinowy.</param>
    /// <returns>Formatowany czas w postaci godziny i minut (hh:mm).</returns>
    private string ChangeTimeIntoString(int time_in_minutes)
    {
        TimeSpan time = TimeSpan.FromMinutes(time_in_minutes);
        return time.ToString(@"hh\:mm");
    }

    /// <summary>
    /// Wy�wietla aktualn� ilo�� punkt�w �ycia gracza i zmienia kolor w zale�no�ci od procentowego poziomu HP.
    /// </summary>
    private void DisplayCurrentHp()
    {
        float current_hp = (float)player_go.GetComponent<PlayerAttackAbsorber>().GetHp();
        float max_hp = (float)player_go.GetComponent<PlayerAttackAbsorber>().MaxHP;

        current_hp_display.text = current_hp.ToString();

        float hp_percent = (current_hp / max_hp) * 100;

        if (hp_percent <= 20)
        {
            current_hp_display.color = Color.red;
        }
        else if (hp_percent > 20 && hp_percent <= 50)
        {
            current_hp_display.color = Color.yellow;
        }
    }
}
