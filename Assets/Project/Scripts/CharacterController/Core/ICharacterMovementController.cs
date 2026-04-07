using UnityEngine;

namespace LS.CharacterController.Core
{
    public interface ICharacterMovementController
    {
        Transform CharacterTransform { get; }
        bool HasLaunched { get; }
        Vector3 Velocity { get; }

        void SetSteerInput(float horizontal);
        void ApplySlowdown(float slowdownFactor);
        void ApplyStop();
    }
}
