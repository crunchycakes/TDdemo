using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAgentMovement : MonoBehaviour
{

    // TODO: make AStarPathfinder inherit from abstract Pathfinder class
    [SerializeField] private AStarPathfinder pathfinder;
    [SerializeField] private GameObject end;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] finalPath = pathfinder.pathfind(this.transform.position, end.transform.position);
        foreach (var path in finalPath )
        {
            Debug.Log((path.x, path.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
