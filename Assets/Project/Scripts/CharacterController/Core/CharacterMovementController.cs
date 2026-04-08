using System;
using LS.CharacterController.Physics.Data;
using LS.CharacterController.Physics.Core;
using LS.Core;
using LS.Events;
using LS.Meta;
using UnityEngine;

namespace LS.CharacterController.Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundDetector))]
    public class CharacterMovementController : MonoBehaviour, IInjectable, ICharacterMovementController, IContextService
    {
        [Header("Visual Dependencies")]
        [SerializeField] private Transform _visualModelTransform;
        
        private Rigidbody _rigidbody;
        private GroundDetector _groundDetector;
        private CoreSledPhysics _coreSledPhysics;
        private PhysicsSettings _physicsSettings;
        
        private float _steerInput;
        private bool _launchRequested;

        public Transform CharacterTransform => transform;
        public bool HasLaunched => _coreSledPhysics.HasLaunched;
        public Vector3 Velocity => _rigidbody.linearVelocity;

        public void Inject(IGameContext context)
        {
            _physicsSettings = context.PhysicsSettings;
        }
        
        public void Register(ServiceRegistry registry)
        {
            registry.Register<ICharacterMovementController>(this);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _groundDetector = GetComponent<GroundDetector>();

            GameEvents.OnUpgradeModifiersApplied += ApplyUpgradeModifiers;
            GameEvents.OnLaunchRequested += RequestLaunchWithImpulse;
        }

        private void Start()
        {
            _coreSledPhysics = new CoreSledPhysics(_physicsSettings);
        }

        private void OnDestroy()
        {
            GameEvents.OnUpgradeModifiersApplied -= ApplyUpgradeModifiers;
            GameEvents.OnLaunchRequested -= RequestLaunchWithImpulse;
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

                float bankAngle = -_steerInput * _physicsSettings.CharacterPhysics.MaxBankAngle;
                Quaternion bankRotation = Quaternion.AngleAxis(bankAngle, forward);

                Quaternion targetRotation = bankRotation * slopeRotation;
                _visualModelTransform.rotation = Quaternion.Slerp(
                    _visualModelTransform.rotation, targetRotation,
                    Time.fixedDeltaTime * _physicsSettings.CharacterPhysics.VisualAlignSpeed);
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
        
        public void RequestLaunchWithImpulse(Vector3 impulse)
        {
            if (_coreSledPhysics.HasLaunched) return;
    
            _coreSledPhysics.MarkLaunched(); 
            _rigidbody.AddForce(impulse, ForceMode.VelocityChange);
        }
        
        public void ApplyUpgradeModifiers(UpgradeModifiers modifiers)                                                                                                            
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
