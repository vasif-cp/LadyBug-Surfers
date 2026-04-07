using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "CharacterPhysicsSettings", menuName = "Physics/Data/CharacterPhysicsSettings")]
    public class CharacterPhysicsSettings : ScriptableObject
    {
        public float SteerForce = 10f;
        public float SteerSpeedReference = 5f;
        public float GroundCheckDistance = 2f;
        public float ObstacleSlowFactor = 0.5f;
        public float StopSpeedThreshold = 0.25f;
        public float LaunchConfirmThreshold = 2f;

    }
}
