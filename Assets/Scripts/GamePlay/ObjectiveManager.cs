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
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private Sprite blackStar;
    [SerializeField] private Sprite goldStar;
    [SerializeField] private CurrentLevelAsset currentLevel;
    private string _levelName;
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
        _levelName = level.name;
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
        star1.sprite = goldStar;
        star2.sprite = starsAwarded >= 2 ? goldStar : blackStar;
        star3.sprite = starsAwarded >= 3 ? goldStar : blackStar;
        var previousBest = PlayerPrefs.GetInt(_levelName);
        var best = Mathf.Max(starsAwarded, previousBest);
        PlayerPrefs.SetInt(_levelName, best);
    }

    public void HideWinScreen()
    {
        winScreen.SetActive(false);
    }
}
