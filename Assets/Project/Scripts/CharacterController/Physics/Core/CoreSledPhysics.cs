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
            result.TotalForce += CalculateSlopeGravityForce(ground);
            result.TotalForce += CalculateGroundStickForce(ground);
            result.TotalForce += CalculateAirDrag(ground);
            result.TotalForce += CalculateAirGravity(ground);
            return result;
        }
        
        private Vector3 CalculateSteeringForce(GroundInfo ground, float steerInput)
        {
            if (!ground.IsGrounded || Mathf.Approximately(steerInput, 0f)) return Vector3.zero;
            if (_currentSpeed < _physicsSettings.CharacterPhysics.StopSteeringThreshold) return Vector3.zero;  

            Vector3 forwardReference  = _currentVelocity.sqrMagnitude > 0.5f
                ? _currentVelocity.normalized : Vector3.forward;

            Vector3 slopeForward = Vector3.ProjectOnPlane(forwardReference, ground.SurfaceNormal).normalized;
            Vector3 slopeRight   = Vector3.Cross(ground.SurfaceNormal, slopeForward).normalized;

            float steerForce = (_physicsSettings.CharacterPhysics.SteerForce + _steeringBonus) * steerInput;

            float speedFactor     = Mathf.Clamp01(_currentSpeed / _physicsSettings.CharacterPhysics.SteerSpeedReference);
            float highSpeedDampen = 1f / (1f + _currentSpeed * _physicsSettings.CharacterPhysics.HighSpeedSteerDamping);

            return slopeRight * steerForce * speedFactor * highSpeedDampen;

        }
        
        private Vector3 CalculateFrictionForce(GroundInfo ground)                                                                                                                   
        {                                                        
            if (!ground.IsGrounded || _currentVelocity.sqrMagnitude < 0.1f) return Vector3.zero;                                                                                    
                                                                                                                                                                              
            float friction = ground.SurfaceType == SurfaceType.Ice
                ? _physicsSettings.SurfacePhysics.IceFrictionForce                                                                                                                                 
                : _physicsSettings.SurfacePhysics.SnowFrictionForce;
                                                                                                                                                                              
            return -_currentVelocity * friction;
        }    
        
        private Vector3 CalculateSlopeGravityForce(GroundInfo ground)
        {
            if (!ground.IsGrounded) return Vector3.zero;
            return Vector3.ProjectOnPlane(UnityEngine.Physics.gravity, ground.SurfaceNormal)
                   * _physicsSettings.CharacterPhysics.SlopeGravityMultiplier;
        }
        
        private Vector3 CalculateGroundStickForce(GroundInfo ground)
        {
            if (!ground.IsGrounded) return Vector3.zero;
            return -ground.SurfaceNormal * _physicsSettings.CharacterPhysics.GroundStickForce;
        }
        
        private Vector3 CalculateAirGravity(GroundInfo ground)
        {                                                                                                                                                                           
            if (ground.IsGrounded) return Vector3.zero;                                                                                                                             
            return Vector3.down * (-UnityEngine.Physics.gravity.y) * (_physicsSettings.CharacterPhysics.AirGravityStickForce - 1f);                                                                  
        }         

        private Vector3 CalculateAirDrag(GroundInfo ground)
        {
            if (ground.IsGrounded || _currentVelocity.sqrMagnitude < 0.1f) return Vector3.zero;
            return -_currentVelocity * _physicsSettings.CharacterPhysics.AirDragForce;

        }


        
        public Vector3 CalculateLaunchImpulse(Vector3 forward)
        {
            _hasLaunched = true;
            Vector3 launchDirection = (forward + Vector3.down).normalized;
            return launchDirection * _physicsSettings.SlingshotPhysics.MaxForce;
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
