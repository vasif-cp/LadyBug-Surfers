using LS.Meta;
using UnityEngine;

namespace LS.Save
{
    public static class SaveSystem
    {
        private const string KeyBestDistance = "GameplayData:BestDistance";
        private const string KeyCollectibles = "GameplayData:Collectibles";
        private const string KeyUpgrade = "Upgrade:Level";

        public static int BestDistance
        {
            get => PlayerPrefs.GetInt(KeyBestDistance, 0);
            set
            {
                PlayerPrefs.SetInt(KeyBestDistance, value);
                PlayerPrefs.Save();
            }
        }

        #region Collectibles
        public static void SaveCollectible(int resourceID, int itemID)
        {
            PlayerPrefs.SetInt(CollectibleKey(resourceID, itemID), 1);
        }
        
        public static bool IsCollectibleCollected(int resourceID, int itemID) =>
            PlayerPrefs.GetInt(CollectibleKey(resourceID, itemID), 0) == 1;

        
        private static string CollectibleKey(int resourceID, int itemID) =>
            $"{KeyCollectibles}:{resourceID}:{itemID}";

        #endregion
        
        #region Meta Upgrades
        public static int LoadUpgradeLevel(UpgradeType type) =>
            PlayerPrefs.GetInt($"{KeyUpgrade}:{(int)type}", 0);   
        
        public static void SaveUpgradeLevel(UpgradeType type, int level) =>
            PlayerPrefs.SetInt($"{KeyUpgrade}:{(int)type}", level);    
        #endregion
        
    }
}
