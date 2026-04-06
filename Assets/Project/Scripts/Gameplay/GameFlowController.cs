using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Events;
using LS.Items.Slingshot;
using LS.Meta;
using UnityEngine;

namespace LS.Gameplay
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField] private PhysicsSettings _physicsSettings;
        [SerializeField] private CharacterMovementController _characterMovementController;
        [SerializeField] private MetaGameController _metaGameController;                                                                                                            
        [SerializeField] private SlingshotController _slingshotController;   
        [SerializeField] private Transform _characterTransform;
        
        private GameplaySession _gameplaySession;
        private bool _launchConfirmed;
        
        public GameplaySession GameplaySession => _gameplaySession;

        private void Awake()
        {
            GameEvents.OnCoinsCollected += CollectCoins;
            GameEvents.OnGameStartRequested += ApplyUpgradeModifiers;
        }

        private void OnDestroy()
        {
            GameEvents.OnCoinsCollected -= CollectCoins;
            GameEvents.OnGameStartRequested -= ApplyUpgradeModifiers;
        }

        private void Start()
        {
            _gameplaySession = new GameplaySession(_metaGameController.UpgradeManager.GetModifiers(),_characterTransform);
            _gameplaySession.OnStart();
            _launchConfirmed = false;
        }

        private void Update()
        {
            _gameplaySession.OnUpdate();
            CheckSessionEnd();
        }

        private void CollectCoins(int value)
        {
            if (!_gameplaySession.IsActive) return;
            _gameplaySession.AddCoins(value);
        }

        private void ApplyUpgradeModifiers()
        {
            var modifiers = _metaGameController.UpgradeManager.GetModifiers();
            _characterMovementController.ApplyUpgradeModifiers(modifiers);    
            _slingshotController.ApplyUpgradeModifiers(modifiers); 
        }

        private void CheckSessionEnd()
        {
            if (!_gameplaySession.IsActive) return;
            if (!_characterMovementController.HasLaunched) return;
            float sqrSpeed = _characterMovementController.Velocity.sqrMagnitude;

            if (!_launchConfirmed)
            {
                if (sqrSpeed >= _physicsSettings.LaunchConfirmThreshold)
                    _launchConfirmed = true;
                return;
            }

            if (sqrSpeed > _physicsSettings.StopSpeedThreshold) return;
            
            _gameplaySession.OnEnd();
            GameEvents.OnSessionEnded?.Invoke(_gameplaySession);

        }
    }
}
