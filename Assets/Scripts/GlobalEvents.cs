using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GlobalEvents
{
    //public static event EventHandler OnDestroingRock;
    public static event EventHandler OnNotLookingOnInteractable;

    //public static void FireOnDestroingRock(object sender)
    //{
    //    OnDestroingRock?.Invoke(sender, EventArgs.Empty);
    //}

    public static void FireOnNotLookingOnInteractable(object sender)
    {
        OnNotLookingOnInteractable?.Invoke(sender, EventArgs.Empty);
    }
}
