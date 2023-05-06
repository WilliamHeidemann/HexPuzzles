using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelButton : MonoBehaviour
{
    public GridScriptableObject levelAssignedToButton;
    [SerializeField] private CurrentLevelAsset _currentLevelAsset;

    public void LoadLevel()
    {
        _currentLevelAsset.value = levelAssignedToButton;
        SceneManager.LoadScene("Gameplay");
    }
}
