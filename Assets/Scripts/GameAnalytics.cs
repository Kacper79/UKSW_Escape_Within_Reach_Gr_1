using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    /// <summary>
    /// Ta klasa zajmuje sie analityka
    /// </summary>
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

            // Sprawdzenie argumentow uruchomienia
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

        /// <summary>
        /// Statystyki sa przechowywane w slowniku gdzie nazwa
        /// statystyki jest kojarzona z jej wartoscia. Modyfikuje
        /// wartosc statystyki lub tworzy nowa jezeli nie istnieje
        /// </summary>
        /// <param name="key">Nazwa statystyki</param>
        /// <param name="value">Wartosc statystyki</param>
        public void ModifyStat(string key, object value)
        {
            // Dodaj lub aktualizuj statystyke
            if (stats.ContainsKey(key))
                stats[key] = value;
            else
                stats.Add(key, value);
        }

        /// <summary>
        /// Czysci wszystkie statystyki dla testu
        /// </summary>
        /// <param name="resetPlaytime"></param>
        public void ClearStatsOnFreshSave(bool resetPlaytime)
        {
            //PlayerPrefs.SetFloat("TotalPlayTime", 0.0f);
            PlayerPrefs.SetInt("mostRecentlyCompletedQuestID", -1);
            PlayerPrefs.SetInt("deathCount", 0);
            if(resetPlaytime) PlayerPrefs.SetFloat("TotalPlayTime", 0.0f);
        }

        /*void OnApplicationQuit()
        {
            // Wyslij dane do serwera
            SendDataToServer();
        }*/

        /// <summary>
        /// Funkcja ktora zbiera wszystkie statystyki i tworzy sformatowane cialo JSON
        /// </summary>
        public void SendDataToServer()
        {
            // Zapisz czas gry w PlayerPrefs
            PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);

            // Przygotuj dane do wyslania w formacie JSON jako string
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

            Debug.Log($"Final JSON Data: {jsonData}");
            // Wyslij żadanie POST
            StartCoroutine(SendPostRequest(apiUrl, jsonData));
        }

        /// <summary>
        /// Ta korotyna sluzy do wysylania analityki na serwer
        /// </summary>
        /// <param name="url">URL serwera php ktory kontaktuje sie z baza danych</param>
        /// <param name="json">sformatowane cialo zadania wyslania statystyk</param>
        /// <returns></returns>
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
        /// <summary>
        /// Publiczny dostep do singletona analityki
        /// </summary>
        public static GameAnalytics Instance { get; private set; }
        /// <summary>
        /// Obecny czas grania
        /// </summary>
        [SerializeField] private float totalPlayTime = 0f; // Calkowity czas gry w sekundach
        /// <summary>
        /// Statystyki jakie sa wysylane do bazy danych
        /// </summary>
        private Dictionary<string, object> stats = new();

        /// <summary>
        /// URL serwera php wysylajacego zapytania do bazy danych
        /// </summary>
        [Header("API Settings")]
        public string apiUrl = "https://yourserver.com/saveStats.php"; // Adres API
        /// <summary>
        /// Nazwa gracza jaka sie identyfikuje
        /// </summary>
        public string playerName = "Player123"; // Unikalny identyfikator gracza
    }
}