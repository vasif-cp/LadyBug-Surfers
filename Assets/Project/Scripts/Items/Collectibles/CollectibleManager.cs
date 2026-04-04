using System;
using LS.Events;
using LS.Save;
using UnityEngine;

namespace LS.Items.Collectibles
{
    public class CollectibleManager : MonoBehaviour
    {
        private BaseCollectible[] _allCollectibles;
        
        private const string KeyPrefix = "Collectible_";

        private void Awake()
        {
            _allCollectibles = GetComponentsInChildren<BaseCollectible>();
            GameEvents.OnCollectibleCollected += HandleCollectable;
        }

        private void Start()
        {
            SyncAll();
        }

        private void OnDestroy()
        {
            GameEvents.OnCollectibleCollected -= HandleCollectable;
        }

        private void HandleCollectable(int resourceID, int itemID, int value)
        {
            if (SaveSystem.IsCollectibleCollected(resourceID, itemID)) return;
 
            SaveSystem.SaveCollectible(resourceID, itemID);
 
            for (int i = 0; i < _allCollectibles.Length; i++)
            {
                if (_allCollectibles[i].ResourceID.Equals(resourceID) && _allCollectibles[i].UniqueId.Equals(itemID))
                {
                    _allCollectibles[i].OnCollected();
                    break;
                }
            }
        }
        
        private void SyncAll()
        {
            for (int i = 0; i < _allCollectibles.Length; i++)
            {
                _allCollectibles[i].gameObject.SetActive(!SaveSystem.IsCollectibleCollected(_allCollectibles[i].ResourceID, _allCollectibles[i].UniqueId));
            }
        }
    }
}
