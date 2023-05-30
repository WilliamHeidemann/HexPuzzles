using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LilyPad : MonoBehaviour
{
    private void OnEnable() => transform.Rotate(Vector3.up, Random.Range(0, 360));
}
