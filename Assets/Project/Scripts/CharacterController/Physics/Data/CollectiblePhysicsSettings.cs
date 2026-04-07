using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "CollectiblePhysicsSettings", menuName = "Physics/Data/CollectiblePhysicsSettings")]
    public class CollectiblePhysicsSettings : ScriptableObject
    {
        public float CollectibleRadius = 3.0f;
        public LayerMask CollectibleLayer;
    }
}
