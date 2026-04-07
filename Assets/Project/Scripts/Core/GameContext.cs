using System;
using System.Linq;
using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Meta;
using LS.Save;
using UnityEngine;

namespace LS.Core
{
    [DefaultExecutionOrder(-100)]
    public class GameContext : MonoBehaviour, IGameContext
    {
        [Header("Data References")]
        [SerializeField] private UpgradeSettings _upgradeSettings;
        [SerializeField] private PhysicsSettings _physicsSettings;
        [SerializeField] private EconomySettings _economySettings;
        
        private ServiceRegistry _serviceRegistry;

        
        public IUpgradeManager UpgradeManager { get; private set; }
        public ISaveSystem SaveSystem { get; private set; }
        public PhysicsSettings PhysicsSettings { get; private set; }
        public EconomySettings EconomySettings { get; private set; }
        
        public IInputProvider InputProvider { get; private set; }
        public ICharacterMovementController CharacterMovementController { get; private set;}

        private void Awake()
        {
            SaveSystem = new PlayerPrefsSaveSystem();
            UpgradeManager = new UpgradeManager(_upgradeSettings, SaveSystem);
            PhysicsSettings = _physicsSettings;
            EconomySettings = _economySettings;
            
            _serviceRegistry = new ServiceRegistry();
            
            var monoBehavioursList = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);                                                                                   
                                                                                                                                                    
            foreach (var monoBehaviour in monoBehavioursList)
            {
                if (monoBehaviour is IContextService service)
                {
                    service.Register(_serviceRegistry);
                }
            }

            InputProvider = _serviceRegistry.Resolve<IInputProvider>();
            CharacterMovementController = _serviceRegistry.Resolve<ICharacterMovementController>();                                                                                 
                                                                                                       
            foreach (var monoBehaviour in monoBehavioursList)
            {
                if (monoBehaviour is IInjectable injectable)
                {
                    injectable.Inject(this);  
                }                                                                                                                                      
            }  
        }
    }
}
