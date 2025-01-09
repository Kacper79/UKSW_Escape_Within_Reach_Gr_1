using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public List<ISaveable> saveablesGO = new();
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
        }
    }

    private void ProcessGameSave()
    {
        SaveGame(GatherObjectsForSave());
        saveablesGO.Clear();
    }

    //Actually saves the SaveData struct to file
    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved to {saveFilePath}");
    }

    //Creates SaveData struct with the data from the scene
    public SaveData GatherObjectsForSave()
    {
        SaveData saveData = new();
        //Find Player and save it's position and rotation (could be relevant)
        GameObject playerGO = GameObject.FindWithTag("Player");
        saveData.playerPosition = playerGO.transform.position;
        saveData.playerRotation = playerGO.transform.rotation;

        /*foreach(GameObject saveableGO in saveablesGO)
        {
            if(saveableGO.TryGetComponent(out ISaveable saveable))
            {
                saveable.Save(ref saveData);
            }
        }*/

        foreach(ISaveable saveable in saveablesGO)
        {
            saveable.Save(ref saveData);
        }

        return saveData;
    }

    //Fills the important objects on the scene with the data from SaveData struct
    public void GatherObjectsForLoad(SaveData saveData)
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerGO.transform.position = saveData.playerPosition;
        playerGO.transform.rotation = saveData.playerRotation;

        /*foreach (GameObject saveableGO in saveablesGO)
        {
            if (saveableGO.TryGetComponent(out ISaveable saveable))
            {
                saveable.Load(saveData);
            }
        }*/

        foreach(ISaveable saveable in saveablesGO)
        {
            saveable.Load(saveData);
        }
    }

    //Loads SaveData structure from a file
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

    //Deletes save file for the future UI
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

    public bool CheckSaveExists()
    {
        if (File.Exists(saveFilePath)) return true;
        else return false;
    }
}