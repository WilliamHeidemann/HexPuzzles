using UnityEngine;

namespace ScriptableObjectClasses
{
    [CreateAssetMenu(menuName = "World", fileName = "New World")]
    public class World : ScriptableObject
    {
        public GridScriptableObject centerLevel;
        public GridScriptableObject[] connectedLevels;
        public int index;
    }
}