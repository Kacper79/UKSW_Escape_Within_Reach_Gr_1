using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJackController : MonoBehaviour
{
    /// <summary>
    /// Prefab karty, kt�ry jest instantiowany podczas gry.
    /// </summary>
    [SerializeField] private GameObject card_prefab;

    /// <summary>
    /// Interfejs u�ytkownika dla gry w BlackJack.
    /// </summary>
    [SerializeField] private BlackJackUI black_jack_ui;

    /// <summary>
    /// Miejsce, do kt�rego zostanie teleportowany gracz podczas rozpoczynania gry.
    /// </summary>
    [SerializeField] private Transform place_to_tp_player_to_when_starting_game;

    /// <summary>
    /// Transformacja, do kt�rej b�d� dodawane karty gracza.
    /// </summary>
    [SerializeField] private Transform players_hand_transform;

    /// <summary>
    /// Transformacja, do kt�rej b�d� dodawane karty przeciwnika.
    /// </summary>
    [SerializeField] private Transform computers_hand_transform;

    /// <summary>
    /// Obiekt gracza.
    /// </summary>
    private GameObject player_go;

    private List<Card> all_cards_list = new List<Card>();
    private List<Card> players_cards = new List<Card>();
    private List<Card> opponents_cards = new List<Card>();

    private int player_wins = 0;
    private int player_loses = 0;

    private int bid_money = 0;

    private int current_player_points = 0;
    private int current_opponent_points = 0;

    private bool is_players_turn = true;
    private bool can_press_buttons = true;
    private bool is_hidden_card_shown = false;
    private bool is_round_over = false;
    private bool has_player_overdrown = false;
    private bool is_hidden_card_drown = false;
    private bool is_quest_game;
    private bool is_round_drew;

    private Vector3 card_spawn_position;

    private const float CARD_SPAWN_Y = 1.19f;
    private const float DROWN_CARDS_OFFSET_X = 0.1f;
    private const bool PLAYER_WON_ROUND = true;
    private const bool PLAYER_LOST_ROUND = false;

    /// <summary>
    /// Inicjalizuje komponenty i pobiera obiekt gracza.
    /// </summary>
    private void Awake()
    {
        player_go = FindAnyObjectByType<PlayerInputController>().gameObject;
    }

    /// <summary>
    /// Inicjalizuje ustawienia przed rozpocz�ciem gry, tworzy karty i ustawia nas�uchiwanie na zdarzenia globalne.
    /// </summary>
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

    /// <summary>
    /// Usuwa nas�uchiwanie na zdarzenia globalne po zako�czeniu gry.
    /// </summary>
    private void OnDisable()
    {
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= StartBlackJackGameForPickaxe;
        GlobalEvents.OnStartingBlackJackGameForMoney -= StartBlackJackGameForMoney;
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= EnableBlakjackUI;
        GlobalEvents.OnStartingBlackJackGameForMoney -= EnableBlakjackUI;

        GlobalEvents.OnEndingBlackjackGame -= DisableBlackjackUI;
    }

    /// <summary>
    /// Ukrywa interfejs gry BlackJack po jej zako�czeniu.
    /// </summary>
    private void DisableBlackjackUI(object sender, EventArgs e)
    {
        black_jack_ui.gameObject.SetActive(false);
    }

    /// <summary>
    /// W��cza interfejs gry BlackJack podczas jej rozpocz�cia.
    /// </summary>
    private void EnableBlakjackUI(object sender, System.EventArgs e)
    {
        black_jack_ui.gameObject.SetActive(true);
    }

    /// <summary>
    /// Uruchamia gr� BlackJack dla zadania zwi�zanego z przedmiotem.
    /// </summary>
    private void StartBlackJackGameForPickaxe(object sender, EventArgs e)
    {
        is_quest_game = true;
        Cursor.lockState = CursorLockMode.None;

        GlobalEvents.FireOnTimeStop(this);
        GlobalEvents.FireOnStartingTransition(this, new(0.3f));
        Invoke(nameof(TeleportPlayerToTable), 0.3f);
        StartGame();
    }

    /// <summary>
    /// Uruchamia gr� BlackJack dla zak�ad�w pieni�nych.
    /// </summary>
    private void StartBlackJackGameForMoney(object sender, EventArgs e)
    {
        is_quest_game = false;
        Cursor.lockState = CursorLockMode.None;

        GlobalEvents.FireOnTimeStop(this);
        GlobalEvents.FireOnStartingTransition(this, new(0.3f));
        Invoke(nameof(TeleportPlayerToTable), 0.3f);
    }

    /// <summary>
    /// Logika gry BlackJack. Rozpoczyna now� rund�, przetwarza tury gracza i przeciwnika.
    /// </summary>
    IEnumerator BlackJackGame()
    {
        while (!(player_wins == 3 || player_loses == 3))
        {
            StartNewRound();
            while (!is_round_over)
            {
                if (is_players_turn)
                {
                    // Ruch gracza
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
                    yield return new WaitForSeconds(1f);
                    can_press_buttons = false;
                    ComputersTurn();
                    current_opponent_points = CountHandPoints(opponents_cards);
                }

                yield return null;
            }

            DetermineTheWinnerOfTheRound();
            yield return new WaitForSeconds(2.5f);
            black_jack_ui.HideRoundWinner();
            ResetAfterRound();
        }

        // Zako�czenie gry
        GlobalEvents.FireOnEndingBlackjackGame(this);
        GlobalEvents.FireOnTimeStart(this);
        if (player_wins == 3 && is_quest_game)
        {
            QuestManager.Instance.MarkQuestCompleted(1);
        }
        else if (player_wins == 3 && is_quest_game == false)
        {
            GlobalEvents.FireOnLosingMoneyInABlackjackGame(this, new(bid_money));
        }
        else if (is_quest_game == true)
        {
            GlobalEvents.FireOnLosingMoneyInABlackjackGame(this, new(-500));
        }
        else
        {
            GlobalEvents.FireOnLosingMoneyInABlackjackGame(this, new(-bid_money));
        }
        ResetGame();
        Cursor.lockState = CursorLockMode.Locked;
    }


    /// <summary>
    /// Tworzy wszystkie karty w grze, instancjonuj�c prefabrykaty kart i inicjalizuj�c warto�ci dla ka�dej karty.
    /// </summary>
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

    /// <summary>
    /// Losuje kart� dla gracza, dodaje j� do r�ki gracza i sprawdza, czy gracz przekroczy� 21 punkt�w.
    /// </summary>
    public void DrawPlayerCard()
    {
        Card drown_card = all_cards_list[UnityEngine.Random.Range(0, all_cards_list.Count)];
        all_cards_list.Remove(drown_card);
        players_cards.Add(drown_card);
        drown_card.card_graphic.ShowObverse();
        drown_card.transform.position = players_hand_transform.position;
        drown_card.transform.position += new Vector3((players_cards.Count - 1) * DROWN_CARDS_OFFSET_X, 0f, 0f);

        current_player_points = CountHandPoints(players_cards);

        if (current_player_points > 21)
        {
            has_player_overdrown = true;
            is_round_over = true;
            can_press_buttons = false;
        }
    }

    /// <summary>
    /// Zlicza punkty na podstawie kart w r�ce, uwzgl�dniaj�c specjalne zasady dla As�w.
    /// </summary>
    private int CountHandPoints(List<Card> cards_list)
    {
        int points = 0;
        List<Card> temp = new();

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

        if (temp.Count == 0)
        {
            return points;
        }
        else if (points + 11 + temp.Count - 1 > 21)
        {
            points += temp.Count;
        }
        else
        {
            points += temp.Count + 10;
        }

        return points;
    }

    /// <summary>
    /// Okre�la zwyci�zc� rundy na podstawie punkt�w gracza i przeciwnika.
    /// </summary>
    private void DetermineTheWinnerOfTheRound()
    {
        Debug.Log(CountHandPoints(players_cards));
        Debug.Log(CountHandPoints(opponents_cards));

        if (has_player_overdrown)
        {
            player_loses++;
            black_jack_ui.ShowRoundWinner(PLAYER_LOST_ROUND);
        }
        else if (CountHandPoints(opponents_cards) > 21)
        {
            player_wins++;
            black_jack_ui.ShowRoundWinner(PLAYER_WON_ROUND);
        }
        else if (CountHandPoints(players_cards) > CountHandPoints(opponents_cards))
        {
            player_wins++;
            black_jack_ui.ShowRoundWinner(PLAYER_WON_ROUND);
        }
        else if (CountHandPoints(players_cards) < CountHandPoints(opponents_cards))
        {
            player_loses++;
            black_jack_ui.ShowRoundWinner(PLAYER_LOST_ROUND);
        }
        else
        {
            is_round_drew = true;
        }
    }

    /// <summary>
    /// Okre�la, jak komputer dobiera karty. Komputer dobiera karty, a� osi�gnie co najmniej 17 punkt�w.
    /// </summary>
    private void ComputersTurn()
    {
        int computer_points = CountHandPoints(opponents_cards);

        if (computer_points < 17)
        {
            DrawComputerCard();
        }
        else
        {
            is_round_over = true;
        }
    }

    /// <summary>
    /// Losuje kart� dla komputera, dodaje j� do r�ki i wy�wietla jej grafik�.
    /// </summary>
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

            if (drown_card.GetValue() == 0)
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

    /// <summary>
    /// Odkrywa pierwsz� kart� komputera, kt�ra by�a wcze�niej zakryta.
    /// </summary>
    private void ShowHiddenCard()
    {
        opponents_cards[0].card_graphic.ShowObverse();
    }

    /// <summary>
    /// Resetuje stan gry po zako�czeniu rundy, przygotowuj�c si� do nowej.
    /// </summary>
    private void ResetAfterRound()
    {
        is_hidden_card_shown = false;
        is_round_over = false;
        has_player_overdrown = false;
        is_players_turn = true;
        is_hidden_card_drown = false;
        can_press_buttons = true;
        is_round_drew = false;

        current_opponent_points = 0;
        current_player_points = 0;

        foreach (Card card in players_cards)
        {
            all_cards_list.Add(card);
            card.transform.position = card_spawn_position;
            card.card_graphic.ShowReverse();
        }

        foreach (Card card in opponents_cards)
        {
            all_cards_list.Add(card);
            card.transform.position = card_spawn_position;
            card.card_graphic.ShowReverse();
        }

        players_cards.Clear();
        opponents_cards.Clear();
    }

    /// <summary>
    /// Rozpoczyna now� rund�, losuj�c karty dla gracza i komputera.
    /// </summary>
    private void StartNewRound()
    {
        for (int i = 0; i < 2; i++)
        {
            DrawPlayerCard();
            DrawComputerCard();
            is_hidden_card_drown = true;
        }
    }

    /// <summary>
    /// Resetuje ca�� gr�, ustalaj�c wyniki wygranych i przegranych.
    /// </summary>
    private void ResetGame()
    {
        player_wins = 0;
        player_loses = 0;
    }

    /// <summary>
    /// Uruchamia g��wn� p�tl� gry Blackjack.
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(BlackJackGame());
    }

    /// <summary>
    /// Teleportuje gracza do miejsca, gdzie odbywa si� gra w Blackjacka.
    /// </summary>
    private void TeleportPlayerToTable()
    {
        player_go.transform.position = place_to_tp_player_to_when_starting_game.position;
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Zwraca, czy gracz mo�e obecnie naciska� przyciski w grze.
    /// </summary>
    public bool GetCanPressButtons()
    {
        return can_press_buttons;
    }

    /// <summary>
    /// Ustawia, czy gracz mo�e naciska� przyciski.
    /// </summary>
    public void SetCanPressButtons(bool b)
    {
        can_press_buttons = b;
    }

    /// <summary>
    /// Ustawia, czy to jest kolejka gracza.
    /// </summary>
    public void SetIsPlayersTurn(bool b)
    {
        is_players_turn = b;
    }

    /// <summary>
    /// Zwraca liczb� wygranych gracza.
    /// </summary>
    public int GetPlayerWins()
    {
        return player_wins;
    }

    /// <summary>
    /// Zwraca liczb� przegranych gracza.
    /// </summary>
    public int GetPlayerLosses()
    {
        return player_loses;
    }

    /// <summary>
    /// Zwraca obecne punkty gracza.
    /// </summary>
    public int GetCurrentPlayerPoints()
    {
        return current_player_points;
    }

    /// <summary>
    /// Zwraca obecne punkty komputera.
    /// </summary>
    public int GetCurrentComputerPoints()
    {
        return current_opponent_points;
    }

    /// <summary>
    /// Ustawia warto�� zak�adu gracza.
    /// </summary>
    public void SetBidMoney(int value)
    {
        bid_money = value;
    }

    /// <summary>
    /// Zwraca, czy runda zako�czy�a si� remisem.
    /// </summary>
    public bool GetIsRoundDrew()
    {
        return is_round_drew;
    }

    /// <summary>
    /// Zwraca, czy gra jest cz�ci� zadania.
    /// </summary>
    public bool GetIsQuest()
    {
        return is_quest_game;
    }
}
