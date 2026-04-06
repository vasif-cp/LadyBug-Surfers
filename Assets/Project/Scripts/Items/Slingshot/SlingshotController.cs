using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Events;
using LS.Meta;
using UnityEngine;

namespace LS.Items.Slingshot
{
    public class SlingshotController : MonoBehaviour
    {
        [Header("Data Dependencies")]
        [SerializeField] private PhysicsSettings _physicsSettings;
        
        [Header("Scene Dependencies")]
        [SerializeField] private CharacterMovementController _characterMovementController;
        [SerializeField] private FloatingJoystick _joystick;
        [SerializeField] private LineRenderer _bandLineRenderer;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private UnityEngine.Camera _mainCamera;
        
        
        private SlingshotEngine _engine;
        private Vector3 _restPosition;
        private Vector3 _currentCharacterPosition;
        private float _launchPowerBonus;
        private bool _isActive;
        
        
        private const float PullStartThreshold = 0.2f;
        private const float PullUpdateThreshold = 0.01f;

        private void Awake()
        {
            _engine = new SlingshotEngine(_physicsSettings);
            _isActive = false;
            
            GameEvents.OnCameraTransitionComplete += OnGameStartRequested;
        }

        private void OnDestroy()
        {
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
            _joystick.gameObject.SetActive(true);
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
            float pullInput = Mathf.Clamp01(-_joystick.Vertical); 
                                                                                                                                                                              
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
                                        + (-_characterTransform.forward * pullAmount * _physicsSettings.MaxPullDistance);

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
 
            _characterMovementController.RequestLaunchWithImpulse(impulse);
            
            GameEvents.OnPullEnded?.Invoke();
        }
        
        private void CancelPull()
        {
            _engine.Reset();
            _currentCharacterPosition = _restPosition;
        }

        private bool IsJoystickInputActive()
        {
            return Mathf.Abs(_joystick.Vertical) > 0.0f;
        }
        
        public void ApplyUpgradeModifiers(in UpgradeModifiers modifiers)
        {                                                               
            _launchPowerBonus = modifiers.LaunchPowerBonus;
        }
        
        private void InterpolateSledPosition()
        {
            if (!IsJoystickInputActive()) return;
            
            float speed = _engine.IsPulling ? _physicsSettings.ForwardInterpolationSpeed : _physicsSettings.BackwardInterpolationSpeed;

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
        
        private Vector3 ScreenToGroundPlane(Vector2 screenPos)
        {
            Ray ray = _mainCamera.ScreenPointToRay(screenPos);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, 0.0f, 0f));
 
            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
 
            return ray.GetPoint(_physicsSettings.MaxPullDistance);
        }
        
        private float GetLaunchMultiplier()
        {
            return 1f + _launchPowerBonus;
        }
    }
}
