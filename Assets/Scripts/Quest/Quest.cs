using System;

[Serializable]
public class Quest
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }

    public event Action<Quest> OnQuestCompleted;

    public Quest(int id, string description)
    {
        Id = id;
        Description = description;
        IsCompleted = false;
    }

    public void CompleteQuest()
    {
        if (IsCompleted) return;

        IsCompleted = true;
        OnQuestCompleted?.Invoke(this);  // Notify observers
    }
}