using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementReferToGrid : AgentMovement
{

    public override void Init()
    {
        if (target == null)
        {
            target = GameObject.Find("Goal");
        }

        pathPoint = GridHandler.NextPathPoint(gameObject.transform.position);
    }

    public override void UpdatePathPoint()
    {
        pathPoint = GridHandler.NextPathPoint(gameObject.transform.position);
    }

    public override void ToNextPathPoint()
    {
        UpdatePathPoint();
    }

}
