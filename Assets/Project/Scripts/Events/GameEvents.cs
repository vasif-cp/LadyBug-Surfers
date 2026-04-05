using System;
using LS.Gameplay;
using LS.Items.Obstacles;
using LS.Meta;
using UnityEngine;

namespace LS.Events
{
    public class GameEvents
    {
        public static Action OnGameStartRequested;
        
        public static Action OnPullStarted;
        public static Action<float> OnPullUpdated;
        public static Action OnPullEnded;
        
        public static Action<UpgradeType> OnUpgradePurchased;

        
        public static Action<int,int,int> OnCollectibleCollected;
        public static Action<int> OnCoinsCollected;
        
        public static Action<ObstacleType> OnObstacleHit;

        public static Action<GameplaySession> OnSessionEnded;
    }
}
