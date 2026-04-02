using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "PhysicsSettings", menuName = "Physics/Data/PhysicsSettings")]
    public class PhysicsSettings : ScriptableObject
    {
        public float SteerForce = 10f;
        public float SteerSpeedReference = 5f;
        public float GroundCheckDistance = 2f;
        public LayerMask GroundLayerMask;
    }
}
