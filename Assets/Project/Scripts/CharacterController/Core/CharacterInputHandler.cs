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
        }

        private void Update()
        {
            if (_characterMovementController.HasLaunched)
            {
                HandleSteeringInput();
            }
        }
        
        private void HandleSteeringInput()
        {
            _characterMovementController.SetSteerInput(_joystick.Horizontal);
        }

    }
}
