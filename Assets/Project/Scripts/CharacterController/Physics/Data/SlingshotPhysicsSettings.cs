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
        public float ForwardInterpolationSpeed = 10.0f;
        public float BackwardInterpolationSpeed = 5.0f;
        public float BehindDistance = 2f;
    }
}
