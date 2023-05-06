using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelOrder levelOrder;
    [SerializeField] private CurrentLevelAsset currentLevel;
    public delegate void EnterLevelDelegate(GridScriptableObject level);
    public static event EnterLevelDelegate EnterLevelEvent;
    private void Start()
    {
        EnterLevel(currentLevel.value);
    }

    private void EnterLevel(GridScriptableObject level)
    {
        EnterLevelEvent?.Invoke(level);
    }

    public void RetryLevel()
    {
        EnterLevel(currentLevel.value);
    }
    
    public void NextLevel()
    {
        if (currentLevel.value == levelOrder.orderedLevels[^1]) return;
        var currentIndex = levelOrder.orderedLevels.IndexOf(currentLevel.value);
        currentLevel.value = levelOrder.orderedLevels[currentIndex + 1];
        EnterLevel(currentLevel.value);
    }
}