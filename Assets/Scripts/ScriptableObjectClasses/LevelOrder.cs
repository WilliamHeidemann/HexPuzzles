using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectClasses
{
    [CreateAssetMenu(menuName = "Level Order")]
    public class LevelOrder : ScriptableObject
    {
        public List<GridScriptableObject> orderedLevels;
    }
}