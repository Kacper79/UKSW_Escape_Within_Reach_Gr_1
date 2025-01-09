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
                GameObject newCoin = Instantiate(coinToSpawnPrefab, transform.position, Quaternion.identity);
                newCoin.GetComponent<SimulateCoin>().StartCoinThrow();
            } else
            {
                Debug.Log("You have no coin to throw");
            }

        }

        private InventoryManager inventoryManager;
        [SerializeField] private Item coinItemPrefab;
        [SerializeField] private GameObject coinToSpawnPrefab;
    }
}