using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Zarz�dza interfejsem u�ytkownika gry w Blackjacka.
/// Obs�uguje przyciski, suwaki oraz wy�wietlanie wynik�w.
/// </summary>
public class BlackJackUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button draw_button;
    [SerializeField] private Button stay_button;
    [SerializeField] private Button start_game_button;

    [Header("Sliders")]
    [SerializeField] private Slider bid_slider;

    [Header("Scripts")]
    [SerializeField] private BlackJackController black_jack_controller;
    [SerializeField] private InventoryManager inventory_manager;

    [Header("PointsUI")]
    [SerializeField] private TextMeshProUGUI player_wins;
    [SerializeField] private TextMeshProUGUI player_losses;
    [SerializeField] private TextMeshProUGUI current_player_points;
    [SerializeField] private TextMeshProUGUI current_computer_points;
    [SerializeField] private TextMeshProUGUI current_bid;
    [SerializeField] private TextMeshProUGUI round_winner;

    [Header("Game Objects")]
    [SerializeField] private GameObject bid_menu;
    [SerializeField] private GameObject in_game_ui;

    private const string PLAYER_WINS_TEXT = "Wygranych: ";
    private const string PLAYER_LOSSES_TEXT = "Przegranych: ";
    private const string CURRENT_PLAYER_POINTS_TEXT = "Twoje Punkty: ";
    private const string CURRENT_COMPUTER_POINTS = "Punkty Dziada: ";
    private const string PLAYER_WON_ROUND_TEXT = "Wygra�e� rund�!";
    private const string COMP_WON_ROUND_TEXT = "Dziad wygra� rund�!";
    private const string ROUND_DREW = "Runda remisowa!";

    /// <summary>
    /// Inicjalizuje UI i ustawia pocz�tkowe warto�ci oraz eventy dla przycisk�w.
    /// </summary>
    private void Start()
    {
        if (black_jack_controller.GetIsQuest())
        {
            bid_menu.SetActive(false);
            in_game_ui.SetActive(true);
        }

        draw_button.onClick.AddListener(OnDrawButtonClick);
        stay_button.onClick.AddListener(OnStayButtonClick);
        start_game_button.onClick.AddListener(OnStartGameButtonClick);

        bid_slider.onValueChanged.AddListener(SetBidGold);
        round_winner.gameObject.SetActive(false);

        bid_slider.minValue = 0;
        bid_slider.maxValue = inventory_manager.GetGoldAmount();
        GlobalEvents.FireOnAnyUIOpen(this);
    }

    /// <summary>
    /// Aktualizuje interfejs u�ytkownika w ka�dej klatce.
    /// Ustawia interaktywno�� przycisk�w oraz aktualizuje warto�ci tekstowe.
    /// </summary>
    private void Update()
    {
        if (!black_jack_controller.GetCanPressButtons())
        {
            draw_button.interactable = false;
            stay_button.interactable = false;

            draw_button.image.color = Color.gray;
            stay_button.image.color = Color.gray;
        }
        else
        {
            draw_button.interactable = true;
            stay_button.interactable = true;

            draw_button.image.color = Color.white;
            stay_button.image.color = Color.white;
        }

        current_bid.text = ((int)bid_slider.value).ToString();

        player_wins.text = PLAYER_WINS_TEXT + black_jack_controller.GetPlayerWins().ToString();
        player_losses.text = PLAYER_LOSSES_TEXT + black_jack_controller.GetPlayerLosses().ToString();
        current_player_points.text = CURRENT_PLAYER_POINTS_TEXT + black_jack_controller.GetCurrentPlayerPoints().ToString();
        current_computer_points.text = CURRENT_COMPUTER_POINTS + black_jack_controller.GetCurrentComputerPoints().ToString();
    }

    /// <summary>
    /// Wywo�ywane podczas zamykania UI, informuje system o jego zamkni�ciu.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.FireOnAnyUIClose(this);
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Dobierz kart�".
    /// </summary>
    private void OnDrawButtonClick()
    {
        black_jack_controller.DrawPlayerCard();
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Zosta�", ko�czy tur� gracza.
    /// </summary>
    private void OnStayButtonClick()
    {
        black_jack_controller.SetCanPressButtons(false);
        black_jack_controller.SetIsPlayersTurn(false);
    }

    /// <summary>
    /// Ustawia warto�� zak�adu na podstawie pozycji suwaka.
    /// </summary>
    private void SetBidGold(float value)
    {
        black_jack_controller.SetBidMoney((int)value);
    }

    /// <summary>
    /// Obs�uguje klikni�cie przycisku "Rozpocznij gr�".
    /// </summary>
    private void OnStartGameButtonClick()
    {
        in_game_ui.gameObject.SetActive(true);
        bid_menu.gameObject.SetActive(false);
        black_jack_controller.StartGame();
    }

    /// <summary>
    /// Wy�wietla zwyci�zc� rundy na ekranie.
    /// </summary>
    /// <param name="did_player_win">Czy gracz wygra� rund�?</param>
    public void ShowRoundWinner(bool did_player_win)
    {
        round_winner.gameObject.SetActive(true);

        if (black_jack_controller.GetIsRoundDrew())
        {
            round_winner.text = ROUND_DREW;
        }
        else if (did_player_win)
        {
            round_winner.text = PLAYER_WON_ROUND_TEXT;
        }
        else
        {
            round_winner.text = COMP_WON_ROUND_TEXT;
        }
    }

    /// <summary>
    /// Ukrywa informacj� o zwyci�zcy rundy.
    /// </summary>
    public void HideRoundWinner()
    {
        round_winner.gameObject.SetActive(false);
    }
}
