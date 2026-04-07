using LS.CharacterController.Physics.Data;
using UnityEngine;

namespace LS.CharacterController.Physics.Core
{
    public class TerrainSurfaceDetector
    {
        private readonly SurfacePhysicsSettings _settings;

        private Vector3 _lastSampledPosition;
        private SurfaceType _cachedSurfaceType;
        
        private const float ResampleThreshold = 0.05f; 
        
        public TerrainSurfaceDetector(SurfacePhysicsSettings settings)
        {                                                                                                                                                                   
            _settings = settings;                                                                                                                                               
            _cachedSurfaceType = _settings.DefaultSurfaceType;
        }  
        
        public SurfaceType GetSurfaceType(Vector3 worldPos, Terrain terrain)                                                                                                
        {
            if ((worldPos - _lastSampledPosition).sqrMagnitude < ResampleThreshold) return _cachedSurfaceType;
            
            _lastSampledPosition = worldPos;
            _cachedSurfaceType = Sample(worldPos, terrain);                                                                                                                 
            return _cachedSurfaceType;      
        }              
        
        private SurfaceType Sample(Vector3 worldPos, Terrain terrain)                                                                                                       
        {
            TerrainData data = terrain.terrainData;                                                                                                                         
            Vector3 origin   = terrain.transform.position;
                                          
            float relX = (worldPos.x - origin.x) / data.size.x;
            float relZ = (worldPos.z - origin.z) / data.size.z;                                                                                                             
            
            int mapX = Mathf.Clamp(Mathf.FloorToInt(relX * data.alphamapWidth),  0, data.alphamapWidth  - 1);                                                               
            int mapZ = Mathf.Clamp(Mathf.FloorToInt(relZ * data.alphamapHeight), 0, data.alphamapHeight - 1);
                                                                                                                                                                              
            float[,,] alphamap = data.GetAlphamaps(mapX, mapZ, 1, 1);                                                                                                       
                                                                                                                                                                              
            int dominantIndex  = 0;                                                                                                                                       
            float dominantWeight = 0f;  
                                                                                                                                                                              
            for (int i = 0; i < alphamap.GetLength(2); i++)                                                                                                                 
            {
                if (alphamap[0, 0, i] > dominantWeight)                                                                                                                     
                {
                    dominantWeight = alphamap[0, 0, i];
                    dominantIndex  = i; 
                }
            }                                                                                                                                                               
   
            
            TerrainLayer[] layers = data.terrainLayers;                                                                                                                     
            if (dominantIndex >= layers.Length) return _settings.DefaultSurfaceType;

            return _settings.Resolve(layers[dominantIndex]);                                                                                                                  
        }  

    }
}
