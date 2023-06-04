using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectClasses;
using UnityEngine;

namespace GamePlay
{
    public class PlayerMovementAnimation : MonoBehaviour
    {
        public static PlayerMovementAnimation instance;
        private readonly List<MoveCommand> _movementCommands = new();
        private bool WithinRange => Vector3.Distance(transform.position, _movementCommands[0].Position) < 0.05f;
        private Coroutine _transition;
        private void Awake()
        {
            if (instance != null) Destroy(this);
            instance = this;
            LevelLoader.EnterLevelEvent += ResetMovementAnimation;
        }

        private void OnDestroy()
        {
            LevelLoader.EnterLevelEvent -= ResetMovementAnimation;
        }

        public void MoveTo(MoveCommand moveCommand)
        {
            _movementCommands.Add(moveCommand);
        }
    
        private void Update()
        {
            if (_movementCommands.Any() && _transition == null)
            {
                var command = _movementCommands[0];
                _transition = command.AnimationType == AnimationType.Hop
                    ? StartCoroutine(HopTransition(command))
                    : StartCoroutine(DigTransition(command));
            }
            else
            {
                // Play Idle Animation
            }
        }

        private IEnumerator DigTransition(MoveCommand command)
        {
            var blob = transform;

            var mesh = GetComponentInChildren<SkinnedMeshRenderer>();
            mesh.enabled = false;
            var time = 0f;
            var start = blob.position;
            var target = command.Position;
            blob.rotation = Quaternion.LookRotation(target - start);
            while (!WithinRange)
            {
                time += Time.deltaTime * 3;
                blob.position = Vector3.Lerp(start, target, time);
                yield return null;
            }

            mesh.enabled = true;
            time = 0f;
            var interpolation = 0f;
            const float startSpeed = 0.07f;
            var airTarget = command.Position + Vector3.up * 2;
            while (interpolation < 1)
            {
                time += Time.deltaTime * 2;
                var speed = Mathf.Lerp(startSpeed, 0.01f, time);
                interpolation += speed;
                blob.position = Vector3.Lerp(target, airTarget, interpolation);
                yield return null;
            }
            
            time = 0f;
            interpolation = 0f;
            while (interpolation < 1)
            {
                time += Time.deltaTime * 2;
                var speed = Mathf.Lerp(0f, startSpeed, time);
                interpolation += speed;
                blob.position = Vector3.Lerp(airTarget, target, interpolation);
                yield return null;
            }
            
            _movementCommands.Remove(command);
            _transition = null;
            PlayerMovement.MoveRequestCompleted(command);
        }

        private IEnumerator HopTransition(MoveCommand command)
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
                blob.position = Vector3.Slerp(start, target, time) + centerPivot;
                blob.rotation = Quaternion.Slerp(startRotation, targetRotation, time);
                yield return null;
            }
            _movementCommands.Remove(command);
            _transition = null;
            PlayerMovement.MoveRequestCompleted(command);
        }

        private void ResetMovementAnimation(GridScriptableObject level)
        {
            _movementCommands.Clear();
            if (_transition != null)
            {
                StopCoroutine(_transition);
                _transition = null;
            }
            var mesh = GetComponentInChildren<SkinnedMeshRenderer>();
            mesh.enabled = true;
        }
    }
}