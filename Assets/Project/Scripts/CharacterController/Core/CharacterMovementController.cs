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
            }
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
    }
}
