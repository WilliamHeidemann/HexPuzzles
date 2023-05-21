using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private GameObject nextWorldButton;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private CurrentLevelAsset currentLevel;
    public static ObjectiveManager instance;
    public bool LevelComplete => _objectivesRequiredForLevel == CompletedObjectivesForLevel;
    private int CompletedObjectivesForLevel => _objectivesRequiredForLevel - BlueTile.BlueTilesInLevel;
    private int _objectivesRequiredForLevel;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        LevelLoader.EnterLevelEvent += SetObjective;
        LevelLoader.EnterLevelEvent += HideMenu;
    }

    private void OnDestroy()
    {
        LevelLoader.EnterLevelEvent -= SetObjective;
        LevelLoader.EnterLevelEvent -= HideMenu;
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
        if (LevelComplete)
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        endGameScreen.SetActive(true);
        winScreen.SetActive(true);
        ShowStars();
        if (currentLevel.world.connectedLevels.Any(level => level.LevelIsComplete == false)) 
            nextLevelButton.SetActive(true);
        else nextWorldButton.SetActive(true);
    }

    private void HideMenu(GridScriptableObject level)
    {
        endGameScreen.SetActive(false);
        winScreen.SetActive(false);
        nextWorldButton.SetActive(false);
        nextLevelButton.SetActive(false);
    }
    
    private void ShowStars()
    {
        var starsAwarded = StepCounter.Instance.StarsToAward();
        UpdateScore(starsAwarded);
    }

    private void UpdateScore(int starsAwarded)
    {
        var previousBest = PlayerPrefs.GetInt(currentLevel.name);
        var best = Mathf.Max(starsAwarded, previousBest);
        PlayerPrefs.SetInt(currentLevel.name, best);
    }

    public void HideWinScreen()
    {
        winScreen.SetActive(false);
    }
}
