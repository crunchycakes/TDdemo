using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AgentMovement : MonoBehaviour
{

    // TODO: make AStarPathfinder inherit from abstract Pathfinder class
    [SerializeField] protected Pathfinder pathfinder;

    protected GameObject target;
    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    protected Vector3 pathPoint;
    public Vector3 PathPoint
    {
        get { return pathPoint; }
        set { pathPoint = value; }
    }

    public abstract void Init();

    // big update, expensive
    public abstract void UpdatePathPoint();
    // small update, go to next calced pathpoint; in some cases may be same as update
    public abstract void ToNextPathPoint();

}
