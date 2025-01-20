using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Quest
{
    public class EndGameArea : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && QuestManager.Instance.GetActiveQuests.Count == 0)
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
}