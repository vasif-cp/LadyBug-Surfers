using LS.Core;
using LS.Events;
using LS.Meta;
using LS.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace LS.UI.View
{
    public class MainMenuView : UIViewBase, IInjectable
    {
        [Header("UI Dependencies")]
        [SerializeField] private UpgradeSlotView _upgradeSlotPrefab;
        [SerializeField] private Transform _slotsContainer;    
        
        private IUpgradeManager _upgradeManager;

        public void Inject(IGameContext context)
        {
            _upgradeManager = context.UpgradeManager;
        }

        private void Start()
        {
            Show();
            
            foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
            {
                var upgradeSlotView = Instantiate(_upgradeSlotPrefab, _slotsContainer);
                upgradeSlotView.Initialize(_upgradeManager, type);                                                                                                  
            }                                                                                                               
        }
        
        public void OnPlayRequested()
        {                                                                                                                                                                   
            Hide();
            GameEvents.OnGameStartRequested?.Invoke();
        }    
    }
}
