using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAgentMovementReferToGrid : MonoBehaviour
{
    [SerializeField] private GameObject end;
    private GridHandler gridHandler;

    // this being public is kinda gross too
    [HideInInspector]
    public Vector3 pathPoint;

    // Start is called before the first frame update
    public void Init()
    {
        gridHandler = GameObject.Find("Admin").GetComponent<GridHandler>();

        if (end == null)
        {
            end = GameObject.Find("Goal");
        }

        pathPoint = gridHandler.nextPathPoint(gameObject.transform.position);
    }
}
