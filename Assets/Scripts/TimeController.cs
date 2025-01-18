using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kontroler czasu gry, zarz¹dzaj¹cy up³ywem czasu oraz zdarzeniami powi¹zanymi z cyklem dnia.
/// Implementuje interfejs ISaveable, co umo¿liwia zapis i wczytywanie stanu czasu.
/// </summary>
public class TimeController : MonoBehaviour, ISaveable
{
    /// <summary>
    /// Czas rozpoczêcia dnia w minutach (godzina 8:00).
    /// </summary>
    private const int DAY_START_TIME = 8 * 60;

    /// <summary>
    /// Czas zakoñczenia dnia w minutach (godzina 22:00).
    /// </summary>
    private const int DAY_END_TIME = 22 * 60;

    /// <summary>
    /// Czêstotliwoœæ przyrostu czasu w sekundach (dodawanie 1 minuty do zegara gry).
    /// </summary>
    private const float FREQUENCY_OF_TIME_FLOW_IN_SECONDS = 1f;

    /// <summary>
    /// Aktualny czas w minutach od pocz¹tku dnia.
    /// </summary>
    private int time_in_minutes;

    /// <summary>
    /// Czas, jaki up³yn¹³ od ostatniego dodania minuty do zegara gry.
    /// </summary>
    private float time_passed_since_last_added_minute = 0;

    /// <summary>
    /// Flaga wskazuj¹ca, czy czas gry jest zatrzymany.
    /// </summary>
    private bool is_time_stopped;

    /// <summary>
    /// Argumenty zdarzenia informuj¹ce o zmianie czasu.
    /// </summary>
    private GlobalEvents.OnChangingTimeArgs on_changing_time_args;

    /// <summary>
    /// Ustawia pocz¹tkowy czas gry na czas rozpoczêcia dnia.
    /// </summary>
    private void Awake()
    {
        time_in_minutes = DAY_START_TIME;
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
    /// Wyrejestrowuje subskrypcje globalnych zdarzeñ uruchamiania i zatrzymywania czasu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnTimeStart -= StartTime;
        GlobalEvents.OnTimeStop -= StopTime;
    }

    /// <summary>
    /// Inicjalizuje kontroler czasu i powiadamia o pocz¹tkowym czasie gry.
    /// </summary>
    private void Start()
    {
        SaveManager.Instance.saveablesGO.Add(this);
        on_changing_time_args = new GlobalEvents.OnChangingTimeArgs(time_in_minutes);
        GlobalEvents.FireOnChangingTime(this, on_changing_time_args);
    }

    /// <summary>
    /// Aktualizuje up³yw czasu gry w ka¿dej klatce, o ile czas nie jest zatrzymany.
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
                // Jeœli gracz nie jest w swoim pokoju, "zabij" gracza i przenieœ go do pokoju.
                ResetDayTime();
            }
        }
    }

    /// <summary>
    /// Sprawdza, czy czas osi¹gn¹³ godzinê zakoñczenia dnia.
    /// </summary>
    /// <returns>True, jeœli osi¹gniêto koniec dnia; w przeciwnym razie false.</returns>
    private bool IsDayEndTime()
    {
        return time_in_minutes == DAY_END_TIME;
    }

    /// <summary>
    /// Resetuje czas do pocz¹tku dnia.
    /// </summary>
    private void ResetDayTime()
    {
        time_in_minutes = DAY_START_TIME;
    }

    /// <summary>
    /// Zapisuje aktualny czas gry w strukturze SaveData.
    /// </summary>
    /// <param name="saveData">Struktura, do której zapisywane s¹ dane gry.</param>
    public void Save(ref SaveData saveData)
    {
        saveData.timeInMinutes = time_in_minutes;
    }

    /// <summary>
    /// Wczytuje zapisany czas gry z przekazanej struktury SaveData.
    /// </summary>
    /// <param name="saveData">Struktura z zapisanymi danymi gry.</param>
    public void Load(SaveData saveData)
    {
        time_in_minutes = saveData.timeInMinutes;
    }

    /// <summary>
    /// Zatrzymuje up³yw czasu gry po otrzymaniu odpowiedniego zdarzenia globalnego.
    /// </summary>
    private void StopTime(object sender, System.EventArgs e)
    {
        is_time_stopped = true;
    }

    /// <summary>
    /// Wznawia up³yw czasu gry po otrzymaniu odpowiedniego zdarzenia globalnego.
    /// </summary>
    private void StartTime(object sender, System.EventArgs e)
    {
        is_time_stopped = false;
    }
}

