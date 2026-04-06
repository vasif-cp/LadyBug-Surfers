using System;
using LS.CharacterController.Physics.Data;
using LS.CharacterController.Physics.Core;
using LS.Meta;
using UnityEngine;

namespace LS.CharacterController.Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundDetector))]
    public class CharacterMovementController : MonoBehaviour
    {
        [Header("Data Dependencies")]
        [SerializeField] private PhysicsSettings _physicsSettings;
        
        [Header("Visual Dependencies")]
        [SerializeField] private Transform _visualModelTransform;
        
        private Rigidbody _rigidbody;
        private GroundDetector _groundDetector;
        private CoreSledPhysics _coreSledPhysics;
        
        private float _steerInput;
        private bool _launchRequested;
        
        public bool HasLaunched => _coreSledPhysics.HasLaunched;
        public Vector3 Velocity => _rigidbody.linearVelocity;  

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _groundDetector = GetComponent<GroundDetector>();
            _coreSledPhysics = new CoreSledPhysics(_physicsSettings);
        }
        
        private void FixedUpdate()
        {
            if (!_coreSledPhysics.HasLaunched)
            {
                HandleLaunch();
                return;
            }
            
            GroundInfo ground = _groundDetector.DetectGround();
            ForceResult forces = _coreSledPhysics.CalculateForces(_rigidbody.linearVelocity, ground, _steerInput);
 
            _rigidbody.AddForce(forces.TotalForce, ForceMode.Acceleration);
            
            if (ground.IsGrounded && _visualModelTransform != null && _rigidbody.linearVelocity.sqrMagnitude > 0.1f)
            {
                Vector3 forward = Vector3.ProjectOnPlane(_rigidbody.linearVelocity, ground.SurfaceNormal).normalized;
                Quaternion slopeRotation = Quaternion.LookRotation(forward, ground.SurfaceNormal);

                float bankAngle = -_steerInput * _physicsSettings.MaxBankAngle;
                Quaternion bankRotation = Quaternion.AngleAxis(bankAngle, forward);

                Quaternion targetRotation = bankRotation * slopeRotation;
                _visualModelTransform.rotation = Quaternion.Slerp(
                    _visualModelTransform.rotation, targetRotation,
                    Time.fixedDeltaTime * _physicsSettings.VisualAlignSpeed);
            }

        }
        
        public void SetSteerInput(float horizontal)
        {
            _steerInput = Mathf.Clamp(horizontal, -1f, 1f);
        }
        
        private void HandleLaunch()
        {
            if (!_launchRequested) return;
            _launchRequested = false;
 
            Vector3 impulse = _coreSledPhysics.CalculateLaunchImpulse(transform.forward);
            _rigidbody.AddForce(impulse, ForceMode.VelocityChange);
        }
        
        public void RequestLaunch()
        {
            if (_coreSledPhysics.HasLaunched) return;
            _launchRequested = true;
        }
        
        public void RequestLaunchWithImpulse(Vector3 impulse)
        {
            if (_coreSledPhysics.HasLaunched) return;
    
            _coreSledPhysics.MarkLaunched(); 
            _rigidbody.AddForce(impulse, ForceMode.VelocityChange);
        }
        
        public void ApplyUpgradeModifiers(in UpgradeModifiers modifiers)                                                                                                            
        {                                                               
            _coreSledPhysics.ApplyModifiers(modifiers.SteeringBonus, modifiers.SpeedBoostForceBonus);
        }

        
        public void ApplySlowdown(float slowdownFactor)
        {
            _rigidbody.linearVelocity *= slowdownFactor;
        }

        public void ApplyStop()
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}
