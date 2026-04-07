using UnityEngine;

namespace LS.Meta
{
    [CreateAssetMenu(fileName = "EconomySettings", menuName = "Meta/EconomySettings")]
    public class EconomySettings : ScriptableObject
    {
        public float BaseCoinsPerMeter = 0.5f;                                                                                                                              
        public int CollectibleBonusCoins = 50;
    }
}
