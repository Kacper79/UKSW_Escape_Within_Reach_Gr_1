using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
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

    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved to {saveFilePath}");
    }

    public SaveData GatherObjectsForSave()
    {
        SaveData saveData = new();
        //Find Player and save it's position and rotation (could be relevant)
        GameObject playerGO = GameObject.FindWithTag("Player");
        saveData.playerPosition = playerGO.transform.position;
        saveData.playerRotation = playerGO.transform.rotation;

        return saveData;
    }

    public void GatherObjectsForLoad(SaveData saveData)
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        playerGO.transform.position = saveData.playerPosition;
        playerGO.transform.rotation = saveData.playerRotation;
    }

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
}