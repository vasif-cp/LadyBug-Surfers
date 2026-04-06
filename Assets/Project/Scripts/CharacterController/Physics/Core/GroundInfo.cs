using UnityEngine;

namespace LS.CharacterController.Physics.Core
{
    public struct GroundInfo
    {
        public bool IsGrounded;
        public SurfaceType SurfaceType;
        public Vector3 SurfaceNormal;
        public Vector3 HitPoint;
        public float SlopeAngle;
    }
}
