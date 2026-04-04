using System;
using LS.CharacterController.Physics.Data;
using LS.CharacterController.Physics.Core;
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
