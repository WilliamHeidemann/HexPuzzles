using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelSpawner levelSpawner;
    [SerializeField] private LevelOrder levels;
    [SerializeField] private int levelIndex;
    private GridScriptableObject CurrentLevel => levels.orderedLevels[levelIndex];

    public delegate void EnterLevelDelegate(GridScriptableObject level);
    public static event EnterLevelDelegate EnterLevelEvent;

    private void Start()
    {
        EnterLevel(CurrentLevel);
    }

    private void EnterLevel(GridScriptableObject level)
    {
        levelSpawner.assetToLoad = level;
        levelSpawner.LoadAsset();
        EnterLevelEvent?.Invoke(level);
    }

    public void RetryLevel()
    {
        EnterLevel(CurrentLevel);
    }
    
    public void NextLevel()
    {
        levelIndex++;
        if (levelIndex >= levels.orderedLevels.Count) return;
        EnterLevel(CurrentLevel);
    }
}