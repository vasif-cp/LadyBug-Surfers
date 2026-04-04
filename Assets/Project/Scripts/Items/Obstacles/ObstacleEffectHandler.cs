using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Events;
using UnityEngine;

namespace LS.Items.Obstacles
{
    public class ObstacleEffectHandler : MonoBehaviour
    {
        [SerializeField] private PhysicsSettings _physicsSettings;

        private CharacterMovementController _characterMovementController;

        private void Awake()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            GameEvents.OnObstacleHit += HandleObstacleHit;
        }

        private void OnDestroy()
        {
            GameEvents.OnObstacleHit -= HandleObstacleHit;
        }
        
        private void HandleObstacleHit(ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.SlowingDown:
                    _characterMovementController.ApplySlowdown(_physicsSettings.ObstacleSlowFactor);
                    break;
                case ObstacleType.Stop:
                    _characterMovementController.ApplyStop();
                    break;
            }
        }

    }
}
