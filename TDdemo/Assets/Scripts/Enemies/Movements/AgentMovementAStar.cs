using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementAStar : AgentMovement
{

    private Vector3[] path;
    private int pathIndex;

    public override void Init()
    {
        pathfinder = gameObject.AddComponent<PathfinderAStar>();
        pathfinder.Init();

        pathIndex = 0;

        if (target == null)
        {
            target = GameObject.Find("Goal");
        }

        path = pathfinder.Pathfind(this.transform.position, target.transform.position);

        pathPoint = path[pathIndex];
    }

    public override void UpdatePathPoint()
    {
        path = pathfinder.Pathfind(this.transform.position, target.transform.position);
        pathIndex = 0;
        pathPoint = path[pathIndex];
    }

    public override void ToNextPathPoint()
    {
        pathIndex++; // TODO: CHECK OUT OF BOUNDS!!!!
        pathPoint = path[pathIndex];
    }
}
