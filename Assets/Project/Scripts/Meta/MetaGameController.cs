using System;
using LS.Events;
using LS.Gameplay;
using LS.Save;
using UnityEngine;

namespace LS.Meta
{
    public class MetaGameController : MonoBehaviour
    {
        [SerializeField] private UpgradeSettings _upgradeSettings;
        
        private UpgradeManager _upgradeManager;
        
        public UpgradeManager UpgradeManager => _upgradeManager;

        private void Awake()
        {
            _upgradeManager = new UpgradeManager(_upgradeSettings);
            GameEvents.OnSessionEnded += OnSessionEnded;
        }

        private void OnDestroy()
        {
            GameEvents.OnSessionEnded -= OnSessionEnded;

        }

        private void OnSessionEnded(GameplaySession session)
        {
            SaveSystem.AddCoins(session.EarnedCoins);                                                                                                                               
            GameEvents.OnCoinsBalanceUpdated?.Invoke(SaveSystem.LoadCoins());
        }
    }
}
