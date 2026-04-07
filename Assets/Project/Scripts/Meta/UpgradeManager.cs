using LS.Events;
using LS.Save;
using UnityEngine;

namespace LS.Meta
{
    public class UpgradeManager : IUpgradeManager
    {
        private ISaveSystem _saveSystem;
        private readonly UpgradeSettings _settings;
        private readonly int[] _levels;
        
        public UpgradeManager(UpgradeSettings settings, ISaveSystem saveSystem)
        {
            _settings = settings;
            _saveSystem = saveSystem;
                                                                                                                                                                              
            _levels = new int[_settings.UpgradeCount];
            for (int i = 0; i < _levels.Length; i++)                                                                                                                        
                _levels[i] = _saveSystem.LoadUpgradeLevel((UpgradeType)i);
        }
        
        public int GetLevel(UpgradeType type) => _levels[(int)type];                                                                                                        
   
        public bool IsMaxLevel(UpgradeType type) =>                                                                                                                         
            _levels[(int)type] >= _settings.Get(type).MaxLevel;

        public int GetNextPrice(UpgradeType type)                                                                                                                           
        {
            var def = _settings.Get(type);                                                                                                                                  
            return def.BasePrice + def.PriceIncreasePerLevel * _levels[(int)type];
        }                                                                                                                                                                   
   
        public string GetDisplayName(UpgradeType type) => _settings.Get(type).DisplayName;            
        
        public Sprite GetIcon(UpgradeType type) => _settings.Get(type).Icon;
                  
        public bool TryPurchase(UpgradeType type)
        {
            if (IsMaxLevel(type)) return false;      
            if (!_saveSystem.TryToSpendCoins(GetNextPrice(type))) return false;
                                                                                                                                                                              
            _levels[(int)type]++;
            _saveSystem.SaveUpgradeLevel(type, _levels[(int)type]);                                                                                                          
            GameEvents.OnUpgradePurchased?.Invoke(type);
            GameEvents.OnCoinsBalanceUpdated?.Invoke(_saveSystem.LoadCoins());                                                                                                        
            return true;                                                                                                                                                    
        }
        
        public UpgradeModifiers GetModifiers()
        {
            return new UpgradeModifiers
            {
                LaunchPowerBonus      = GetValue(UpgradeType.LaunchPower),
                SpeedBoostForceBonus  = GetValue(UpgradeType.Speed),                                                                                                        
                SteeringBonus         = GetValue(UpgradeType.Steering),
                CollectibleValueBonus = (int)GetValue(UpgradeType.CollectibleValue)                                                                                         
            };  
        }                                                                                                                                                                   
                  
        private float GetValue(UpgradeType type) =>                                                                                                                         
            _settings.Get(type).ValuePerLevel * _levels[(int)type];




    }
}
