using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Transform _cachedTransform;
    private Vector3 _offset;

    private void Start()
    {
        _cachedTransform = transform;
        _offset = _cachedTransform.position - player.position;
    }

    void Update()
    {
        var playerPosition = player.position;
        _cachedTransform.position = _offset + new Vector3(playerPosition.x, 0f, playerPosition.z);
    }
}
