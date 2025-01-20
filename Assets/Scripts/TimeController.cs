using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kontroler czasu gry, zarzadzajacy uplywem czasu oraz zdarzeniami powiazanymi z cyklem dnia.
/// Implementuje interfejs ISaveable, co umozliwia zapis i wczytywanie stanu czasu.
/// </summary>
public class TimeController : MonoBehaviour, ISaveable
{
    /// <summary>
    /// Czas rozpoczecia dnia w minutach (godzina 8:00).
    /// </summary>
    private const int DAY_START_TIME = 8 * 60;

    /// <summary>
    /// Czas zakoeczenia dnia w minutach (godzina 22:00).
    /// </summary>
    private const int DAY_END_TIME = 22 * 60;

    /// <summary>
    /// Czestotliwosc przyrostu czasu w sekundach (dodawanie 1 minuty do zegara gry).
    /// </summary>
    private const float FREQUENCY_OF_TIME_FLOW_IN_SECONDS = 1f;

    /// <summary>
    /// Aktualny czas w minutach od poczatku dnia.
    /// </summary>
    private int time_in_minutes;

    /// <summary>
    /// Czas, jaki uplynal od ostatniego dodania minuty do zegara gry.
    /// </summary>
    private float time_passed_since_last_added_minute = 0;

    /// <summary>
    /// Flaga wskazujaca, czy czas gry jest zatrzymany.
    /// </summary>
    private bool is_time_stopped;

    /// <summary>
    /// Argumenty zdarzenia informujace o zmianie czasu.
    /// </summary>
    private GlobalEvents.OnChangingTimeArgs on_changing_time_args;

    /// <summary>
    /// Ustawia poczatkowy czas gry na czas rozpoczecia dnia.
    /// </summary>
    private void Awake()
    {
        time_in_minutes = 21 * 60;
    }

    /// <summary>
    /// Subskrybuje globalne zdarzenia uruchamiania i zatrzymywania czasu.
    /// </summary>
    private void OnEnable()
    {
        GlobalEvents.OnTimeStart += StartTime;
        GlobalEvents.OnTimeStop += StopTime;
    }

    /// <summary>
    /// Wyrejestrowuje subskrypcje globalnych zdarzen uruchamiania i zatrzymywania czasu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnTimeStart -= StartTime;
        GlobalEvents.OnTimeStop -= StopTime;
    }

    /// <summary>
    /// Inicjalizuje kontroler czasu i powiadamia o poczatkowym czasie gry.
    /// </summary>
    private void Start()
    {
        SaveManager.Instance.saveablesGO.Add(this);
        on_changing_time_args = new GlobalEvents.OnChangingTimeArgs(time_in_minutes);
        GlobalEvents.FireOnChangingTime(this, on_changing_time_args);
    }

    /// <summary>
    /// Aktualizuje uplyw czasu gry w kazdej klatce, o ile czas nie jest zatrzymany.
    /// </summary>
    private void Update()
    {
        if (!is_time_stopped)
        {
            time_passed_since_last_added_minute += Time.deltaTime;

            if (time_passed_since_last_added_minute > FREQUENCY_OF_TIME_FLOW_IN_SECONDS)
            {
                time_in_minutes++;
                time_passed_since_last_added_minute = 0.0f;

                on_changing_time_args = new GlobalEvents.OnChangingTimeArgs(time_in_minutes);
                GlobalEvents.FireOnChangingTime(this, on_changing_time_args);
            }

            if (IsDayEndTime())
            {
                Debug.Log("End of day, killing prisoners not in cells");
                // Jesli gracz nie jest w swoim pokoju, "zabij" gracza i przenies go do pokoju.
                ResetDayTime();
            }
        }
    }

    /// <summary>
    /// Sprawdza, czy czas osiagnal godzine zakonczenia dnia.
    /// </summary>
    /// <returns>True, jesli osiegnieto koniec dnia; w przeciwnym razie false.</returns>
    private bool IsDayEndTime()
    {
        return time_in_minutes == DAY_END_TIME;
    }

    /// <summary>
    /// Resetuje czas do poczatku dnia.
    /// </summary>
    private void ResetDayTime()
    {
        time_in_minutes = DAY_START_TIME;
    }

    /// <summary>
    /// This function is being used to save date's time to a file
    /// </summary>
    /// <param name="saveData">Mutable save data struct</param>
    public void Save(ref SaveData saveData)
    {
        saveData.timeInMinutes = time_in_minutes;
    }

    /// <summary>
    /// This function is being used to load date's time from a file
    /// </summary>
    /// <param name="saveData">Save data struct</param>
    public void Load(SaveData saveData)
    {
        time_in_minutes = saveData.timeInMinutes;
    }

    /// <summary>
    /// Zatrzymuje uplyw czasu gry po otrzymaniu odpowiedniego zdarzenia globalnego.
    /// </summary>
    private void StopTime(object sender, System.EventArgs e)
    {
        is_time_stopped = true;
    }

    /// <summary>
    /// Wznawia uplyw czasu gry po otrzymaniu odpowiedniego zdarzenia globalnego.
    /// </summary>
    private void StartTime(object sender, System.EventArgs e)
    {
        is_time_stopped = false;
    }
}

