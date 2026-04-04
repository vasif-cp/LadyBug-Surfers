using System;
using LS.Events;
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
            SyncAll();

            GameEvents.OnCollectibleCollected += HandleCollectable;
        }

        private void OnDestroy()
        {
            GameEvents.OnCollectibleCollected -= HandleCollectable;
        }

        private void HandleCollectable(int resourceID, int itemID, int value)
        {
            if (IsCollected(resourceID, itemID)) return;
 
            SetCollected(resourceID, itemID);
 
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
                _allCollectibles[i].gameObject.SetActive(!IsCollected(_allCollectibles[i].ResourceID, _allCollectibles[i].UniqueId));
            }
        }
        
        private void SetCollected(int resourceID, int itemID)
        {
            PlayerPrefs.SetInt($"{KeyPrefix}:{resourceID}:{itemID}", 1);
            PlayerPrefs.Save();
        }
        
        private bool IsCollected(int resourceID, int itemID)
        {
            return PlayerPrefs.GetInt($"{KeyPrefix}:{resourceID}:{itemID}", 0) == 1;
        }
    }
}
