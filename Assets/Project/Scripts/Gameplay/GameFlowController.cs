using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Events;
using UnityEngine;

namespace LS.Gameplay
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField] private PhysicsSettings _physicsSettings;
        [SerializeField] private CharacterMovementController _characterMovementController;
        [SerializeField] private Transform _characterTransform;
        
        private GameplaySession _gameplaySession;
        
        public GameplaySession GameplaySession => _gameplaySession;
        
        private void Start()
        {
            _gameplaySession = new GameplaySession(_characterTransform);
            _gameplaySession.OnStart();
        }

        private void Update()
        {
            _gameplaySession.OnUpdate();
            CheckSessionEnd();
        }

        private void CheckSessionEnd()
        {
            if (!_gameplaySession.IsActive) return;
            if (!_characterMovementController.HasLaunched) return;
            if (_characterMovementController.Velocity.sqrMagnitude > _physicsSettings.StopSpeedThreshold) return;
            
            _gameplaySession.OnEnd();
            GameEvents.OnSessionEnded?.Invoke(_gameplaySession);

        }
    }
}
