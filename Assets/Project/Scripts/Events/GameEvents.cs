using System;
using LS.Gameplay;
using LS.Items.Obstacles;
using LS.Meta;
using UnityEngine;

namespace LS.Events
{
    public readonly struct CollectibleCollectedEvent                                                                                                                        
    {           
        public readonly int ResourceID;
        public readonly int ItemID;
        public readonly int Value;                                                                                                                                          
   
        public CollectibleCollectedEvent(int resourceID, int itemID, int value)                                                                                             
        {       
            ResourceID = resourceID;
            ItemID = itemID;
            Value = value;                                                                                                                                                  
        }
    }     
    
    public static class GameEvents
    {
        /* Lifecycle */
        public static Action OnGameStartRequested;
        public static Action OnCameraTransitionComplete;
        public static Action<GameplaySession> OnSessionStarted;                                                                                                                     
        public static Action<GameplaySession> OnSessionEnded;
        
        /* Slingshot */
        public static Action OnPullStarted;
        public static Action<float> OnPullUpdated;
        public static Action OnPullEnded;
        public static Action<Vector3> OnLaunchRequested;
        
        
        /* Character Movement */
        public static Action<float> OnCharacterDistanceUpdated;
        
        
        /* Meta Upgrades */
        public static Action<UpgradeModifiers> OnUpgradeModifiersApplied;
        public static Action<UpgradeType> OnUpgradePurchased;

        /* Upgrades */
        public static Action<CollectibleCollectedEvent> OnCollectibleCollected;
        public static Action<int> OnCoinsCollected;
        public static Action<int> OnCoinsBalanceUpdated;
        
        /* Item Interactions */
        public static Action<ObstacleType> OnObstacleHit;

        public static void Clear()
        {
            OnGameStartRequested = null;
            OnCameraTransitionComplete = null;
            OnSessionStarted = null;
            OnSessionEnded = null;
            OnPullStarted = null;
            OnPullUpdated = null;
            OnPullEnded = null;
            OnLaunchRequested = null;
            OnUpgradeModifiersApplied = null;
            OnUpgradePurchased = null;
            OnCollectibleCollected = null;
            OnCoinsCollected = null;
            OnCoinsBalanceUpdated = null;
            OnObstacleHit = null;
            OnCharacterDistanceUpdated = null;

        }
    }
}
