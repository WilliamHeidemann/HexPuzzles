using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        ScreenFader.instance.FadeTo("Main Menu");
    }
    public void LoadNextWorld()
    {
        ScreenFader.instance.FadeTo("Main Menu");
        MenuScroller.shouldScroll = true;
    }
}
