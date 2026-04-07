using LS.Meta;
using UnityEngine;

namespace LS.Save
{
    public interface ISaveSystem
    {
        float BestDistance { get; set; }
        int LoadCoins();                
        bool TryToSpendCoins(int amount);                                                                                                                                         
        void AddCoins(int amount);     
        void SaveUpgradeLevel(UpgradeType type, int level);                                                                                                                     
        int LoadUpgradeLevel(UpgradeType type);            
        void SaveCollectible(int resourceId, int itemId);                                                                                                                       
        bool IsCollectibleCollected(int resourceId, int itemId);

    }
}
