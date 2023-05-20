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
        if (world.centerLevel == null) centerButton.gameObject.SetActive(false);
        else
        {
            centerButton.levelAssignedToButton = world.centerLevel;
            centerButton.world = world;
        }
        
        for (int i = 0; i < 6; i++)
        {
            if (world.connectedLevels[i] == null) outerButtons[i].gameObject.SetActive(false);
            else
            {
                outerButtons[i].levelAssignedToButton = world.connectedLevels[i];
                outerButtons[i].world = world;
            }
        }
    }
}
