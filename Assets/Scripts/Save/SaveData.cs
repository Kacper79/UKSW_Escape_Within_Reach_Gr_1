using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable struct containing all of the neccessary game's state data used to save the game
/// </summary>
[Serializable]
public class SaveData
{
    //Player transform
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    //Player inventory
    public SerializableDictionary<string, int> inventoryItemAmount;
    public List<int> inventoryPlotItems;
    public List<int> inventoryOtherItems;
    public int inventoryGoldAmount;
    //Player HP
    public int playerCurrentHP;
    public int playerMaxHP;
    //Player stress
    public float playerStressLevel;
    public bool playerResidingCell;
    //Player quests
    public List<Quest> playerActiveQuests;
    //public bool isFinalQuestAvailable; 
    //Time of day
    public int timeInMinutes;
    public int breakableWallItemsUsed;
}

/// <summary>
/// Dictionary class used for JSON serialization
/// </summary>
/// <typeparam name="TKey">Key Type</typeparam>
/// <typeparam name="TValue">Value Type</typeparam>
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public SerializableDictionary() : base() { }
    public SerializableDictionary(Dictionary<TKey, TValue> dictionary) : base(dictionary) { }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (var kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}