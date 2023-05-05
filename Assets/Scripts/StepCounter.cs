using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StepCounter : MonoBehaviour
{
    public static StepCounter Instance;
    [SerializeField] private GameObject EndGameScreen;
    [SerializeField] private TextMeshProUGUI stepCountText;
    private int _stepCount;
    private int _availableSteps;
    private int RemainingSteps => _availableSteps - _stepCount;
    private int _optimalSteps;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
        LevelLoader.EnterLevelEvent += RefreshDisplaySteps;
    }

    private void RefreshDisplaySteps(GridScriptableObject level)
    {
        _stepCount = 0;
        _optimalSteps = level.optimalSteps;
        _availableSteps = Mathf.RoundToInt(_optimalSteps * 1.5f);
        stepCountText.text = RemainingSteps.ToString();
    }

    public void IncrementStepCount()
    {
        _stepCount++;
        stepCountText.text = RemainingSteps.ToString();
        if (RemainingSteps == 0 && !ObjectiveManager.Instance.LevelComplete) EndGame();
    }

    public void GainBonusSteps(int amount)
    {
        _stepCount -= amount;
        stepCountText.text = RemainingSteps.ToString();
    }

    private void EndGame()
    {
        EndGameScreen.SetActive(true);
        var button = EndGameScreen.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => EndGameScreen.SetActive(false));
    }

    public int StarsToAward()
    {
        _stepCount++; // Game ended before last step was accounted for
        if (_stepCount <= _optimalSteps) return 3; // Optimal or better path found
        if (_stepCount > _optimalSteps && _stepCount < _optimalSteps * 1.2f) return 2; // Within 20% best solution found
        return 1; // Level completed
    }
}

public static class MinimumStepSolver
{
    public static int Solve(GridScriptableObject level)
    {
        return 0;
    }
}