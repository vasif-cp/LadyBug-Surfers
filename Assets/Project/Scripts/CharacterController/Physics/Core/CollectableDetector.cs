using LS.Events;
using LS.Items.Collectibles;
using UnityEngine;

namespace LS.CharacterController.Physics
{
    public class CollectableDetector : MonoBehaviour
    {
        [Header("Detector Settings")]
        [SerializeField] private float _radius = 3.0f;
        [SerializeField] private LayerMask _collectibleLayer;
        
        private readonly Collider[] _hitBuffer = new Collider[10]; // pre-allocated, zero GC

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
