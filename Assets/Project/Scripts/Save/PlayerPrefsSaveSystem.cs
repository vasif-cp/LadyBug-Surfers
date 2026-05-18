using System;
using LS.Meta;
using UnityEngine;

namespace LS.Save
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        private const string KeyBestDistance = "GameplayData:BestDistance";
        private const string KeyCollectibles = "GameplayData:Collectibles";
        private const string KeyUpgrade = "Upgrade:Level";
        private const string KeyCoins = "Resource:Coins";
        private const string KeyDailyRewardLastClaimDate = "Rewards:Daily:LastClaimDate";

        private const int DailyRewardMaxCoins = 200;
        private const int DailyRewardStepCoins = 50;

        public float BestDistance
        {
            get => PlayerPrefs.GetFloat(KeyBestDistance, 0);
            set
            {
                PlayerPrefs.SetFloat(KeyBestDistance, value);
                PlayerPrefs.Save();
            }
        }

        #region Collectibles
        public void SaveCollectible(int resourceID, int itemID)
        {
            PlayerPrefs.SetInt(CollectibleKey(resourceID, itemID), 1);
        }
        
        public bool IsCollectibleCollected(int resourceID, int itemID) =>
            PlayerPrefs.GetInt(CollectibleKey(resourceID, itemID), 0) == 1;

        
        private string CollectibleKey(int resourceID, int itemID) =>
            $"{KeyCollectibles}:{resourceID}:{itemID}";

        #endregion
        
        #region Meta Upgrades
        public int LoadUpgradeLevel(UpgradeType type) =>
            PlayerPrefs.GetInt($"{KeyUpgrade}:{(int)type}", 0);   
        
        public void SaveUpgradeLevel(UpgradeType type, int level) =>
            PlayerPrefs.SetInt($"{KeyUpgrade}:{(int)type}", level);    
        #endregion

        #region Coins Resource
        public int LoadCoins() => PlayerPrefs.GetInt(KeyCoins, 0);

        public void AddCoins(int amount) => SaveCoins(LoadCoins() + amount);
        
        private void SaveCoins(int amount) => PlayerPrefs.SetInt(KeyCoins, amount);

        public bool TryToSpendCoins(int amount)
        {
            int current = LoadCoins();
            if (current < amount) return false;
            SaveCoins(current - amount);                                                                                                                                     
            return true;

        }
        
        #endregion

        #region Daily Rewards
        public bool TryClaimDailyReward(DateTime currentTime, out int rewardAmount)
        {
            var currentDay = currentTime.Date;
            var lastClaimDay = GetLastClaimDay();
            rewardAmount = GetRewardForDay(currentDay, lastClaimDay);

            if (rewardAmount <= 0)
                return false;

            AddCoins(rewardAmount);
            PlayerPrefs.SetString(KeyDailyRewardLastClaimDate, currentDay.ToBinary().ToString());
            PlayerPrefs.Save();
            return true;
        }

        private static int GetRewardForDay(DateTime currentDay, DateTime lastClaimDay)
        {
            if (lastClaimDay == DateTime.MinValue)
                return 0;

            var daysSinceLastClaim = (currentDay - lastClaimDay).Days;
            if (daysSinceLastClaim <= 0)
                return 0;

            if (daysSinceLastClaim > 1)
                return DailyRewardStepCoins;

            var streakIndex = Mathf.Clamp((currentDay - lastClaimDay).Days, 1, 4);
            return Mathf.Clamp(streakIndex * DailyRewardStepCoins, DailyRewardStepCoins, DailyRewardMaxCoins);
        }

        private DateTime GetLastClaimDay()
        {
            var storedValue = PlayerPrefs.GetString(KeyDailyRewardLastClaimDate, string.Empty);
            return string.IsNullOrEmpty(storedValue) ? DateTime.MinValue : DateTime.FromBinary(Convert.ToInt64(storedValue));
        }
        #endregion
    }
}
