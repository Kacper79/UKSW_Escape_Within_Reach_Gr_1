using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private const int DAY_START_TIME = 8 * 60;
    private const int DAY_END_TIME = 20 * 60;
    private const float FREQUENCY_OF_TIME_FLOW_IN_SECONDS = 0.01f;//how often do you add 1 minute to the time display

    private int time_in_minutes;
    private float time_passed_since_last_added_minute = 0;

    private GlobalEvents.OnChangingTimeArgs on_changing_time_args;

    private void Awake()
    {
        time_in_minutes = DAY_START_TIME;
    }

    private void Start()
    {
        on_changing_time_args = new GlobalEvents.OnChangingTimeArgs(time_in_minutes);
        GlobalEvents.FireOnChangingTime(this, on_changing_time_args);
    }

    private void Update()
    {
        time_passed_since_last_added_minute += Time.deltaTime;

        if(time_passed_since_last_added_minute > FREQUENCY_OF_TIME_FLOW_IN_SECONDS)
        {
            time_in_minutes++;
            time_passed_since_last_added_minute = 0.0f;

            on_changing_time_args = new GlobalEvents.OnChangingTimeArgs(time_in_minutes);
            GlobalEvents.FireOnChangingTime(this, on_changing_time_args);
        }

        if(IsDayEndTime())
        {
            Debug.Log("End of day, killing prisoners not in cells");
            //if player is not in his room, kill player and return him to hit room
            ResetDayTime();
        }
    }

    private bool IsDayEndTime()
    {
        return time_in_minutes == DAY_END_TIME;
    }

    private void ResetDayTime()
    {
        time_in_minutes = DAY_START_TIME;
    }
}
