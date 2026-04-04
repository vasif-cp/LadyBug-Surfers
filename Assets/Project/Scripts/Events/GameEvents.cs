using System;
using LS.Items.Obstacles;
using UnityEngine;

namespace LS.Events
{
    public class GameEvents
    {
        public static Action OnPullStarted;
        public static Action<float> OnPullUpdated;
        public static Action OnPullEnded;
        
        
        public static Action<int,int,int> OnCollectibleCollected;

        public static Action<ObstacleType> OnObstacleHit;
    }
}
