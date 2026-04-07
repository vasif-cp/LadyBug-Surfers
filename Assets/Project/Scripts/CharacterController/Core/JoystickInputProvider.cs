using System;
using UnityEngine;

namespace LS.Core
{
    public class JoystickInputProvider : MonoBehaviour, IInputProvider, IContextService
    {
        private FloatingJoystick _joystick;

        public float HorizontalInput => _joystick.Horizontal;
        public float VerticalInput => _joystick.Vertical;

        public void Register(ServiceRegistry registry)
        {
            registry.Register<IInputProvider>(this);
        }

        private void Awake()
        {
            _joystick = GetComponent<FloatingJoystick>();
        }
    }
}
