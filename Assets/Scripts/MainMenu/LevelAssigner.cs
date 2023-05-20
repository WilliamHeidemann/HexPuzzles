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
    [SerializeField] private Sprite goldStar;
    [SerializeField] private Sprite silverStar;
    
    private void Start() => AssignLevels();
    private void AssignLevels()
    {
        if (world.centerLevel == null) centerButton.gameObject.SetActive(false);
        else
        {
            centerButton.levelAssignedToButton = world.centerLevel;
            centerButton.world = world;
            if (PlayerPrefs.GetInt(world.centerLevel.name) > 0) centerButton.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            else centerButton.transform.GetChild(0).GetComponent<Image>().color = Color.black;

            centerButton.transform.GetChild(0).GetComponent<Image>().sprite =
                PlayerPrefs.GetInt(world.centerLevel.name) == 3 ? goldStar : silverStar;
        }
        
        for (int i = 0; i < 6; i++)
        {
            if (world.connectedLevels[i] == null) outerButtons[i].gameObject.SetActive(false);
            else
            {
                outerButtons[i].levelAssignedToButton = world.connectedLevels[i];
                outerButtons[i].world = world;
                
                if (PlayerPrefs.GetInt(world.connectedLevels[i].name) > 0) outerButtons[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                else outerButtons[i].transform.GetChild(0).GetComponent<Image>().color = Color.black;

                outerButtons[i].transform.GetChild(0).GetComponent<Image>().sprite =
                    PlayerPrefs.GetInt(world.connectedLevels[i].name) == 3 ? goldStar : silverStar;
            }
        }
    }
}
