using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GlobalEvents
{
    public static event EventHandler OnDestroingRock;
    public static event EventHandler OnNotLookingOnInteractable;
    public static event EventHandler OnChangingTime;
    public static event EventHandler OnReadingPage;
    public static event EventHandler OnStoppingReadingPage;

    public static event EventHandler OnStartingDialogue;
    public static event EventHandler OnEndingDialogue;
    public static event EventHandler<OnChoosingCertainDialogueOptionEventArgs> OnChoosingCertainDialogueOption;
    public static event EventHandler<OnLookingForDialogueListWithGivenIDEventArgs> OnLookingForDialogueListWithGivenID;
    public static event EventHandler<CallbackForOnLookingForDialogueListWithGivenIDEventArgs> CallbackForOnLookingForDialogueListWithGivenID;

    public static event EventHandler OnBeatingEnemyInATournament;

    #region dialogue_related_events
    private static Dictionary<DialogueNodeSO.DialogueEvent, EventHandler> dialogue_event_dict;

    public static event EventHandler OnStartingFightTournament
    {
        add { AddToDictionary(DialogueNodeSO.DialogueEvent.StartFightingTournament, value); }
        remove { RemoveFromDictionary(DialogueNodeSO.DialogueEvent.StartFightingTournament, value); }
    }

    public static void FireCertainDialogueEvent(object sender, DialogueNodeSO.DialogueEvent dialogue_event)
    {
        dialogue_event_dict[dialogue_event]?.Invoke(sender, EventArgs.Empty);
    }

    static GlobalEvents()
    {
        dialogue_event_dict = new();
    }

    private static void AddToDictionary(DialogueNodeSO.DialogueEvent dialogueEvent, EventHandler handler)
    {
        if (!dialogue_event_dict.ContainsKey(dialogueEvent))
        {
            dialogue_event_dict[dialogueEvent] = null;
        }

        dialogue_event_dict[dialogueEvent] += handler;
    }

    private static void RemoveFromDictionary(DialogueNodeSO.DialogueEvent dialogueEvent, EventHandler handler)
    {
        if (dialogue_event_dict.ContainsKey(dialogueEvent))
        {
            dialogue_event_dict[dialogueEvent] -= handler;
        }
    }
    #endregion

    public class CallbackForOnLookingForDialogueListWithGivenIDEventArgs : EventArgs
    {
        public List<DialogueNodeSO> list_of_options;

        public CallbackForOnLookingForDialogueListWithGivenIDEventArgs(List<DialogueNodeSO> list_of_options)
        {
            this.list_of_options = list_of_options;
        }
    }

    public class OnLookingForDialogueListWithGivenIDEventArgs : EventArgs
    {
        public string given_id;

        public OnLookingForDialogueListWithGivenIDEventArgs(string given_id)
        {
            this.given_id = given_id;
        }
    }

    public class OnChoosingCertainDialogueOptionEventArgs : EventArgs
    {
        public string choosen_option_id;

        public OnChoosingCertainDialogueOptionEventArgs(string choosen_option_id)
        {
            this.choosen_option_id = choosen_option_id;
        }
    }

    public class OnChangingTimeArgs : EventArgs
    {
        public int minutes;

        public OnChangingTimeArgs(int minutes_)
        {
            minutes = minutes_;
        }
    }

    public static void FireOnDestroingRock(object sender)
    {
        OnDestroingRock?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnNotLookingOnInteractable(object sender)
    {
        OnNotLookingOnInteractable?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnChangingTime(object sender, OnChangingTimeArgs args)
    {
        OnChangingTime?.Invoke(sender, args);
    }

    public static void FireOnReadingPage(object sender)
    {
        OnReadingPage?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnStoppingReadingPage(object sender)
    {
        OnStoppingReadingPage?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnStartingDialogue(object sender)
    {
        OnStartingDialogue?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnEndingDialogue(object sender)
    {
        OnEndingDialogue?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnChoosingCertainDialogueOption(object sender, OnChoosingCertainDialogueOptionEventArgs args)
    {
        OnChoosingCertainDialogueOption?.Invoke(sender, args);
    }

    public static void FireOnLookingForDialogueListWithGivenID(object sender, OnLookingForDialogueListWithGivenIDEventArgs args)
    {
        OnLookingForDialogueListWithGivenID?.Invoke(sender, args);
    }

    public static void FireCallbackForOnLookingForDialogueListWithGivenID(object sender, CallbackForOnLookingForDialogueListWithGivenIDEventArgs args)
    {
        CallbackForOnLookingForDialogueListWithGivenID?.Invoke(sender, args);
    }

    public static void FireOnBeatingEnemyInATournament(object sender)
    {
        OnBeatingEnemyInATournament?.Invoke(sender, EventArgs.Empty);
    }
}
