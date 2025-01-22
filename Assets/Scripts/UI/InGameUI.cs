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
    /// Odwolanie do wyswietlania aktualnego czasu w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI time_of_day_display;

    /// <summary>
    /// Odwolanie do wyswietlania aktualnych punktow zycia w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI current_hp_display;

    /// <summary>
    /// Odwolanie do wyswietlania ilosci zlota w UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gold_display;

    /// <summary>
    /// Odwolanie do paska postepu przedstawiajacego poziom stresu.
    /// </summary>
    [SerializeField] private ProgressBar stress_progress_bar;

    /// <summary>
    /// Odwolanie do paska postepu przedstawiajacego punkty zycia.
    /// </summary>
    [SerializeField] private ProgressBar hp_bar;

    /// <summary>
    /// Odwolanie do obiektu gracza.
    /// </summary>
    private GameObject player_go;


    /// <summary>
    /// Inicjalizuje referencje do obiektow gracza.
    /// </summary>
    private void Start()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }

    /// <summary>
    /// Rejestruje subskrypcje do zdarzen systemowych, ktore zmieniaja stan UI.
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
    /// Aktualizuje wartosci wyswietlane w UI na podstawie danych gracza.
    /// </summary>
    private void Update()
    {
        stress_progress_bar.SetProgressBarValues(player_go.GetComponent<PlayerStress>().CurrentStressLevel, player_go.GetComponent<PlayerStress>().maxStressLevel);
        hp_bar.SetProgressBarValues(player_go.GetComponent<PlayerAttackAbsorber>().GetHp(), player_go.GetComponent<PlayerAttackAbsorber>().MaxHP);
        gold_display.text = player_go.GetComponentInChildren<InventoryManager>().GetGoldAmount().ToString();
        DisplayCurrentHp();
    }

    /// <summary>
    /// Usuwa subskrypcje edo zdarzen systemowych, aby uniknac wyciekow pamieci.
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
    /// Wlacza wszystkie elementy UI, kiedy gra zaczyna sie lub czas zmienia.
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
    /// Wylacza wszystkie elementy UI, gdy gra wstrzymana lub inny UI jest otwarty.
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
    /// Aktualizuje etykiete czasu w UI na podstawie wydarzenia zmiany czasu.
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
    /// Zmienia czas wyra¿ony w minutach na format "hh:mm".
    /// </summary>
    /// <param name="time_in_minutes">Czas w minutach, ktory ma zostac przeksztalcony na format godzinowy.</param>
    /// <returns>Formatowany czas w postaci godziny i minut (hh:mm).</returns>
    private string ChangeTimeIntoString(int time_in_minutes)
    {
        TimeSpan time = TimeSpan.FromMinutes(time_in_minutes);
        return time.ToString(@"hh\:mm");
    }

    /// <summary>
    /// Wyswietla aktualna ilosc punktow zycia gracza i zmienia kolor w zaleznosci od procentowego poziomu HP.
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
