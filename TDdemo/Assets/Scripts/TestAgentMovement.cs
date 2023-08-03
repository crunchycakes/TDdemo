using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestAgentMovement : MonoBehaviour
{

    // TODO: make AStarPathfinder inherit from abstract Pathfinder class
    [SerializeField] private AStarPathfinder pathfinder;
    [SerializeField] private GameObject end;

    private Vector3[] path;
    private int pathIndex;

    // Start is called before the first frame update
    void Start()
    {
        pathIndex = 0;

        path = pathfinder.pathfind(this.transform.position, end.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (pathIndex < path.Length)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(
                this.gameObject.transform.position, path[pathIndex], Time.deltaTime * 3f
            );

            if (this.gameObject.transform.position == path[pathIndex])
            {
                pathIndex++;
            }
        }

        

    }
}
