using System;
using LS.CharacterController.Physics.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "SurfacePhysicsSettings", menuName = "Physics/Data/SurfacePhysicsSettings")]
public class SurfacePhysicsSettings : ScriptableObject
{
    [Serializable]
    public class Mapping                                                                                                                                                
    {                                   
        public TerrainLayer TerrainLayer;                                                                                                                               
        public SurfaceType SurfaceType;
    }      
        
    public Mapping[] Mappings;                                                                                                                                          
    public SurfaceType DefaultSurfaceType;
    
    public float SnowFrictionForce = 5f;                                                                                                                                        
    public float IceFrictionForce  = 2.5f; 
    public LayerMask GroundLayerMask;
    
    public SurfaceType Resolve(TerrainLayer layer)
    {
        for (int i = 0; i < Mappings.Length; i++)
        {
            if (Mappings[i].TerrainLayer == layer)
            {
                return Mappings[i].SurfaceType;                                                                                                                         
            }
        }                                                                                                                     
                                                                                                                                                                              
        return DefaultSurfaceType;  
    }
}
