using LS.Events;
using LS.Meta;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LS.UI.View
{
    public class UpgradeSlotView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;                                                                                                                        
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _priceText;                                                                                                                       
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _upgradeButtonText; 
        
        private UpgradeManager _upgradeManager;   
        private UpgradeType _upgradeType;
        
        public void Initialize(UpgradeManager upgradeManager, UpgradeType upgradeType)                                                                                                                                                                                                
        {
            _upgradeManager = upgradeManager;      
            _upgradeType = upgradeType;
            _iconImage.sprite = _upgradeManager.GetIcon(_upgradeType);                                                                                                        
            _nameText.SetText(_upgradeManager.GetDisplayName(_upgradeType));
            
            _upgradeButton.onClick.AddListener(OnUpgradeClicked);                                                                                                     
                                                                                                                                                                              
            GameEvents.OnUpgradePurchased += OnUpgradePurchased;      
                                                                                                                                                                              
            Refresh();
        }   
        
        private void OnDestroy()
        {
            GameEvents.OnUpgradePurchased -= OnUpgradePurchased;                                                                                                     
        }

        private void OnUpgradeClicked()
        {
            _upgradeManager.TryPurchase(_upgradeType);
        }

        private void OnUpgradePurchased(UpgradeType type)                                                                                                                   
        {
            if (type == _upgradeType) Refresh();                                                                                                                            
        }
   
        private void Refresh()                                                                                                                                              
        {       
            bool isMax  = _upgradeManager.IsMaxLevel(_upgradeType);
            int  level  = _upgradeManager.GetLevel(_upgradeType);
            int  price  = _upgradeManager.GetNextPrice(_upgradeType);     
                                                                                                                                                                              
            _levelText.SetText("{0}", level);
            _upgradeButton.interactable = !isMax;            
            
            _priceText.SetText(isMax ? string.Empty : "<sprite index=0>{0}", price);    
            
            _upgradeButtonText.SetText(isMax ? "MAX" : "Upgrade");                                                                                                          
        }  

    }
}
