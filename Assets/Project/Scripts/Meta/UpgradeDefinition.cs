using System;
using UnityEngine;

namespace LS.Meta
{
    [Serializable]
    public class UpgradeDefinition
    {
        public UpgradeType Type;
        public string DisplayName;
        public Sprite Icon;
        public int MaxLevel;
        public int BasePrice;
        public int PriceIncreasePerLevel;
        public float ValuePerLevel;
    }
}
