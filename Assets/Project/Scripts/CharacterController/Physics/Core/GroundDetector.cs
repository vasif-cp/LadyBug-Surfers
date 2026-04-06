using UnityEngine;
using LS.CharacterController.Physics.Data;

namespace LS.CharacterController.Physics.Core
{
    public class GroundDetector : MonoBehaviour
    {
        [SerializeField] private PhysicsSettings _physicsSettings;
        [SerializeField] private TerrainSurfaceSettings _terrainSurfaceSettings;
        
        private Transform _rayOrigin;
        private GroundInfo _currentGround;
        private TerrainSurfaceDetector _surfaceDetector; 
        
        private void Awake()
        {
            _rayOrigin = transform;
            _surfaceDetector = new TerrainSurfaceDetector(_terrainSurfaceSettings);
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
                    SlopeAngle = Vector3.Angle(hit.normal, Vector3.up),
                    SurfaceType = ResolveSurfaceType(hit)
                };
            }
            else
            {
                _currentGround = default;
            }
 
            return _currentGround;
        }
        
        private SurfaceType ResolveSurfaceType(RaycastHit hit)
        {                               
            if (hit.collider is TerrainCollider)
            {                                                                                                                                                               
                Terrain terrain = hit.collider.GetComponent<Terrain>();
                if (terrain != null)                                                                                                                                        
                    return _surfaceDetector.GetSurfaceType(hit.point, terrain);
            }

            return SurfaceType.Air;
        }  
    }
}
