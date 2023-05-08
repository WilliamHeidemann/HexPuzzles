using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public enum MoveType
{
    Walk,
    Jump,
    Teleport
}

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}

internal record MoveCommand(Vector3 Position, MoveType MoveType);
public class PlayerMovementAnimation : MonoBehaviour
{
    public static PlayerMovementAnimation Instance;
    private readonly List<MoveCommand> _movementCommands = new();
    private bool WithinRange => Vector3.Distance(transform.position, _movementCommands[0].Position) < 0.05f;
    public void AddMovementCommand(Vector3 position, MoveType moveType) => _movementCommands.Add(new MoveCommand(position, moveType));

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
        var command = _movementCommands[0];
        // if Walk: (play walk animation)
        transform.position = Vector3.MoveTowards(transform.position, command.Position, 5 * Time.deltaTime);
        // if Jump: (play jump animation)
        // if Teleport: (play teleport animation)
        
        if (WithinRange)
        {
            _movementCommands.Remove(command);
            PlayerMovement.Instance.DisplayTilesInRange();
        }
    }

    private void ClearMoveCommands(GridScriptableObject level)
    {
        _movementCommands.Clear();
    }
}