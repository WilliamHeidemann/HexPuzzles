using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public GridScriptableObject levelAssignedToButton;
    [SerializeField] private CurrentLevelAsset _currentLevelAsset;

    public void LoadLevel()
    {
        _currentLevelAsset.value = levelAssignedToButton;
        ScreenFader.instance.FadeTo("Gameplay");
    }
}
