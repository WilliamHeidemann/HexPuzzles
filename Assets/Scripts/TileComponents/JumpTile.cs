using System.Linq;
using UnityEngine;

public class JumpTile : TileComponentBase, IActivatedTile
{
    private const int JumpLength = 2;
    
    public void Activate()
    {
        var movement = PlayerMovement.Instance;
        var previousQ = movement.previous.q;
        var currentQ = movement.current.q;
        var directionQ = Bounded(currentQ - previousQ);
        var targetQ = currentQ + (directionQ * JumpLength);
        
        var previousR = movement.previous.r;
        var currentR = movement.current.r;
        var directionR = Bounded(currentR - previousR);
        var targetR = currentR + (directionR * JumpLength);

        var targetTile = FindObjectsOfType<LevelTile>().First(tile => tile.q == targetQ && tile.r == targetR);
        movement.MoveRequest(new MoveCommand(targetTile, MoveType.Jump));
    }

    private static int Bounded(int num) => num < 0 ? -1 : num > 0 ? 1 : 0;
}