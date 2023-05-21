using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
    public static PlayerMovementAnimation Instance;
    private readonly List<MoveCommand> movementCommands = new();
    private bool WithinRange => Vector3.Distance(transform.position, movementCommands[0].Position) < 0.05f;
    private Coroutine _transition;
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
        if (movementCommands.Any() && _transition == null)
        {
            _transition = StartCoroutine(WalkTransition(movementCommands[0]));
        }
        else
        {
            // Play Idle Animation
        }
    }
    private IEnumerator WalkTransition(MoveCommand command)
    {
        var time = 0f;
        var start = transform.position;
        var target = command.Position;
        var centerPivot = (start + target) * 0.5f;
        centerPivot.y -= 0.1f;
        start -= centerPivot;
        target -= centerPivot;
        while (!WithinRange)
        {
            time += Time.deltaTime * 3;
            transform.position = Vector3.Slerp(start, target, time) + centerPivot;
            yield return null;
        }
        movementCommands.Remove(command);
        _transition = null;
        PlayerMovement.MoveRequestCompleted(command);
    }
    
    private IEnumerator JumpTransition(MoveCommand command)
    {
        while (!WithinRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, command.Position, 5 * Time.deltaTime);
            yield return null;
        }
        movementCommands.Remove(command);
        _transition = null;
        PlayerMovement.MoveRequestCompleted(command);
    }

    private void ClearMoveCommands(GridScriptableObject level)
    {
        movementCommands.Clear();
    }
}