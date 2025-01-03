using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] private List<Quest> defaultQuestList;
    private List<Quest> activeQuests = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        DefaultInitializeQuests();
    }

    public void DefaultInitializeQuests()
    {
        activeQuests = defaultQuestList;
    }

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            //quest.OnQuestCompleted += HandleQuestCompleted;
        }
    }

    public void RemoveQuest(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            //quest.OnQuestCompleted -= HandleQuestCompleted;
            activeQuests.Remove(quest);
        }
    }

    public void SampleCallback()
    {
        Debug.Log("Yay, the quest is completed");
    }

    public void MarkQuestCompleted(int questID)
    {
        Quest? quest = GetActiveQuests.Find(questp => questp.Id == questID);
        if (quest == null) return;

        Debug.Log($"Quest Completed: {quest?.Description}");
        quest?.CompleteQuest();
        RemoveQuest((Quest)quest);
    }

    public List<Quest> GetActiveQuests { get => activeQuests; private set => activeQuests = value; }

    // Method to check if a quest is active or completed
    public bool IsQuestCompleted(int questId)
    {
        var quest = activeQuests.Find(q => q.Id == questId);
        return quest.IsCompleted;
    }

    public Quest GetCurrentActiveQuest(int questId)
    {
        return activeQuests.Find(q => q.Id == questId);
    }
}