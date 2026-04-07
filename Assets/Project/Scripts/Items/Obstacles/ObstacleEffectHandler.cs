using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Core;
using LS.Events;
using UnityEngine;

namespace LS.Items.Obstacles
{
    public class ObstacleEffectHandler : MonoBehaviour, IInjectable
    {
        private ICharacterMovementController _characterMovementController;
        private PhysicsSettings _physicsSettings;
        

        public void Inject(IGameContext context)
        {
            _physicsSettings = context.PhysicsSettings;
            _characterMovementController = context.CharacterMovementController;
        }

        private void Awake()
        {
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
                    _characterMovementController.ApplySlowdown(_physicsSettings.CharacterPhysics.ObstacleSlowFactor);
                    break;
                case ObstacleType.Stop:
                    _characterMovementController.ApplyStop();
                    break;
            }
        }

    }
}
