using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time_of_day_display;

    private void OnEnable()
    {
        GlobalEvents.OnChangingTime += ChangeTimeLabel;
    }

    private void OnDisable()
    {
        GlobalEvents.OnChangingTime -= ChangeTimeLabel;
    }

    private void EnableUI()
    {
        time_of_day_display.enabled = true;
    }

    private void DisableUI()
    {
        time_of_day_display.enabled = false;
    }

    private void ChangeTimeLabel(object sender, EventArgs e)
    {
        if(e is GlobalEvents.OnChangingTimeArgs args)
        {
            time_of_day_display.text = ChangeTimeIntoString(args.minutes);
        }
        else
        {
            Debug.Log("Bug in InGameUI, received EventArgs not of type GlobalEvents.OnChangingTimeArgs");
        }
    }

    private string ChangeTimeIntoString(int time_in_minutes)
    {
        TimeSpan time = TimeSpan.FromMinutes(time_in_minutes);
        return time.ToString(@"hh\:mm");
    }
}
