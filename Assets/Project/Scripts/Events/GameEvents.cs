using System;
using LS.Gameplay;
using LS.Items.Obstacles;
using LS.Meta;
using UnityEngine;

namespace LS.Events
{
    public static class GameEvents
    {
        public static Action OnGameStartRequested;
        public static Action OnCameraTransitionComplete;
        
        public static Action<GameplaySession> OnSessionStarted;                                                                                                                     
        public static Action<GameplaySession> OnSessionEnded;
        
        public static Action OnPullStarted;
        public static Action<float> OnPullUpdated;
        public static Action OnPullEnded;

        public static Action<Vector3> OnLaunchRequested;
        
        public static Action<UpgradeModifiers> OnUpgradeModifiersApplied;
        public static Action<UpgradeType> OnUpgradePurchased;

        
        public static Action<int,int,int> OnCollectibleCollected;
        public static Action<int> OnCoinsCollected;
        public static Action<int> OnCoinsBalanceUpdated;
        
        public static Action<ObstacleType> OnObstacleHit;
    }
}
