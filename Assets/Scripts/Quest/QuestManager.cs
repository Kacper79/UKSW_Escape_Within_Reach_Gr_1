using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a singleton manager class that controlls all quests in the game.
/// </summary>
public class QuestManager : MonoBehaviour, ISaveable
{
    /// <summary>
    /// Singleton's public access point
    /// </summary>
    public static QuestManager Instance { get; private set; }

    /// <summary>
    /// This lists contains all the quests that the game can have.
    /// It is used to default initialize activeQuests variable when
    /// the new save state has been created giving new fresh save state
    /// </summary>
    [SerializeField] private List<Quest> defaultQuestList;
    /// <summary>
    /// This class contains all the quests in the incomplete state
    /// Can be mutated by the save/load functionality
    /// </summary>
    private List<Quest> activeQuests = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SaveManager.Instance.saveablesGO.Add(this);
        if (PlayerPrefs.GetInt("ContinueSave") == 0)
        {
            DefaultInitializeQuests();
        }
    }

    /// <summary>
    /// Default initialize the activeQuest list by the defaultQuestList list
    /// Used when the player selects to play fresh new save state
    /// </summary>
    public void DefaultInitializeQuests()
    {
        if(activeQuests.Count == 0) activeQuests = new(defaultQuestList);
    }

    /// <summary>
    /// Adds a quest to to the active quest list variable
    /// </summary>
    /// <param name="quest">New quest to be added</param>
    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            //quest.OnQuestCompleted += HandleQuestCompleted;
        }
    }

    /// <summary>
    /// Removes quest from the active quest list,
    /// usually when the quest is completed
    /// </summary>
    /// <param name="quest"></param>
    public void RemoveQuest(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            //quest.OnQuestCompleted -= HandleQuestCompleted;
            activeQuests.Remove(quest);
        }
    }

    /// <summary>
    /// Use this function to clean this manager' state after the quest has been completed
    /// </summary>
    /// <param name="questID">ID of the quest completed</param>
    public void MarkQuestCompleted(int questID)
    {
        Quest? quest = GetActiveQuests.Find(questp => questp.Id == questID);
        if (quest == null) return;

        Debug.Log($"Quest Completed: {quest?.Description}");
        PlayerPrefs.SetInt("mostRecentlyCompletedQuestID", questID);
        quest?.CompleteQuest();
        RemoveQuest((Quest)quest);
    }

    public List<Quest> GetActiveQuests { get => activeQuests; private set => activeQuests = value; }

    /// <summary>
    /// This is a utility method that can be used to check whether
    /// quest is being active and therefore not completed
    /// </summary>
    /// <param name="questId">ID of the quest to be checked </param>
    /// <returns>Returns a bool value telling if the quests has been </returns>
    public bool IsQuestCompleted(int questId)
    {
        var quest = activeQuests.Find(q => q.Id == questId);
        return quest.IsCompleted;
    }

    /// <summary>
    /// This is a utility method that can be used to check whether
    /// quest is being active and therefore not completed
    /// </summary>
    /// <param name="questId">ID of the quest to be checked</param>
    /// <returns>Returns a Quest class instance of the found class or null if the quest is finished</returns>
    public Quest GetCurrentActiveQuest(int questId)
    {
        return activeQuests.Find(q => q.Id == questId);
    }

    /// <summary>
    /// This function is being used to save all the quests in the save file
    /// </summary>
    /// <param name="saveData">Mutable save data struct to save data to</param>
    public void Save(ref SaveData saveData)
    {
        saveData.playerActiveQuests = activeQuests.GetRange(0, activeQuests.Count);
        GameAnalytics.Instance.ModifyStat("mostRecentlyCompletedQuestID", PlayerPrefs.GetInt("mostRecentlyCompletedQuestID"));
        GameAnalytics.Instance.ModifyStat("gameProgress", System.Math.Round( (defaultQuestList.Count - activeQuests.Count) / (float)defaultQuestList.Count, 2));
    }

    /// <summary>
    /// This function is being used to save all the quests in the save file
    /// </summary>
    /// <param name="saveData">Save data struct to load data from</param>
    public void Load(SaveData saveData)
    {
        activeQuests.Clear();
        activeQuests = saveData.playerActiveQuests;
    }
}