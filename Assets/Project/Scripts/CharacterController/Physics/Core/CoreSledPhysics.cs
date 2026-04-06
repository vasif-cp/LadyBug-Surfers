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
        
        private float _steeringBonus;                                                                                                                                               
        private float _speedBoostForce;
        
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
            result.TotalForce += CalculateSpeedBoost();
            result.TotalForce += CalculateFrictionForce(ground);
 
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

            float steerForce = (_physicsSettings.SteerForce + _steeringBonus) * steerInput;
            float speedFactor = Mathf.Clamp01(_currentVelocity.magnitude / _physicsSettings.SteerSpeedReference);

            return slopeRight * steerForce * speedFactor;
        }
        
        private Vector3 CalculateFrictionForce(GroundInfo ground)                                                                                                                   
        {                                                        
            if (!ground.IsGrounded || _currentVelocity.sqrMagnitude < 0.1f) return Vector3.zero;                                                                                    
                                                                                                                                                                              
            float friction = ground.SurfaceType == SurfaceType.Ice
                ? _physicsSettings.IceFrictionForce                                                                                                                                 
                : _physicsSettings.SnowFrictionForce;
                                                                                                                                                                              
            return -_currentVelocity.normalized * friction;
        }    
        
        public Vector3 CalculateLaunchImpulse(Vector3 forward)
        {
            _hasLaunched = true;

            float power = 50.0f;
 
            Vector3 launchDirection = (forward + Vector3.down).normalized;
            return launchDirection * power;
        }
        
        private Vector3 CalculateSpeedBoost()                                                                                                                                       
        {                                    
            if (_speedBoostForce <= 0f || _currentVelocity.sqrMagnitude < 0.1f) return Vector3.zero;                                                                                
            return _currentVelocity.normalized * _speedBoostForce;
        }     
        
        public void ApplyModifiers(float steeringBonus, float speedBoostForce)
        {                                                                                                                                                                           
            _steeringBonus = steeringBonus;
            _speedBoostForce = speedBoostForce;                                                                                                                                     
        }   
        
        public void MarkLaunched() => _hasLaunched = true;
    }
}
