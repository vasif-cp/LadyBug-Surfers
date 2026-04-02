using UnityEngine;
using LS.CharacterController.Physics.Data;

namespace LS.CharacterController.Physics.Core
{
    public class GroundDetector : MonoBehaviour
    {
        [SerializeField] private PhysicsSettings _physicsSettings;
        
        private Transform _rayOrigin;
        private GroundInfo _currentGround;
        
        private void Awake()
        {
            _rayOrigin = transform;
        }
        
        public GroundInfo DetectGround()
        {
            if (UnityEngine.Physics.Raycast(_rayOrigin.position, Vector3.down, out RaycastHit hit, _physicsSettings.GroundCheckDistance, _physicsSettings.GroundLayerMask))
            {
                _currentGround = new GroundInfo
                {
                    IsGrounded = true,
                    SurfaceNormal = hit.normal,
                    HitPoint = hit.point,
                    SlopeAngle = Vector3.Angle(hit.normal, Vector3.up)
                };
            }
 
            return _currentGround;
        }
    }
}
