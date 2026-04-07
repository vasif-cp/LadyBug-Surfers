using UnityEngine;

namespace LS.Meta
{
    public interface IUpgradeManager
    {
        UpgradeModifiers GetModifiers();                                                                                                                                    
        bool TryPurchase(UpgradeType type);
        int GetLevel(UpgradeType type);     
        bool IsMaxLevel(UpgradeType type);  
        int GetNextPrice(UpgradeType type);
        string GetDisplayName(UpgradeType type);                                                                                                                            
        UnityEngine.Sprite GetIcon(UpgradeType type);    
    }
}
