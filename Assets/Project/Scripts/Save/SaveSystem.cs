using UnityEngine;

namespace LS.Save
{
    public static class SaveSystem
    {
        private const string KeyBestDistance = "GameplayData:BestDistance";
        private const string KeyCollectibles = "GameplayData:Collectibles";

        public static int BestDistance
        {
            get => PlayerPrefs.GetInt(KeyBestDistance, 0);
            set
            {
                PlayerPrefs.SetInt(KeyBestDistance, value);
                PlayerPrefs.Save();
            }
        }

        public static void SaveCollectible(int resourceID, int itemID)
        {
            Debug.LogError($"Saving collectible {resourceID}:{itemID}");
            PlayerPrefs.SetInt(CollectibleKey(resourceID, itemID), 1);
        }
        
        public static bool IsCollectibleCollected(int resourceID, int itemID) =>
            PlayerPrefs.GetInt(CollectibleKey(resourceID, itemID), 0) == 1;

        
        private static string CollectibleKey(int resourceID, int itemID) =>
            $"{KeyCollectibles}:{resourceID}:{itemID}";

        
        
    }
}
