using UnityEngine;

namespace MainMenu
{
    public class MainMenuLoader : MonoBehaviour
    {
        public void LoadMainMenu()
        {
            ScreenFader.instance.FadeTo("Main Menu");
        }
    }
}
