using LS.CharacterController.Core;
using LS.Events;
using UnityEngine;

namespace LS.Items.Obstacles
{
    public enum ObstacleType {SlowingDown, Stop}
    public abstract class BaseObstacle : MonoBehaviour
    {
        public abstract ObstacleType Type { get; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<ICharacterMovementController>() != null)
            {
                GameEvents.OnObstacleHit?.Invoke(Type);
                gameObject.SetActive(Type != ObstacleType.SlowingDown);
            }
        }

    }
}
