using UnityEngine;

namespace LS.Meta
{
    [CreateAssetMenu(fileName = "UpgradeSettings", menuName = "Meta/UpgradeSettings")]
    public class UpgradeSettings : ScriptableObject
    {
        [SerializeField] private UpgradeDefinition[] Upgrades = new UpgradeDefinition[4];
        public UpgradeDefinition Get(UpgradeType type) => Upgrades[(int)type];
    }
}
