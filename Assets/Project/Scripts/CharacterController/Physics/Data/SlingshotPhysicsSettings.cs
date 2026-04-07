using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "SlingshotPhysicsSettings", menuName = "Physics/Data/SlingshotPhysicsSettings")]
    public class SlingshotPhysicsSettings : ScriptableObject
    {
        public float MaxPullDistance = 10.0f;
        public float LaunchPullExponent = 2.0f;
        public float MinForce = 5.0f;
        public float MaxForce = 30.0f;
        public float MaxSpeed = 40f;
        public float SlopeGravityMultiplier = 1f;
        public float GroundStickForce = 8f;
        public float AirGravityStickForce = 2.5f;
        public float AirDragForce = 1f;
        public float HighSpeedSteerDamping = 0.05f;
        public float VisualAlignSpeed = 12f;
        public float MaxBankAngle = 20f;
        public float ForwardInterpolationSpeed = 10.0f;
        public float BackwardInterpolationSpeed = 5.0f;
    }
}
