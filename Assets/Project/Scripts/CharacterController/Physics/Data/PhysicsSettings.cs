using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "PhysicsSettings", menuName = "Physics/Data/PhysicsSettings")]
    public class PhysicsSettings : ScriptableObject
    {
        [SerializeField] private CharacterPhysicsSettings _characterPhysics;
        [SerializeField] private SlingshotPhysicsSettings _slingshotPhysics;
        [SerializeField] private SurfacePhysicsSettings _surfacePhysics;
        [SerializeField] private CollectablePhysicsSettings _collectablePhysics;

        public CharacterPhysicsSettings CharacterPhysics => _characterPhysics;
        public SlingshotPhysicsSettings SlingshotPhysics => _slingshotPhysics;
        public SurfacePhysicsSettings SurfacePhysics => _surfacePhysics;
        public CollectablePhysicsSettings CollectablePhysics => _collectablePhysics;
    }
}
