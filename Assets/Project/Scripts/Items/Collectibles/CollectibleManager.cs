using System;
using System.Collections.Generic;
using LS.Core;
using LS.Events;
using LS.Gameplay;
using LS.Save;
using UnityEngine;

namespace LS.Items.Collectibles
{
    public class CollectibleManager : MonoBehaviour, IInjectable
    {
        private ISaveSystem _saveSystem;
        private BaseCollectible[] _allCollectibles;
        private readonly HashSet<(int resourceID, int itemID)> _collectedSetOnSession = new();

        
        private const string KeyPrefix = "Collectible_";

        public void Inject(IGameContext context)
        {
            _saveSystem = context.SaveSystem;
        }

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
            if (_saveSystem.IsCollectibleCollected(resourceID, itemID) || 
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
            {
                _saveSystem.SaveCollectible(resourceID, itemID);
            }
            
            _collectedSetOnSession.Clear();
        }                                                                                                                                                                   


        
        private void SyncAll()
        {
            for (int i = 0; i < _allCollectibles.Length; i++)
            {
                bool isEnabled =
                    !_saveSystem.IsCollectibleCollected(_allCollectibles[i].ResourceID, _allCollectibles[i].UniqueId);
                _allCollectibles[i].gameObject.SetActive(isEnabled);
            }
        }
    }
}
