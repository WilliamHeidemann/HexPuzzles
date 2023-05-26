using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private GameObject nextWorldButton;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private CurrentLevelAsset currentLevel;
    [SerializeField] private GameObject starImage;
    public static ObjectiveManager instance;
    public bool LevelComplete => _objectivesRequiredForLevel == CompletedObjectivesForLevel;
    private int CompletedObjectivesForLevel => _objectivesRequiredForLevel - BlueTile.BlueTilesInLevel;
    private int _objectivesRequiredForLevel;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        LevelLoader.EnterLevelEvent += SetObjective;
    }

    private void OnDestroy()
    {
        LevelLoader.EnterLevelEvent -= SetObjective;
    }

    private void SetObjective(GridScriptableObject level)
    {
        _objectivesRequiredForLevel = level.tileData.Count(tile => tile.tileType == TileType.Blue);
        BlueTile.BlueTilesInLevel = _objectivesRequiredForLevel;
        objectiveText.text = $"{CompletedObjectivesForLevel}/{_objectivesRequiredForLevel}";
    }
    
    public void ProgressionCheck()
    {
        objectiveText.text = $"{CompletedObjectivesForLevel}/{_objectivesRequiredForLevel}";
        if (LevelComplete) ShowWinScreen();
    }

    private void ShowWinScreen()
    {
        endGameScreen.SetActive(true);
        starImage.SetActive(true);
        nextLevelButton.SetActive(true);
        if (currentLevel.world.connectedLevels.Where(level => level != null).All(level => level.LevelIsComplete)) 
            nextWorldButton.SetActive(true);
    }
}