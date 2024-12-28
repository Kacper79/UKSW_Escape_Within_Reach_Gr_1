using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEventProcManager : MonoBehaviour
{
    public void OnFightTournamentWin()
    {
        GlobalEvents.FireOnFinishingTournament(this);
        GlobalEvents.FireOnMakingGivenDialogueOptionAvailableOrUnavailable(this, new("b100648026c8423084914450f3f01768", false));
    }
}
