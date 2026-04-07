using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Core;
using LS.Events;
using LS.Meta;
using UnityEngine;

namespace LS.Items.Slingshot
{
    public class SlingshotController : MonoBehaviour, IInjectable
    {
        private IInputProvider _inputProvider;
        private Transform _characterTransform;
        
        private SlingshotEngine _engine;
        private PhysicsSettings _physicsSettings;
        
        private LineRenderer _bandLineRenderer;
        
        private Vector3 _restPosition;
        private Vector3 _currentCharacterPosition;
        private float _launchPowerBonus;
        private bool _isActive;
        
        
        private const float PullStartThreshold = 0.2f;
        private const float PullUpdateThreshold = 0.01f;

        public void Inject(IGameContext context)
        {
            _physicsSettings = context.PhysicsSettings;
            _inputProvider = context.InputProvider;
            _characterTransform = context.CharacterMovementController.CharacterTransform;
        }

        private void Awake()
        {
            _engine = new SlingshotEngine(_physicsSettings);
            _isActive = false;

            _bandLineRenderer = GetComponentInChildren<LineRenderer>();

            GameEvents.OnUpgradeModifiersApplied += ApplyUpgradeModifiers;
            GameEvents.OnCameraTransitionComplete += OnGameStartRequested;
        }

        private void OnDestroy()
        {
            GameEvents.OnUpgradeModifiersApplied -= ApplyUpgradeModifiers;
            GameEvents.OnCameraTransitionComplete -= OnGameStartRequested;
        }

        private void OnEnable()
        {
            _restPosition = _characterTransform.position;
            _currentCharacterPosition = _restPosition;
        }

        private void OnGameStartRequested()
        {
            _isActive = true;
        }
        
        private void Update()
        {
            if (!_isActive || _engine.IsLaunched) return;
 
            HandleInput();
            InterpolateSledPosition();
            UpdateBandVisuals();
        }
        
        private void HandleInput()
        {
            float pullInput = Mathf.Clamp01(-_inputProvider.VerticalInput); 
                                                                                                                                                                              
            if (!_engine.IsPulling)             
            {
                if (pullInput > PullStartThreshold)
                {
                    BeginPull();
                }
            }                                                                                                                                                                       
            else if (pullInput > PullUpdateThreshold)             
            {                                   
                UpdatePull(pullInput);
            }                                                                                                                                                                       
            else
            {                                                                                                                                                                       
                ReleasePull();
            }
        }
        
        private void BeginPull()
        {
            _restPosition = _characterTransform.position;
            _engine.BeginPull(_restPosition);   
            GameEvents.OnPullStarted?.Invoke();
        }        
        
        private void UpdatePull(float pullAmount)                                                                                                                       
        {               
            Vector3 simulatedWorldPos = _restPosition
                                        + (-_characterTransform.forward * pullAmount * _physicsSettings.SlingshotPhysics.MaxPullDistance);

            Vector3 targetPos = _engine.UpdatePull(simulatedWorldPos, _characterTransform.forward);                                                                                 
            _currentCharacterPosition = targetPos;
                                                                                                                                                                              
            GameEvents.OnPullUpdated?.Invoke(_engine.PullAmount);                                                                                                                   
        }
        
        
        private void ReleasePull()
        {
            Vector3 impulse = _engine.Release(_characterTransform.forward, GetLaunchMultiplier());
 
            if (impulse.sqrMagnitude < 0.01f)
            {
                CancelPull();
                return;
            }
 
            _characterTransform.position = _restPosition;
            _currentCharacterPosition = _restPosition;
 
            GameEvents.OnLaunchRequested?.Invoke(impulse);
            GameEvents.OnPullEnded?.Invoke();
        }
        
        private void CancelPull()
        {
            _engine.Reset();
            _currentCharacterPosition = _restPosition;
        }

        private bool IsJoystickInputActive()
        {
            return Mathf.Abs(_inputProvider.VerticalInput) > 0.0f;
        }
        
        public void ApplyUpgradeModifiers(UpgradeModifiers modifiers)
        {                                                               
            _launchPowerBonus = modifiers.LaunchPowerBonus;
        }
        
        private void InterpolateSledPosition()
        {
            if (!IsJoystickInputActive()) return;
            
            float speed = _engine.IsPulling ? _physicsSettings.SlingshotPhysics.ForwardInterpolationSpeed : _physicsSettings.SlingshotPhysics.BackwardInterpolationSpeed;

            _characterTransform.position = Vector3.Lerp(_characterTransform.position, _currentCharacterPosition,
                Time.deltaTime * speed);

        }
        
        private void UpdateBandVisuals()
        {
            if (_bandLineRenderer == null) return;
            
            float _behindDistance = 2f;
            Vector3 behindOffset = -_characterTransform.forward * _behindDistance;
            Vector3 middlePoint = _characterTransform.position + behindOffset;

            _bandLineRenderer.SetPosition(1, middlePoint);
        }
        
        private float GetLaunchMultiplier()
        {
            return 1f + _launchPowerBonus;
        }
    }
}
