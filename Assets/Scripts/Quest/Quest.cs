using System;
using UnityEngine.Events;

[Serializable]
public struct Quest
{
    public int Id;
    public string Description;
    public bool IsCompleted;

    public UnityEvent<Quest>? OnQuestCompleted;

    /*public Quest(int id, string description)
    {
        Id = id;
        Description = description;
        IsCompleted = false;
    }*/

    public Quest(int id, string description, UnityEvent<Quest> QuestCallback)
    {
        Id = id;
        Description = description;
        IsCompleted = false;
        OnQuestCompleted = QuestCallback;
    }

    public void CompleteQuest()
    {
        if (IsCompleted) return;

        IsCompleted = true;
        OnQuestCompleted?.Invoke(this);  // Notify observers
        //if (additionalCallback) QuestManager.Instance.RemoveQuest(this);
    }
}