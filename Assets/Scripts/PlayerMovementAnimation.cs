using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
    public static PlayerMovementAnimation Instance;
    private readonly List<MoveCommand> movementCommands = new();
    private bool WithinRange => Vector3.Distance(transform.position, movementCommands[0].Position) < 0.05f;
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

    public void MoveTo(MoveCommand moveCommand)
    {
        movementCommands.Add(moveCommand);
    }
    
    private void Update()
    {
        if (movementCommands.Any())
        {
            var command = movementCommands[0];
            switch (command.MoveType)
            {
                case MoveType.Walk:
                    transform.position = Vector3.MoveTowards(transform.position, command.Position, 5 * Time.deltaTime);
                    break;
                case MoveType.Jump:
                    transform.position = Vector3.MoveTowards(transform.position, command.Position, 5 * Time.deltaTime);
                    break;
                case MoveType.Teleport:
                    transform.position = Vector3.MoveTowards(transform.position, command.Position, 5 * Time.deltaTime);
                    break;
            }
            if (WithinRange)
            {
                movementCommands.Remove(command);
                PlayerMovement.Instance.MoveRequestCompleted(command);
            }
        }
        else
        {
            // Play idle animation
        }
    }

    private void ClearMoveCommands(GridScriptableObject level)
    {
        movementCommands.Clear();
    }
}