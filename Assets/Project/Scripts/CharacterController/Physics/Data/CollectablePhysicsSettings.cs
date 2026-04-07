using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "CollectablePhysicsSettings", menuName = "Physics/Data/CollectablePhysicsSettings")]
    public class CollectablePhysicsSettings : ScriptableObject
    {
        public float CollectableRadius = 1.0f;
        public LayerMask CollectableLayer;
    }
}
