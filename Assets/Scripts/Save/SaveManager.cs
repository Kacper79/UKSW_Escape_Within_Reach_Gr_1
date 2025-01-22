using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This manager class is being used to save/load the game's state
/// </summary>
[Serializable]
public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// Singleton public access point
    /// </summary>
    public static SaveManager Instance { get; private set; }
    /// <summary>
    /// List of the classes implementing ISaveable interface
    /// </summary>
    public List<ISaveable> saveablesGO = new();
    /// <summary>
    /// The path at which the save file will be created
    /// </summary>
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the manager persists across scenes
            saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        Application.quitting += ProcessGameSave;
    }

    void OnDisable()
    {
        Application.quitting -= ProcessGameSave;
    }

    void Start()
    {
        bool continueSave = PlayerPrefs.GetInt("ContinueSave") == 1;
        ProcessGameLoad(continueSave);
    }

    /// <summary>
    /// Callback that handles game state's loading from the main menu
    /// </summary>
    /// <param name="continueSave">Whether to continue the current save state or create a new one</param>
    public void ProcessGameLoad(bool continueSave)
    {
        if (continueSave)
        {
            if (CheckSaveExists())
            {
                SaveData saveData = LoadGame();
                GatherObjectsForLoad(saveData);
            } else
            {
                Debug.LogError("Cannot load the save that doesn't exist");
            }
        } else
        {
            DeleteSave();
            GameAnalytics.Instance.ClearStatsOnFreshSave(false);
        }
    }

    /// <summary>
    /// This callback is being used to save game's state on quiting the game
    /// </summary>
    public void ProcessGameSave()
    {
        SaveGame(GatherObjectsForSave());
        saveablesGO.Clear();
    }

    /// <summary>
    /// This function touches the filesystem to save the SaveData struct to file
    /// </summary>
    /// <param name="data">SaveData struct containing all of the save data</param>
    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved to {saveFilePath}");
    }

    /// <summary>
    /// Gathers all of the ISaveable classes and creates SaveData struct with the data gathered
    /// </summary>
    /// <returns>New SaveData struct containing the game's current state</returns>
    public SaveData GatherObjectsForSave()
    {
        SaveData saveData = new();
        //Find Player and save it's position and rotation (could be relevant)
        GameObject playerGO = GameObject.FindWithTag("Player");
        saveData.playerPosition = playerGO.transform.position;
        saveData.playerRotation = playerGO.transform.rotation;

        foreach(ISaveable saveable in saveablesGO)
        {
            saveable.Save(ref saveData);
        }

        GameAnalytics.Instance.SendDataToServer();

        return saveData;
    }

    /// <summary>
    /// Gathers all of the ISaveable classes and fills them with the data from SaveData struct
    /// </summary>
    /// <param name="saveData">SaveData struct containing the game's sate loaded from the save file</param>
    public void GatherObjectsForLoad(SaveData saveData)
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerGO.transform.position = saveData.playerPosition;
        playerGO.transform.rotation = saveData.playerRotation;

        foreach(ISaveable saveable in saveablesGO)
        {
            saveable.Load(saveData);
        }
    }

    /// <summary>
    /// This function touches the filesystem and reads SaveData structure from a file
    /// </summary>
    /// <returns>SaveData struct containing the game's sate loaded from the save file</returns>
    public SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded successfully.");
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }

    /// <summary>
    /// Deletes save file when the player chooses to play a new game
    /// </summary>
    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.LogWarning("No save file to delete.");
        }
    }

    /// <summary>
    /// Checks whether the save file exists in the filesystem
    /// </summary>
    /// <returns>Boolean value telling if the save file exists</returns>
    public bool CheckSaveExists()
    {
        if (File.Exists(saveFilePath)) return true;
        else return false;
    }
}