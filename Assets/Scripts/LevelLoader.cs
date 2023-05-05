using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelSpawner levelSpawner;
    [SerializeField] private List<GridScriptableObject> levels;
    [SerializeField] private int levelIndex;
    private GridScriptableObject CurrentLevel => levels[levelIndex];

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
        if (levelIndex >= levels.Count) return;
        EnterLevel(CurrentLevel);
    }
}