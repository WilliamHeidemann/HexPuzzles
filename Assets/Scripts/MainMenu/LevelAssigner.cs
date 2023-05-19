using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelAssigner : MonoBehaviour
{
    [SerializeField] private World world;
    [SerializeField] private LevelButton centerButton;
    [SerializeField] private LevelButton[] outerButtons;
    private void Start() => AssignLevels();
    private void AssignLevels()
    {
        centerButton.levelAssignedToButton = world.centerLevel;
        for (int i = 0; i < 6; i++) 
            outerButtons[i].levelAssignedToButton = world.connectedLevels[i];
    }
}
