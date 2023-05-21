using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private CurrentLevelAsset currentLevel;
    public delegate void EnterLevelDelegate(GridScriptableObject level);
    public static event EnterLevelDelegate EnterLevelEvent;
    private void Start()
    {
        EnterLevel(currentLevel.value);
    }

    private void EnterLevel(GridScriptableObject level)
    {
        currentLevel.value = level;
        EnterLevelEvent?.Invoke(level);
    }

    public void RetryLevel()
    {
        EnterLevel(currentLevel.value);
    }
    
    public void NextLevel()
    {
        var nextLevel = currentLevel.world.connectedLevels.First(level => level.LevelIsComplete == false);
        EnterLevel(nextLevel);
    }
}