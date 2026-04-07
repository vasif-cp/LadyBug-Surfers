using LS.Meta;
using LS.Save;
using UnityEngine;

namespace LS.Gameplay
{
    public class GameplaySession
    {
        private UpgradeModifiers _upgradeModifiers;
        private ISaveSystem _saveSystem;
        private Vector3 _startPosition;
        private Transform _character;
        
        public float TravelledDistance { get; private set; }
        public int CollectedCoins { get; private set; }
        public int EarnedCoins { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsBestScore { get; private set; }
        
        public GameplaySession(UpgradeModifiers upgradeModifiers, Transform character, ISaveSystem saveSystem)
        {
            _character = character;
            _startPosition = character.position;
            _upgradeModifiers = upgradeModifiers;
            _saveSystem = saveSystem;
        }

        public void OnStart()
        {
            _startPosition = _character.position;
            TravelledDistance = 0f;
            CollectedCoins = 0;
            EarnedCoins = 0;
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
            CalculateEarnedCoins();
            CheckPossibleBestScore();
        }
        
        public void AddCoins(int collectedAmount)
        {
            CollectedCoins += collectedAmount;
        }

        private void CalculateEarnedCoins()
        {
            const float baseCoinsPerMeter = 0.5f;
            const int collectibleBonusCoins = 50;
            float coinRate = baseCoinsPerMeter + _upgradeModifiers.CollectibleValueBonus;                                                                         
            int distanceCoins = Mathf.FloorToInt(TravelledDistance * coinRate);
            int collectibleCoins = CollectedCoins * collectibleBonusCoins;
                                              
            EarnedCoins = distanceCoins + collectibleCoins;
        }

        private void CheckPossibleBestScore()
        {
            if (TravelledDistance <= _saveSystem.BestDistance) return;

            IsBestScore = true;
            _saveSystem.BestDistance = TravelledDistance;

        }
    }
}
