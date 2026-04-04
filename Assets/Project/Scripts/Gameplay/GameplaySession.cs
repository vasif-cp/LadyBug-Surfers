using UnityEngine;

namespace LS.Gameplay
{
    public class GameplaySession
    {
        private Vector3 _startPosition;
        private Transform _character;
        
        public float TravelledDistance { get; private set; }
        public int CollectedCoins { get; private set; }
        public bool IsActive { get; private set; }
        
        public GameplaySession(Transform character)
        {
            _character = character;
            _startPosition = character.position;
        }

        public void OnStart()
        {
            _startPosition = _character.position;
            TravelledDistance = 0f;
            CollectedCoins = 0;
            IsActive = true;
        }

        public void OnUpdate()
        {
            if (!IsActive) return;
            TravelledDistance = Vector3.Distance(_startPosition, _character.position);
        }
        
        public void OnEnd()
        {
            IsActive = false;
        }
        
        public void AddCoins(int collectedAmount)
        {
            CollectedCoins += collectedAmount;
        }
    }
}
