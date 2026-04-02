using LS.CharacterController.Physics.Data;
using UnityEngine;

namespace LS.CharacterController.Physics.Core
{
    public class CoreSledPhysics
    {
        private readonly PhysicsSettings _physicsSettings;
        
        private bool _hasLaunched;
        
        public bool HasLaunched => _hasLaunched;
        
        public CoreSledPhysics(PhysicsSettings physicsSettings)
        {
            _physicsSettings = physicsSettings;
        }
        
        public Vector3 CalculateLaunchImpulse(Vector3 forward)
        {
            _hasLaunched = true;

            float power = 50.0f;
 
            Vector3 launchDirection = (forward + Vector3.down).normalized;
            return launchDirection * power;
        }
    }
}
