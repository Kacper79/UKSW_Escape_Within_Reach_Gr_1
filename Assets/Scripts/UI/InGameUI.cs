using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PlayerRelated;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time_of_day_display;
    [SerializeField] private ProgressBar stress_progress_bar;

    private GameObject player_go;


    private void Start()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }
    private void OnEnable()
    {
        GlobalEvents.OnChangingTime += ChangeTimeLabel;
        GlobalEvents.OnTimeStart += EnableUI;
        GlobalEvents.OnTimeStop += DisableUI;
    }

    private void Update()
    {
        stress_progress_bar.SetProgressBarValues(player_go.GetComponent<PlayerStress>().stressLevel, player_go.GetComponent<PlayerStress>().maxStressLevel);
    }

    private void OnDisable()
    {
        GlobalEvents.OnChangingTime -= ChangeTimeLabel;
        GlobalEvents.OnTimeStart -= EnableUI;
        GlobalEvents.OnTimeStop -= DisableUI;
    }

    private void EnableUI(object sender, System.EventArgs e)
    {
        time_of_day_display.enabled = true;
    }

    private void DisableUI(object sender, System.EventArgs e)
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
