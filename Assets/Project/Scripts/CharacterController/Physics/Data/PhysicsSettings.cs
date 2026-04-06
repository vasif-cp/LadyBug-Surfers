using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "PhysicsSettings", menuName = "Physics/Data/PhysicsSettings")]
    public class PhysicsSettings : ScriptableObject
    {
        [Header("Character Settings")]
        public float SteerForce = 10f;
        public float SteerSpeedReference = 5f;
        public float GroundCheckDistance = 2f;
        public float ObstacleSlowFactor = 0.5f;
        public float StopSpeedThreshold = 0.25f;
        public float LaunchConfirmThreshold = 2f;
        public LayerMask GroundLayerMask;

        [Space(10)] 
        
        [Header("Slingshot Settings")]
        public float MaxPullDistance = 10.0f;
        public float LaunchPullExponent = 2.0f;
        public float MinForce = 5.0f;
        public float MaxForce = 30.0f;
        public float MaxSpeed                = 40f;
        public float SlopeGravityMultiplier  = 1f;
        public float GroundStickForce        = 8f;
        public float AirDragForce            = 1f;
        public float HighSpeedSteerDamping   = 0.05f; 
        public float VisualAlignSpeed        = 12f;
        public float MaxBankAngle            = 20f;
        public float ForwardInterpolationSpeed = 10.0f;
        public float BackwardInterpolationSpeed = 5.0f;
        
        [Space(10)] 
        
        [Header("Surface Settings")]
        public LayerMask IceLayerMask;
        public float SnowFrictionForce = 5f;                                                                                                                                        
        public float IceFrictionForce  = 2.5f; 
        
    }
}
