using System;
using UnityEngine;
using LS.CharacterController.Physics.Data;
using LS.Core;

namespace LS.CharacterController.Physics.Core
{
    public class GroundDetector : MonoBehaviour, IInjectable
    {
        private Transform _rayOrigin;
        private GroundInfo _currentGround;
        private TerrainSurfaceDetector _surfaceDetector; 
        private PhysicsSettings _physicsSettings;

        public void Inject(IGameContext context)
        {
            _physicsSettings = context.PhysicsSettings;
        }

        private void Awake()
        {
            _rayOrigin = transform;
        }

        private void Start()
        {
            _surfaceDetector = new TerrainSurfaceDetector(_physicsSettings.SurfacePhysics);
        }

        public GroundInfo DetectGround()
        {
            if (UnityEngine.Physics.Raycast(_rayOrigin.position, Vector3.down, out RaycastHit hit, _physicsSettings.CharacterPhysics.GroundCheckDistance, _physicsSettings.SurfacePhysics.GroundLayerMask))
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
