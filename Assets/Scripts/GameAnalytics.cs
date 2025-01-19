using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class GameAnalytics : MonoBehaviour
    {
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Ensure the manager persists across scenes
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // Wczytaj czas gry z zapisanych danych
            totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);

            // Sprawdzenie argumentów uruchomienia
            string[] args = System.Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--playername" && i + 1 < args.Length)
                {
                    playerName = args[i + 1];
                    break;
                }
            }

            Debug.Log($"Player Name: {playerName}");
        }

        void Update()
        {
            // Zlicz czas gry
            totalPlayTime += (Time.deltaTime * 0.0002778f);
        }

        public void ModifyStat(string key, object value)
        {
            // Dodaj lub aktualizuj statystykę
            if (stats.ContainsKey(key))
                stats[key] = value;
            else
                stats.Add(key, value);
        }

        public void ClearStatsOnFreshSave(bool resetPlaytime)
        {
            //PlayerPrefs.SetFloat("TotalPlayTime", 0.0f);
            PlayerPrefs.SetInt("mostRecentlyCompletedQuestID", -1);
            PlayerPrefs.SetInt("deathCount", 0);
            if(resetPlaytime) PlayerPrefs.SetFloat("TotalPlayTime", 0.0f);
        }

        /*void OnApplicationQuit()
        {
            // Wyślij dane do serwera
            SendDataToServer();
        }*/

        public void SendDataToServer()
        {
            // Zapisz czas gry w PlayerPrefs
            PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);

            // Przygotuj dane do wysłania jako obiekt
            // Przygotuj dane do wysłania w formacie JSON jako string
            string jsonData = $@"
            {{
                ""nazwa_gracza"": ""{playerName}"",
                ""playtime_h"": {System.Math.Round(totalPlayTime, 2)},
                ""ilosc_przedm_fabul"": {stats["plotItemsCount"]},
                ""ilosc_smierci"": {PlayerPrefs.GetInt("deathCount")},
                ""ilosc_waluty"": {stats["moneyCounter"]},
                ""ostatni_ukon_quest"": {stats["mostRecentlyCompletedQuestID"]},
                ""postep_fabuly"": {stats["gameProgress"]}
            }}";

            // Serializuj obiekt bezpośrednio do JSON
            //string jsonData = JsonUtility.ToJson(postData);
            Debug.Log($"Final JSON Data: {jsonData}");
            // Wyślij żądanie POST
            StartCoroutine(SendPostRequest(apiUrl, jsonData));
        }

        private IEnumerator SendPostRequest(string url, string json)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(body);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                //yield return request.SendWebRequest();
                request.SendWebRequest();
                while (!request.isDone) { }
                Debug.Log("Coroutine resumed...");

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Data sent successfully: " + request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("Error sending data: " + request.error);
                }
                yield return new WaitForSecondsRealtime(1.0f);
            }
        }

        public static GameAnalytics Instance { get; private set; }
        [SerializeField] private float totalPlayTime = 0f; // Całkowity czas gry w sekundach
        private Dictionary<string, object> stats = new();

        [Header("API Settings")]
        public string apiUrl = "https://yourserver.com/saveStats.php"; // Adres API
        public string playerName = "Player123"; // Unikalny identyfikator gracza
    }
}