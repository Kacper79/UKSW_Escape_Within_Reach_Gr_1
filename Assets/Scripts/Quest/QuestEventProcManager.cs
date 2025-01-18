using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpalaj¹ca ró¿ne eventy po skoñczonych danych questach
/// </summary>
public class QuestEventProcManager : MonoBehaviour
{
    private const string PICKAXE_ITEM_NAME = "Kilof";

    public void OnFightTournamentWin()
    {
        GlobalEvents.FireOnFinishingTournament(this);
        GlobalEvents.FireOnMakingGivenDialogueOptionAvailableOrUnavailable(this, new("b100648026c8423084914450f3f01768", false));
    }

    public void OnWinningPickaxeInABlakjackGame()
    {
        GlobalEvents.FireOnWinningPickaxeInABlackjackGame(this, new(PICKAXE_ITEM_NAME));
        GlobalEvents.FireOnMakingGivenDialogueOptionAvailableOrUnavailable(this, new("18798bc42c614ff49c606dd3b4bd4418", false));
    }
}
