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
    [SerializeField] private TextMeshProUGUI current_hp_display;
    [SerializeField] private TextMeshProUGUI gold_display;

    [SerializeField] private ProgressBar stress_progress_bar;
    [SerializeField] private ProgressBar hp_bar;

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

        GlobalEvents.OnAnyUIOpen += DisableUI;
        GlobalEvents.OnAnyUIClose += EnableUI;
    }

    private void Update()
    {
        stress_progress_bar.SetProgressBarValues(player_go.GetComponent<PlayerStress>().stressLevel, player_go.GetComponent<PlayerStress>().maxStressLevel);
        hp_bar.SetProgressBarValues(player_go.GetComponent<PlayerAttackAbsorber>().GetHp(), player_go.GetComponent<PlayerAttackAbsorber>().MaxHP);
        gold_display.text = player_go.GetComponent<InventoryManager>().GetGoldAmount().ToString();
        DisplayCurrentHp();
    }

    private void OnDisable()
    {
        GlobalEvents.OnChangingTime -= ChangeTimeLabel;
        GlobalEvents.OnTimeStart -= EnableUI;
        GlobalEvents.OnTimeStop -= DisableUI;

        GlobalEvents.OnAnyUIOpen -= DisableUI;
        GlobalEvents.OnAnyUIClose -= EnableUI;
    }

    private void EnableUI(object sender, System.EventArgs e)
    {
        time_of_day_display.enabled = true;
        stress_progress_bar.gameObject.SetActive(true);
        hp_bar.gameObject.SetActive(true);
        current_hp_display.gameObject.SetActive(true);
        gold_display.gameObject.SetActive(true);
    }

    private void DisableUI(object sender, System.EventArgs e)
    {
        time_of_day_display.enabled = false;
        stress_progress_bar.gameObject.SetActive(false);
        hp_bar.gameObject.SetActive(false);
        current_hp_display.gameObject.SetActive(false);
        gold_display.gameObject.SetActive(false);
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
