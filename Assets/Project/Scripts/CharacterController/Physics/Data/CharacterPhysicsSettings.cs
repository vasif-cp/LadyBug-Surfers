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
        public float StopSteeringThreshold = 0.5f;
        public float LaunchConfirmThreshold = 2f;
        public float SlopeGravityMultiplier = 1f;
        public float GroundStickForce = 8f;
        public float AirGravityStickForce = 2.5f;
        public float AirDragForce = 1f;
        public float HighSpeedSteerDamping = 0.05f;
        public float VisualAlignSpeed = 12f;
        public float MaxBankAngle = 20f;

    }
}
