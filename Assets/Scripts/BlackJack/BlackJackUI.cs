using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackJackUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button draw;
    [SerializeField] private Button stay;

    [Header("Scripts")]
    [SerializeField] private BlackJackController black_jack_controller;

    [Header("PointsUI")]
    [SerializeField] private TextMeshProUGUI player_wins;
    [SerializeField] private TextMeshProUGUI player_losses;
    [SerializeField] private TextMeshProUGUI current_player_points;
    [SerializeField] private TextMeshProUGUI current_computer_points;

    private const string PLAYER_WINS_TEXT = "Wygranych: ";
    private const string PLAYER_LOSSES_TEXT = "Przegranych: ";
    private const string CURRENT_PLAYER_POINTS_TEXT = "Twoje Punkty: ";
    private const string CURRENT_COMPUTER_POINTS = "Punkty Dziada: ";


    private void Start()
    {
        draw.onClick.AddListener(OnDrawButtonClick);
        stay.onClick.AddListener(OnStayButtonClick);
        GlobalEvents.FireOnAnyUIOpen(this);
    }

    private void Update()
    {
        if(!black_jack_controller.GetCanPressButtons())
        {
            draw.interactable = false;
            stay.interactable = false;

            draw.image.color = Color.gray;
            stay.image.color = Color.gray;
        }
        else
        {
            draw.interactable = true;
            stay.interactable = true;

            draw.image.color = Color.white;
            stay.image.color = Color.white;
        }

        player_wins.text = PLAYER_WINS_TEXT + black_jack_controller.GetPlayerWins().ToString();
        player_losses.text = PLAYER_LOSSES_TEXT + black_jack_controller.GetPlayerLosses().ToString();
        current_player_points.text = CURRENT_PLAYER_POINTS_TEXT + black_jack_controller.GetCurrentPlayerPoints().ToString();
        current_computer_points.text = CURRENT_COMPUTER_POINTS + black_jack_controller.GetCurrentComputerPoints().ToString();

    }

    private void OnDisable()
    {
        GlobalEvents.FireOnAnyUIClose(this);
    }

    private void OnDrawButtonClick()
    {
        black_jack_controller.DrawPlayerCard();
    }

    private void OnStayButtonClick()
    {
        black_jack_controller.SetCanPressButtons(false);
        black_jack_controller.SetIsPlayersTurn(false);
    }
}
