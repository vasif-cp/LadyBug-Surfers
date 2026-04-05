using System;
using System.Collections.Generic;
using LS.Events;
using LS.Gameplay;
using LS.Save;
using UnityEngine;

namespace LS.Items.Collectibles
{
    public class CollectibleManager : MonoBehaviour
    {
        private BaseCollectible[] _allCollectibles;
        private readonly HashSet<(int resourceID, int itemID)> _collectedSetOnSession = new();

        
        private const string KeyPrefix = "Collectible_";

        private void Awake()
        {
            _allCollectibles = GetComponentsInChildren<BaseCollectible>();
            GameEvents.OnCollectibleCollected += HandleCollectable;
            GameEvents.OnSessionEnded += OnSessionEnded;
        }

        private void Start()
        {
            SyncAll();
        }

        private void OnDestroy()
        {
            GameEvents.OnCollectibleCollected -= HandleCollectable;
            GameEvents.OnSessionEnded -= OnSessionEnded;
        }

        private void HandleCollectable(int resourceID, int itemID, int value)
        {
            if (SaveSystem.IsCollectibleCollected(resourceID, itemID) || 
                !_collectedSetOnSession.Add((resourceID, itemID))) return;     
                                                                                                                                                                              
            for (int i = 0; i < _allCollectibles.Length; i++)
            {                                                                                                                                                               
                if (_allCollectibles[i].ResourceID == resourceID && _allCollectibles[i].UniqueId == itemID)
                {                                                                                                                                                           
                    _allCollectibles[i].OnCollected();
                    break;                                                                                                                                                  
                }
            }
        }
        
        private void OnSessionEnded(GameplaySession gameplaySession)                                                                                                                
        {
            foreach (var (resourceID, itemID) in _collectedSetOnSession)                                                                                                         
                SaveSystem.SaveCollectible(resourceID, itemID);
            
            _collectedSetOnSession.Clear();
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
