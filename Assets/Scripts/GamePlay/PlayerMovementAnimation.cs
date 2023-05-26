using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectClasses;
using UnityEngine;

namespace GamePlay
{
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
                var command = movementCommands[0];
                _transition = StartCoroutine(WalkTransition(command));
            }
            else
            {
                // Play Idle Animation
            }
        }
        private IEnumerator WalkTransition(MoveCommand command)
        {
            var blob = transform;

            var time = 0f;
            var start = blob.position;
            var target = command.Position;
            var centerPivot = (start + target) * 0.5f;
            centerPivot.y -= 0.1f;
            start -= centerPivot;
            target -= centerPivot;

            var startRotation = blob.rotation;
            var targetRotation = Quaternion.LookRotation(target - start);

            while (!WithinRange)
            {
                time += Time.deltaTime * 3;
                transform.position = Vector3.Slerp(start, target, time) + centerPivot;
                blob.rotation = Quaternion.Slerp(startRotation, targetRotation, time);
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
}