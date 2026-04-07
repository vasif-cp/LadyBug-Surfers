using LS.Core;
using LS.Events;
using LS.Items.Collectibles;
using UnityEngine;

namespace LS.CharacterController.Physics
{
    public class CollectibleDetector : MonoBehaviour, IInjectable
    {
        private float _radius = 3.0f;
        private LayerMask _collectibleLayer;
        
        private readonly Collider[] _hitBuffer = new Collider[10];

        public void Inject(IGameContext context)
        {
            _radius = context.PhysicsSettings.CollectiblePhysics.CollectibleRadius;
            _collectibleLayer = context.PhysicsSettings.CollectiblePhysics.CollectibleLayer;
        }

        private void FixedUpdate()
        {
            int count = UnityEngine.Physics.OverlapSphereNonAlloc(transform.position, _radius, 
                _hitBuffer, _collectibleLayer);

            
            for (int i = 0; i < count; i++)
            {
                if (_hitBuffer[i].TryGetComponent(out BaseCollectible collectible))
                {
                    GameEvents.OnCollectibleCollected?.Invoke(collectible.ResourceID, collectible.UniqueId, collectible.Value);
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
#endif
    }
}
