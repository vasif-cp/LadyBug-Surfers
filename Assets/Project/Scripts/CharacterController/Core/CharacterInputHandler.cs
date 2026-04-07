using System;
using LS.Core;
using LS.Events;
using UnityEngine;

namespace LS.CharacterController.Core
{
    public class CharacterInputHandler : MonoBehaviour, IInjectable
    {
        private IInputProvider _inputProvider;
        private ICharacterMovementController _characterMovementController;

        public void Inject(IGameContext context)
        {
            _inputProvider = context.InputProvider;
            _characterMovementController = context.CharacterMovementController;
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
            _characterMovementController.SetSteerInput(_inputProvider.HorizontalInput);
        }

    }
}
