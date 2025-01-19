using System;
using UnityEngine.Events;

[Serializable]
public struct Quest
{
    public int Id;              ///Unique quest ID
    public string Description;  ///Quest human readable description
    public bool IsCompleted;    ///Quest completion status

    public UnityEvent<Quest>? OnQuestCompleted; /// Function acting as a callback when this quest is finished

    /// <summary>
    /// Constructor for the Quest struct. Used for storing the quests details.
    /// </summary>
    /// <param name="id">Unique quest id</param>
    /// <param name="description">Quest's human readable description, likely to be shown in UI</param>
    /// <param name="QuestCallback">Function acting as a callback when the quest is completed</param>
    public Quest(int id, string description, UnityEvent<Quest> QuestCallback)
    {
        Id = id;
        Description = description;
        IsCompleted = false;
        OnQuestCompleted = QuestCallback;
    }

    /// <summary>
    /// Use this function to complete quest and/or trigger event based on that
    /// </summary>
    public void CompleteQuest()
    {
        if (IsCompleted) return;

        IsCompleted = true;
        OnQuestCompleted?.Invoke(this);  // Notify observers
        //if (additionalCallback) QuestManager.Instance.RemoveQuest(this);
    }

    public string GetDescription()
    {
        return Description;
    }
}