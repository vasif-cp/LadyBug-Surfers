using LS.CharacterController.Core;
using LS.CharacterController.Physics.Data;
using LS.Meta;
using LS.Save;
using UnityEngine;

namespace LS.Core
{
    public interface IGameContext                                                                                                                                           
    {
        PhysicsSettings PhysicsSettings { get; }
        EconomySettings EconomySettings { get; }
        IUpgradeManager UpgradeManager { get; }       
        ISaveSystem SaveSystem { get; }
        
        IInputProvider InputProvider { get; }
        ICharacterMovementController CharacterMovementController { get; }
        
    }     
}
