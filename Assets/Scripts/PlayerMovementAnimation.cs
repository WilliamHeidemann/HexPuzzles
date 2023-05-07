using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
    public static PlayerMovementAnimation Instance;
    private readonly List<Vector3> _movementCommands = new();
    private bool WithinRange => Vector3.Distance(transform.position, _movementCommands[0]) < 0.05f;
    public void AddMovementCommand(Vector3 position) => _movementCommands.Add(position);

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
        LevelLoader.EnterLevelEvent += ClearMoveCommands;
    }

    private void OnDestroy()
    {
        LevelLoader.EnterLevelEvent -= ClearMoveCommands;
    }

    private void Update()
    {
        if (!_movementCommands.Any()) return;
        var target = _movementCommands[0];
        transform.position = Vector3.MoveTowards(transform.position, target, 5 * Time.deltaTime);
        if (WithinRange)
        {
            _movementCommands.Remove(target);
            PlayerMovement.Instance.DisplayTilesInRange();
        }
    }

    private void ClearMoveCommands(GridScriptableObject level)
    {
        _movementCommands.Clear();
    }
}