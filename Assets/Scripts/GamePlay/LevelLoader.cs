using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private CurrentLevelAsset currentLevel;
    public delegate void EnterLevelDelegate(GridScriptableObject level);
    public static event EnterLevelDelegate EnterLevelEvent;
    private void Start()
    {
        EnterLevel(currentLevel.value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) NextLevel();
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
        var nextLevel = currentLevel.world.connectedLevels.FirstOrDefault(level => level.LevelIsComplete == false);
        if (nextLevel == null) nextLevel = currentLevel.world.connectedLevels.Where(level => level != currentLevel.value).ToArray()[Random.Range(0, 5)];
        EnterLevel(nextLevel);
    }
}