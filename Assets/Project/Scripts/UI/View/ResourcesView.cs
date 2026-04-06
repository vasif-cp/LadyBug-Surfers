using LS.Events;
using LS.Gameplay;
using LS.Save;
using TMPro;
using UnityEngine;

namespace LS.UI.View
{
    public class ResourcesView : UIViewBase
    {
        [SerializeField] private TMP_Text _coinsText;
        
        private void Awake()
        {
            GameEvents.OnSessionEnded += OnSessionEnded;
            GameEvents.OnGameStartRequested += Hide;
            GameEvents.OnCoinsBalanceUpdated += Refresh;
        }

        private void OnDestroy()
        {
            GameEvents.OnSessionEnded -= OnSessionEnded;
            GameEvents.OnGameStartRequested -= Hide;
            GameEvents.OnCoinsBalanceUpdated -= Refresh;                                                                                                                     
        }
                                                                                                                                                                              
        private void Start()
        {
            Refresh(SaveSystem.LoadCoins());
            Show();
        }

        private void OnSessionEnded(GameplaySession session)
        {
            Show();
        }

        private void Refresh(int amount)                                                                                                                                    
        {
            _coinsText.SetText("{0}", amount);                                                                                                                              
        }  
    }
}
