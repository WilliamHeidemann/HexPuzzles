using System.Linq;
using ScriptableObjectClasses;
using UnityEngine;

namespace GamePlay
{
    public class LevelSpawner : MonoBehaviour
    {
        public LevelTile tilePrefab;
        [HideInInspector] public GridScriptableObject assetToUpdate;
        [HideInInspector] public GridScriptableObject assetToLoad;
        public Transform container;

        private void Awake()
        {
            LevelLoader.EnterLevelEvent += SetAssetToLoad;
            LevelLoader.EnterLevelEvent += LoadAsset;
        }

        private void OnDestroy()
        {
            LevelLoader.EnterLevelEvent -= SetAssetToLoad;
            LevelLoader.EnterLevelEvent -= LoadAsset;
        }

        private void SetAssetToLoad(GridScriptableObject level)
        {
            assetToLoad = level;
        }

        public void LoadAsset(GridScriptableObject level)
        {
            if (level == null) return;
            var hexTiles = container.GetComponentsInChildren<LevelTile>();
            // Clear Old Content
            foreach (var levelTile in hexTiles)
            {
                levelTile.ApplyTileStruct(new AxialHex
                {
                    tileType = TileType.Empty,
                    Q = levelTile.q,
                    R = levelTile.r,
                });
            }
        
            // Add New Content
            foreach (var tileStruct in level.tileData)
            {
                var hexTile = hexTiles.FirstOrDefault(hexTile => hexTile.q == tileStruct.Q && hexTile.r == tileStruct.R);
                if (hexTile != null) hexTile.ApplyTileStruct(tileStruct);
            }

            foreach (var levelTile in hexTiles)
            {
                levelTile.ShowTile();
            }
        }
    }
}