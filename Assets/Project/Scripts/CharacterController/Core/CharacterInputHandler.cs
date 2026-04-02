using UnityEngine;

namespace LS.CharacterController.Core
{
    public class CharacterInputHandler : MonoBehaviour
    {
        private CharacterMovementController _characterMovementController;
        
        private void Awake()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
        }
        
        private void Update()
        {
            if (!_characterMovementController.HasLaunched)
            {
                HandleLaunchInput();
            }
            else
            {
                HandleSteeringInput();
            }
        }

        private void HandleLaunchInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _characterMovementController.RequestLaunch();
            }
        }
        
        private void HandleSteeringInput()
        {
            float steerInput = Input.GetAxis("Horizontal");
            _characterMovementController.SetSteerInput(steerInput);
        }

    }
}
