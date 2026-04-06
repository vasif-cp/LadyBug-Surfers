using System;
using LS.Events;
using UnityEngine;

namespace LS.CharacterController.Core
{
    public class CharacterInputHandler : MonoBehaviour
    {
        [SerializeField] private FloatingJoystick _joystick;
        
        private CharacterMovementController _characterMovementController;
        
        private void Awake()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            _joystick.gameObject.SetActive(false);

            GameEvents.OnPullEnded += OnLaunched;
        }

        private void OnDestroy()
        {
            GameEvents.OnPullEnded -= OnLaunched;
        }

        private void Update()
        {
            if (_characterMovementController.HasLaunched)
            {
                HandleSteeringInput();
            }
        }

        private void OnLaunched() => _joystick.gameObject.SetActive(true);
        
        private void HandleSteeringInput()
        {
            _characterMovementController.SetSteerInput(_joystick.Horizontal);
        }

    }
}
