using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Core;
using LS.Events;
using LS.Items.Slingshot;
using LS.Meta;
using LS.Save;
using UnityEngine;

namespace LS.Gameplay
{
    public class GameFlowController : MonoBehaviour, IInjectable
    {
        private ICharacterMovementController _characterMovementController;
        private Transform _characterTransform;
        
        private IUpgradeManager _upgradeManager;
        private EconomySettings _economySettings;
        private ISaveSystem _saveSystem;
        private GameplaySession _gameplaySession;
        private PhysicsSettings _physicsSettings;
        private bool _launchConfirmed;

        public void Inject(IGameContext context)
        {
            _upgradeManager = context.UpgradeManager;
            _saveSystem = context.SaveSystem;
            _physicsSettings = context.PhysicsSettings;
            _economySettings = context.EconomySettings;

            _characterMovementController = context.CharacterMovementController;
            _characterTransform = _characterMovementController.CharacterTransform;
        }

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
            _launchConfirmed = false;
        }

        private void Update()
        {
            _gameplaySession?.OnUpdate();
            CheckSessionEnd();
        }

        private void CollectCoins(int value)
        {
            if (!_gameplaySession.IsActive) return;
            _gameplaySession.AddCoins(value);
        }

        private void ApplyUpgradeModifiers()
        {
            var modifiers = _upgradeManager.GetModifiers();
            GameEvents.OnUpgradeModifiersApplied?.Invoke(modifiers);
            
            _gameplaySession = new GameplaySession(modifiers, _economySettings, _characterTransform, _saveSystem);
            _gameplaySession.OnStart();  
            
            GameEvents.OnSessionStarted?.Invoke(_gameplaySession); 

        }

        private void CheckSessionEnd()
        {
            if (_gameplaySession == null || !_gameplaySession.IsActive) return;
            if (!_characterMovementController.HasLaunched) return;
            float sqrSpeed = _characterMovementController.Velocity.sqrMagnitude;

            if (!_launchConfirmed)
            {
                if (sqrSpeed >= _physicsSettings.CharacterPhysics.LaunchConfirmThreshold)
                    _launchConfirmed = true;
                return;
            }

            if (sqrSpeed > _physicsSettings.CharacterPhysics.StopSpeedThreshold) return;
            
            _gameplaySession.OnEnd();
            GameEvents.OnSessionEnded?.Invoke(_gameplaySession);

        }
    }
}
