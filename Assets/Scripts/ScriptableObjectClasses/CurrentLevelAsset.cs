using UnityEngine;

namespace ScriptableObjectClasses
{
    [CreateAssetMenu(menuName = "Current Level Data")]
    public class CurrentLevelAsset : ScriptableObject
    {
        public GridScriptableObject value;
        public World world;
    }
}
