using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private LevelOrder levelOrder;
    [SerializeField] private Sprite goldStar;
    private void Start()
    {
        PopulateButtons();
    }

    private void PopulateButtons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var button = transform.GetChild(i);
            button.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            button.GetComponent<LevelButton>().levelAssignedToButton = levelOrder.orderedLevels[i];
            AddStars(button, i);
        }
    }

    private void AddStars(Transform button, int index)
    {
        var starsGainedInLevel = PlayerPrefs.GetInt(levelOrder.orderedLevels[index].name);
        for (int i = 0; i < starsGainedInLevel; i++)
        {
            button.GetChild(i + 1).GetComponent<Image>().sprite = goldStar;
        }
    }
}
