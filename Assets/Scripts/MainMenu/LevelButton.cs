using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [HideInInspector] public GridScriptableObject levelAssignedToButton;
    [HideInInspector] public World world;
    [SerializeField] private CurrentLevelAsset _currentLevelAsset;

    public void LoadLevel()
    {
        _currentLevelAsset.value = levelAssignedToButton;
        _currentLevelAsset.world = world;
        ScreenFader.instance.FadeTo("Gameplay");
    }
}
