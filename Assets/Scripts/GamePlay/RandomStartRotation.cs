using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class RandomStartRotation : MonoBehaviour
    {
        private void OnEnable() => transform.Rotate(Vector3.up, Random.Range(0, 360));
    }
}
