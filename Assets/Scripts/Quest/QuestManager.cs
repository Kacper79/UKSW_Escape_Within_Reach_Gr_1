using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour, ISaveable
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
        SaveManager.Instance.saveablesGO.Add(this);
        if(PlayerPrefs.GetInt("ContinueSave") == 0) DefaultInitializeQuests();
    }

    public void DefaultInitializeQuests()
    {
        if(activeQuests.Count == 0) activeQuests = defaultQuestList;
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

    public void Save(ref SaveData saveData)
    {
        saveData.playerActiveQuests = activeQuests.GetRange(0, activeQuests.Count);
    }

    public void Load(SaveData saveData)
    {
        activeQuests.Clear();
        activeQuests = saveData.playerActiveQuests;
    }

    public List<Quest> GetDefaultQuestsList()
    {
        return defaultQuestList;
    }
}