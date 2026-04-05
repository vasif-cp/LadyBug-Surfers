using LS.Meta;
using LS.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace LS.UI.View
{
    public class MainMenuView : UIViewBase
    {
        [Header("Scene Dependencies")]
        [SerializeField] private MetaGameController _metaGameController;
                                                                                                                                                                              
        [Header("UI Dependencies")]
        [SerializeField] private UpgradeSlotView _upgradeSlotPrefab;
        [SerializeField] private Transform _slotsContainer;                                                                         
        [SerializeField] private Button _playButton;
        

        private void Start()
        {
            Show();
            
            foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
            {
                var upgradeSlotView = Instantiate(_upgradeSlotPrefab, _slotsContainer);
                upgradeSlotView.Initialize(_metaGameController.UpgradeManager, type);                                                                                                  
            }

                                                                        
            //_playButton.onClick.AddListener(OnPlayClicked);                                                                                                                 
        }
        
        private void OnPlayClicked()
        {                                                                                                                                                                   
            Hide();
        }    
    }
}
