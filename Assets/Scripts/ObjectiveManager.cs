using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private Sprite blackStar;
    [SerializeField] private Sprite goldStar;
    public static ObjectiveManager Instance;
    public bool LevelComplete => _objectivesRequiredForLevel == _completedObjectivesForLevel;
    private int _objectivesRequiredForLevel;
    private int _completedObjectivesForLevel;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
        LevelLoader.EnterLevelEvent += SetObjective;
        LevelLoader.EnterLevelEvent += HideMenu;
    }

    private void SetObjective(GridScriptableObject level)
    {
        _completedObjectivesForLevel = 0;
        _objectivesRequiredForLevel = level.tileData.Count(tile => tile.tileType == TileType.Blue);
        objectiveText.text = $"{_completedObjectivesForLevel}/{_objectivesRequiredForLevel}";
    }
    
    public void ProgressObjective()
    {
        _completedObjectivesForLevel += 1;
        objectiveText.text = $"{_completedObjectivesForLevel}/{_objectivesRequiredForLevel}";
        if (LevelComplete)
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        // Something that stops the player from moving further
        endGameScreen.SetActive(true);
        winScreen.SetActive(true);
        ShowStars();
    }

    private void HideMenu(GridScriptableObject level)
    {
        endGameScreen.SetActive(false);
        winScreen.SetActive(false);
    }
    
    private void ShowStars()
    {
        var starsAwarded = StepCounter.Instance.StarsToAward();
        star1.sprite = goldStar;
        star2.sprite = starsAwarded >= 2 ? goldStar : blackStar;
        star3.sprite = starsAwarded >= 3 ? goldStar : blackStar;
    }

    public void HideWinScreen()
    {
        winScreen.SetActive(false);
    }
}
