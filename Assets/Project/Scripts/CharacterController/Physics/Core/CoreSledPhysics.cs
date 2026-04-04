using LS.CharacterController.Physics.Data;
using UnityEngine;

namespace LS.CharacterController.Physics.Core
{
    public class CoreSledPhysics
    {
        private readonly PhysicsSettings _physicsSettings;
        
        private bool _hasLaunched;
        private Vector3 _currentVelocity;
        private float _currentSpeed;
        
        public bool HasLaunched => _hasLaunched;
        
        public CoreSledPhysics(PhysicsSettings physicsSettings)
        {
            _physicsSettings = physicsSettings;
        }
        
        public ForceResult CalculateForces(Vector3 currentVelocity, GroundInfo ground, float steerInput)
        {
            _currentVelocity = currentVelocity;
            _currentSpeed = _currentVelocity.magnitude;
            
            var result = new ForceResult();
 
            result.TotalForce += CalculateSteeringForce(ground, steerInput);
 
            result.HasTargetRotation = true;
            return result;
        }
        
        private Vector3 CalculateSteeringForce(GroundInfo ground, float steerInput)
        {
            if (!ground.IsGrounded || Mathf.Approximately(steerInput, 0f))
                return Vector3.zero;
 
            Vector3 forwardReference = _currentVelocity.sqrMagnitude > 0.5f ? _currentVelocity.normalized : Vector3.forward;

            Vector3 slopeForward = Vector3.ProjectOnPlane(forwardReference, ground.SurfaceNormal).normalized;
            Vector3 slopeRight = Vector3.Cross(ground.SurfaceNormal, slopeForward).normalized;

            float steerForce = _physicsSettings.SteerForce * steerInput;
            float speedFactor = Mathf.Clamp01(_currentVelocity.magnitude / _physicsSettings.SteerSpeedReference);

            return slopeRight * steerForce * speedFactor;
        }
        
        public Vector3 CalculateLaunchImpulse(Vector3 forward)
        {
            _hasLaunched = true;

            float power = 50.0f;
 
            Vector3 launchDirection = (forward + Vector3.down).normalized;
            return launchDirection * power;
        }
        
        public void MarkLaunched() => _hasLaunched = true;
    }
}
