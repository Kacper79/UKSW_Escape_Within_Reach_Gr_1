using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerRelated
{
    public class PlayerCoinThrower : MonoBehaviour
    {

        void Start()
        {
            inventoryManager = transform.parent.GetComponentInChildren<InventoryManager>();
        }

        void OnEnable()
        {
            GlobalEvents.OnThrowCoin += OnCoinThrown;
        }

        void OnDisable()
        {
            GlobalEvents.OnThrowCoin -= OnCoinThrown;
        }

        private void OnCoinThrown(object sender, System.EventArgs e)
        {
            if (inventoryManager.RemoveUsedItem(coinItemPrefab))
            {
                Vector3 spawnPos = cameraTransform.position + 0.6f * cameraTransform.forward;
                GameObject newCoin = Instantiate(coinToSpawnPrefab, spawnPos, Quaternion.LookRotation(cameraTransform.forward, Vector3.up));
                float throwAngle = Mathf.Clamp(90.0f - Vector3.Angle(cameraTransform.forward, Vector3.up), -45.0f, 90.0f);
                throwAngle = Mathf.Lerp(0.2f, 0.7f, (throwAngle + 45f) / 135f);
                Vector3 normalRot = newCoin.transform.rotation.eulerAngles;
                normalRot.x = 0.0f;
                newCoin.transform.rotation = Quaternion.Euler(normalRot);
                Debug.Log("Throwing coin at angle " + throwAngle);
                newCoin.GetComponent<SimulateCoin>().StartCoinThrow(throwAngle);
            } else
            {
                Debug.Log("You have no coin to throw");
            }

        }

        private InventoryManager inventoryManager;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Item coinItemPrefab;
        [SerializeField] private GameObject coinToSpawnPrefab;
    }
}