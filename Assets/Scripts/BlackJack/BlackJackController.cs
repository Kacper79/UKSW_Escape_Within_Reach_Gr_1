using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class BlackJackController : MonoBehaviour
{
    [SerializeField] private GameObject card_prefab;
    [SerializeField] private BlackJackUI black_jack_ui;

    [SerializeField] private Transform place_to_tp_player_to_when_starting_game;

    [SerializeField] private Transform players_hand_transform;
    [SerializeField] private Transform computers_hand_transform;

    private GameObject player_go;

    private List<Card> all_cards_list = new List<Card>();
    private List<Card> players_cards = new List<Card>();
    private List<Card> opponents_cards = new List<Card>();

    private int player_wins = 0;
    private int player_loses = 0;

    private int current_player_points = 0;
    private int current_opponent_points = 0;

    private bool is_players_turn = true;
    private bool can_press_buttons = true;
    private bool is_hidden_card_shown = false;
    private bool is_round_over = false;
    private bool has_player_overdrown = false;
    private bool is_hidden_card_drown = false;
    private bool is_quest_game;

    private Vector3 card_spawn_position;

    private const float CARD_SPAWN_Y = 1.19f;
    private const float DROWN_CARDS_OFFSET_X = 0.1f;

    private void Awake()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }
    private void OnEnable()
    {
        card_spawn_position = this.transform.position;
        card_spawn_position.y = CARD_SPAWN_Y;
        CreateCards();

        GlobalEvents.OnStartingBlackJackGameForPickaxe += StartBlackJackGameForPickaxe;
        GlobalEvents.OnStartingBlackJackGameForMoney += StartBlackJackGameForMoney;
        GlobalEvents.OnStartingBlackJackGameForPickaxe += EnableBlakjackUI;
        GlobalEvents.OnStartingBlackJackGameForMoney += EnableBlakjackUI;

        GlobalEvents.OnEndingBlackjackGame += DisableBlackjackUI;

    }
    private void OnDisable()
    {
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= StartBlackJackGameForPickaxe;
        GlobalEvents.OnStartingBlackJackGameForMoney -= StartBlackJackGameForMoney;
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= EnableBlakjackUI;
        GlobalEvents.OnStartingBlackJackGameForMoney -= EnableBlakjackUI;

        GlobalEvents.OnEndingBlackjackGame -= DisableBlackjackUI;
    }

    private void DisableBlackjackUI(object sender, EventArgs e)
    {
        black_jack_ui.gameObject.SetActive(false);
    }

    private void EnableBlakjackUI(object sender, System.EventArgs e)
    {
        black_jack_ui.gameObject.SetActive(true);
    }

    private void DisableBlackjackUI()
    {
        black_jack_ui.gameObject.SetActive(false);
    }

    private void StartBlackJackGameForPickaxe(object sender, EventArgs e)
    {
        is_quest_game = true;

        GlobalEvents.FireOnStartingTransition(this, new(0.3f));
        Invoke(nameof(TeleportPlayerToTable), 0.3f);
        StartCoroutine(BlackJackGame());
    }

    private void StartBlackJackGameForMoney(object sender, EventArgs e)
    {
        is_quest_game = false;

        GlobalEvents.FireOnStartingTransition(this, new(0.3f));
        Invoke(nameof(TeleportPlayerToTable), 0.3f);
        StartCoroutine(BlackJackGame());
    }
    IEnumerator BlackJackGame()
    {
        Cursor.lockState = CursorLockMode.None;

        while (!(player_wins == 3 || player_loses == 3))
        {
            StartNewRound();
            while(!is_round_over)
            {
                if (is_players_turn)
                {
                    //playersturn
                    can_press_buttons = true;
                }
                else
                {
                    if (!is_hidden_card_shown)
                    {
                        ShowHiddenCard();
                        is_hidden_card_shown = true;
                        current_opponent_points = CountHandPoints(opponents_cards);
                    }
                    yield return new WaitForSeconds(1.5f);
                    can_press_buttons = false;
                    ComputersTurn();
                }

                yield return null;
            }

            yield return new WaitForSeconds(3f);

            DetermineTheWinnerOfTheRound();
            ResetAfterRound();
        }
        GlobalEvents.FireOnEndingBlackjackGame(this);
        if (player_wins == 3 && is_quest_game)
        {
            QuestManager.Instance.MarkQuestCompleted(1);
        }
        else if(player_wins == 3 && is_quest_game == false)
        {
            GlobalEvents.FireOnLosingMoneyInABlackjackGame(this, new(300));
        }
        else if (is_quest_game == true)
        {
            GlobalEvents.FireOnLosingMoneyInABlackjackGame(this, new(-500));
        }
        else
        {
            GlobalEvents.FireOnLosingMoneyInABlackjackGame(this, new(-300));
        }
        ResetGame();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void CreateCards()
    {
        for (int i = 0; i <= 12; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                GameObject new_card = Instantiate(card_prefab, card_spawn_position, Quaternion.identity, this.transform);
                new_card.transform.eulerAngles = new(-90, 180, 0);
                Card new_card_script = new_card.GetComponent<Card>();
                new_card_script.InitializeValues(i, j);
                all_cards_list.Add(new_card_script);
            }
        }
    }

    public void DrawPlayerCard()
    {
        Card drown_card = all_cards_list[UnityEngine.Random.Range(0, all_cards_list.Count)];
        all_cards_list.Remove(drown_card);
        players_cards.Add(drown_card);
        drown_card.card_graphic.ShowObverse();
        drown_card.transform.position = players_hand_transform.position;
        drown_card.transform.position += new Vector3((players_cards.Count - 1)*DROWN_CARDS_OFFSET_X, 0f, 0f);

        current_player_points = CountHandPoints(players_cards);

        if(CountHandPoints(players_cards) > 21)
        {
            has_player_overdrown = true;
            is_round_over = true;
            can_press_buttons = false;
        }
    }

    private int CountHandPoints(List<Card> cards_list)
    {
        int points = 0;
        List<Card> temp = new ();

        foreach (Card card in cards_list)
        {
            int card_value = card.GetValue() + 1;

            if (card_value > 1 && card_value < 10)
            {
                points += card_value;
            }
            else if (card_value > 9 && card_value < 14)
            {
                points += 10;
            }
            else
            {
                temp.Add(card);
            }
        }

        if(temp.Count == 0)
        {
            return points;
        }
        else if(points + 11 + temp.Count - 1 > 21)
        {
            points += temp.Count;
        }
        else
        {
            points += temp.Count + 10;
        }

        return points;
    }

    private void DetermineTheWinnerOfTheRound()
    {
        Debug.Log(CountHandPoints(players_cards));
        Debug.Log(CountHandPoints(opponents_cards));

        if(has_player_overdrown)
        {
            player_loses++;
        }
        else if(CountHandPoints(opponents_cards) > 21)
        {
            player_wins++;
        }
        else if(CountHandPoints(players_cards) > CountHandPoints(opponents_cards))
        {
            player_wins++;
        }
        else if(CountHandPoints(players_cards) < CountHandPoints(opponents_cards))
        {
            player_loses++;
        }
        else
        {
            Debug.Log("Runda remisowa");
        }
        Debug.Log(player_loses);
        Debug.Log(player_wins);
        Debug.Log("Koniec rundy");
    }

    private void ComputersTurn()
    {
        int computer_points = CountHandPoints(opponents_cards);

        if(computer_points < 17)
        {
            DrawComputerCard();
        }
        else
        {
            is_round_over = true;
        }

    }

    private void DrawComputerCard()
    {
        Card drown_card = all_cards_list[UnityEngine.Random.Range(0, all_cards_list.Count)];
        all_cards_list.Remove(drown_card);
        opponents_cards.Add(drown_card);
        drown_card.card_graphic.ShowReverse();
        drown_card.transform.position = computers_hand_transform.position;
        drown_card.transform.position += new Vector3((opponents_cards.Count - 1) * DROWN_CARDS_OFFSET_X, 0f, 0f);

        if (!is_hidden_card_drown)
        {
            drown_card.card_graphic.ShowReverse();
        }
        else
        {
            drown_card.card_graphic.ShowObverse();
            
            if(drown_card.GetValue() == 0)
            {
                current_opponent_points += 11;
            }
            else if (drown_card.GetValue() > 9 && drown_card.GetValue() < 13)
            {
                current_opponent_points += 10;
            }
            else
            {
                current_opponent_points = drown_card.GetValue() + 1;
            }
        }
    }

    private void ShowHiddenCard()
    {
        opponents_cards[0].card_graphic.ShowObverse();
    }

    private void ResetAfterRound()
    {
        is_hidden_card_shown = false;
        is_round_over = false;
        has_player_overdrown = false;
        is_players_turn = true;
        is_hidden_card_drown = false;
        can_press_buttons = true;

        current_opponent_points = 0;
        current_player_points = 0;

        foreach(Card card in players_cards)
        {
            all_cards_list.Add(card);
            card.transform.position = card_spawn_position;
            card.card_graphic.ShowReverse();
        }

        foreach(Card card in opponents_cards)
        {
            all_cards_list.Add(card);
            card.transform.position = card_spawn_position;
            card.card_graphic.ShowReverse();
        }

        players_cards.Clear();
        opponents_cards.Clear();
    }

    private void StartNewRound()
    {
        for(int i = 0; i < 2; i++)
        {
            DrawPlayerCard();
            DrawComputerCard();
            is_hidden_card_drown = true;
        }
    }

    private void ResetGame()
    {
        player_wins = 0;
        player_loses = 0;
    }
    private void TeleportPlayerToTable()
    {
        player_go.transform.position = place_to_tp_player_to_when_starting_game.position;
        Physics.SyncTransforms();
    }
    public bool GetCanPressButtons()
    {
        return can_press_buttons;
    }
    public void SetCanPressButtons(bool b)
    {
        can_press_buttons = b;
    }

    public void SetIsPlayersTurn(bool b)
    {
        is_players_turn = b;
    }

    public int GetPlayerWins()
    {
        return player_wins;
    }

    public int GetPlayerLosses()
    {
        return player_loses;
    }

    public int GetCurrentPlayerPoints()
    {
        return current_player_points;
    }

    public int GetCurrentComputerPoints()
    {
        return current_opponent_points;
    }
}
