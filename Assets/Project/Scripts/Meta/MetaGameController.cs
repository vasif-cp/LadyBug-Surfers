using System;
using UnityEngine;

namespace LS.Meta
{
    public class MetaGameController : MonoBehaviour
    {
        [SerializeField] private UpgradeSettings _upgradeSettings;
        
        private UpgradeManager _upgradeManager;
        
        public UpgradeManager UpgradeManager => _upgradeManager;

        private void Awake()
        {
            _upgradeManager = new UpgradeManager(_upgradeSettings);
        }
    }
}
