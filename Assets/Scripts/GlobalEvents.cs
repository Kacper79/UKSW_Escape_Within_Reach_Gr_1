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
    public static event EventHandler<OnMakingGivenDialogueOptionAvailableOrUnavailableEventArgs> OnMakingGivenDialogueOptionAvailableOrUnavailable;

    public static event EventHandler OnBeatingEnemyInATournament;

    public static event EventHandler<OnStartingTransitionEventArgs> OnStartingTransition;
    public static event EventHandler OnEndingTransition;

    public static event EventHandler OnFinishingTournament;

    public static event EventHandler OnAnyUIOpen;
    public static event EventHandler OnAnyUIClose;
    public static event EventHandler OnPauseGame;
    public static event EventHandler OnResumeGame;
    public static event EventHandler<OnPickUpItemEventArgs> OnPickUpItem;
    public static event EventHandler OnInventoryOpen;
    public static event EventHandler<OnInventoryOpenCallBackEventArgs> OnInventoryOpenCallBack;

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

    public class OnMakingGivenDialogueOptionAvailableOrUnavailableEventArgs : EventArgs
    {
        public string dialogue_id;
        public bool new_bool_value;

        public OnMakingGivenDialogueOptionAvailableOrUnavailableEventArgs(string dialogue_id, bool new_bool_value)
        {
            this.dialogue_id = dialogue_id;
            this.new_bool_value = new_bool_value;
        }
    }

    public class OnStartingTransitionEventArgs : EventArgs
    {
        public float time_after_the_transition_ends;

        public OnStartingTransitionEventArgs(float time_after_the_transition_ends)
        {
            this.time_after_the_transition_ends = time_after_the_transition_ends;
        }
    }

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

    public static void FireOnStartingTransition(object sender, OnStartingTransitionEventArgs args)
    {
        OnStartingTransition?.Invoke(sender, args);
    }

    public static void FireOnEndingTransition(object sender)
    {
        OnEndingTransition?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnFinishingTournament(object sender)
    {
        OnFinishingTournament?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnMakingGivenDialogueOptionAvailableOrUnavailable(object sender, OnMakingGivenDialogueOptionAvailableOrUnavailableEventArgs args)
    {
        OnMakingGivenDialogueOptionAvailableOrUnavailable?.Invoke(sender, args);
    }

    public class OnPickUpItemEventArgs : EventArgs
    {
        public Item item;

        public OnPickUpItemEventArgs(Item item_) 
        {
            item = item_;
        }
    }

    public class OnInventoryOpenCallBackEventArgs : EventArgs
    {
        public List<Item> plot_items_list;
        public List<Item> other_items_list;
        public Dictionary<string, int> item_amount;
        public int gold_amount;

        public OnInventoryOpenCallBackEventArgs(List<Item> item_, List<Item> other_items_list_, Dictionary<string, int> item_amount_)
        {
            plot_items_list = item_;
            other_items_list = other_items_list_;
            item_amount = item_amount_;
        }
    }

    public static void FireOnAnyUIOpen(object sender)
    {
        OnAnyUIOpen?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnAnyUIClose(object sender)
    {
        OnAnyUIClose?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnPauseGame(object sender)
    {
        OnPauseGame?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnResumeGame(object sender)
    {
        OnResumeGame?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnPickUpItem(object sender, OnPickUpItemEventArgs args)
    {
        OnPickUpItem?.Invoke(sender, args);
    }

    public static void FireOnInventoryOpen(object sender)
    {
        OnInventoryOpen?.Invoke(sender, EventArgs.Empty);
    }

    public static void FireOnInventoryOpenCallBack(object sender, OnInventoryOpenCallBackEventArgs args)
    {
        OnInventoryOpenCallBack?.Invoke(sender, args);
    }
}