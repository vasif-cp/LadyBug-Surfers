using System;
using LS.CharacterController.Physics.Core;
using UnityEngine;

namespace LS.CharacterController.Physics.Data
{
    [CreateAssetMenu(fileName = "TerrainSurfaceSettings", menuName = "Physics/Data/TerrainSurfaceSettings")]
    public class TerrainSurfaceSettings : ScriptableObject
    {
        [Serializable]
        public class Mapping                                                                                                                                                
        {                                   
            public TerrainLayer TerrainLayer;                                                                                                                               
            public SurfaceType SurfaceType;
        }      
        
        public Mapping[] Mappings;                                                                                                                                          
        public SurfaceType DefaultSurfaceType;
        
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
}
