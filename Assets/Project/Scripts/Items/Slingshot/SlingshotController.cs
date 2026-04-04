using System;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using UnityEngine;

namespace LS.Items.Slingshot
{
    public class SlingshotController : MonoBehaviour
    {
        [Header("Data Dependencies")]
        [SerializeField] private PhysicsSettings _physicsSettings;
        
        [Header("Scene Dependencies")]
        [SerializeField] private CharacterMovementController _characterMovementController;
        [SerializeField] private LineRenderer _bandLineRenderer;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Camera _mainCamera;
        
        
        private SlingshotEngine _engine;
        private Vector3 _restPosition;
        private Vector3 _currentCharacterPosition;
        private bool _isActive;

        private void Awake()
        {
            _engine = new SlingshotEngine(_physicsSettings);
            _isActive = true;
        }
        
        private void OnEnable()
        {
            _restPosition = _characterTransform.position;
            _currentCharacterPosition = _restPosition;
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
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
 
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        TryBeginPull(touch.position);
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (_engine.IsPulling)
                            UpdatePull(touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (_engine.IsPulling)
                            ReleasePull();
                        break;
                }
                return; 
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                TryBeginPull(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && _engine.IsPulling)
            {
                UpdatePull(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && _engine.IsPulling)
            {
                ReleasePull();
            }
        }
        
        private void TryBeginPull(Vector2 screenPosition)
        {
            Vector3 worldPos = ScreenToGroundPlane(screenPosition);
            float distToSled = Vector3.Distance(worldPos, _characterTransform.position);
 
            if (distToSled > _physicsSettings.MaxPullDistance * 0.5f) return;
 
            _restPosition = _characterTransform.position;
            _engine.BeginPull(_restPosition);
        }
        
        private void UpdatePull(Vector2 screenPosition)
        {
            Vector3 worldPos = ScreenToGroundPlane(screenPosition);
            Vector3 targetPos = _engine.UpdatePull(worldPos, _characterTransform.forward);
 
            _currentCharacterPosition = targetPos;
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
        }
        
        private void CancelPull()
        {
            _engine.Reset();
            _currentCharacterPosition = _restPosition;
        }
        
        private void InterpolateSledPosition()
        {
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
            return 1f;
        }
    }
}
