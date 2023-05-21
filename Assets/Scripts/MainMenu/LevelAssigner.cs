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
            centerButton.world = world;
            centerButton.levelAssignedToButton = world.centerLevel;
            var image = GetImage(centerButton.transform);
            SetStar(image, world.centerLevel.name);
        }
        
        for (int i = 0; i < 6; i++)
        {
            if (world.connectedLevels[i] == null) outerButtons[i].gameObject.SetActive(false);
            else
            {
                outerButtons[i].world = world;
                outerButtons[i].levelAssignedToButton = world.connectedLevels[i];
                var image = GetImage(outerButtons[i].transform);
                SetStar(image, world.connectedLevels[i].name);
            }
        }
    }
    private static Image GetImage(Transform button) => button.GetChild(0).GetComponent<Image>();
    private void SetStar(Image image, string levelName)
    {
        var award = PlayerPrefs.GetInt(levelName);
        image.color = award > 0 ? Color.white : Color.black;
        image.sprite = award == 3 ? goldStar : silverStar;
    }
}
