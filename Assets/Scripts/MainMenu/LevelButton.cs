using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [HideInInspector] public GridScriptableObject levelAssignedToButton;
    [HideInInspector] public World world;
    [SerializeField] private CurrentLevelAsset currentLevelAsset;

    public void LoadLevel()
    {
        currentLevelAsset.value = levelAssignedToButton;
        currentLevelAsset.world = world;
        ScreenFader.instance.FadeTo("Gameplay");
    }
}
