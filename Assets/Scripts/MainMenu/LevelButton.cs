using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndexToLoad;
    public void LoadLevel()
    {
        SceneManager.LoadScene("Gameplay");
    }
}