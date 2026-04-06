using LS.CharacterController.Physics.Data;
using UnityEngine;

namespace LS.Items.Slingshot
{
    public class SlingshotEngine
    {
        public enum State { Idle, Pulling, Launched }

        private PhysicsSettings _physicsSettings;
        
        private State _state;
        private float _pullAmount;     
        private Vector3 _pullOffset;   
        private Vector3 _restPosition;
        
        public float PullAmount => _pullAmount;
        public bool IsIdle => _state == State.Idle;
        public bool IsPulling => _state == State.Pulling;
        public bool IsLaunched => _state == State.Launched;
        
        
        public SlingshotEngine(PhysicsSettings physicsSettings)
        {
            _physicsSettings = physicsSettings;
            _state = State.Idle;
        }
        
        public void BeginPull(Vector3 currentSledPosition)
        {
            if (_state != State.Idle) return;
 
            _state = State.Pulling;
            _restPosition = currentSledPosition;
            _pullAmount = 0f;
            _pullOffset = Vector3.zero;
        }
        
        public Vector3 UpdatePull(Vector3 inputWorldPosition, Vector3 sledForward)
        {
            if (_state != State.Pulling)
                return _restPosition;

            float maxPullDistance = _physicsSettings.MaxPullDistance;
            Vector3 rawOffset = inputWorldPosition - _restPosition;
 
            Vector3 backward = -sledForward.normalized;
            float backwardDistance = Vector3.Dot(rawOffset, backward);
            backwardDistance = Mathf.Clamp(backwardDistance, 0f, maxPullDistance);
 
            _pullOffset = backward * backwardDistance;
            _pullAmount = backwardDistance / maxPullDistance;
 
            return _restPosition + _pullOffset;
        }
        
        public Vector3 Release(Vector3 sledForward, float launchPowerMultiplier = 1f)
        {
            if (_state != State.Pulling)
                return Vector3.zero;
 
            _state = State.Launched;

            float pullExponent = _physicsSettings.LaunchPullExponent;
            float curvedPull = Mathf.Pow(_pullAmount, pullExponent);

            float minLaunchForce = _physicsSettings.MinForce;
            float maxLaunchForce = _physicsSettings.MaxForce;
            float launchPower = Mathf.Lerp(minLaunchForce, maxLaunchForce, curvedPull) * launchPowerMultiplier;
            
            Debug.LogError("Launching with power: " + launchPower);
            _pullAmount = 0f;
            _pullOffset = Vector3.zero;
 
            return sledForward.normalized * launchPower;
        }
        
        public void Reset()
        {
            _state = State.Idle;
            _pullAmount = 0f;
            _pullOffset = Vector3.zero;
        }
    }
}
