using System;
using LS.Core;
using LS.Events;
using LS.Gameplay;
using LS.Save;
using UnityEngine;

namespace LS.Meta
{
    public class MetaGameController : MonoBehaviour, IInjectable
    {
        private ISaveSystem _saveSystem;
        
        public void Inject(IGameContext context)
        {
            _saveSystem = context.SaveSystem;
        }

        private void Awake()
        {
            GameEvents.OnSessionEnded += OnSessionEnded;
        }

        private void OnDestroy()
        {
            GameEvents.OnSessionEnded -= OnSessionEnded;

        }

        private void OnSessionEnded(GameplaySession session)
        {
            _saveSystem.AddCoins(session.EarnedCoins);                                                                                                                               
            GameEvents.OnCoinsBalanceUpdated?.Invoke(_saveSystem.LoadCoins());
        }
    }
}
