using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{

    private GridHandler gridHandler;

    // Start is called before the first frame update
    private void Start()
    {
        gridHandler = GameObject.Find("Admin").GetComponent<GridHandler>();
        aStar(new Vector3(-8.5f, -4.5f, 0f), Vector3.zero);
        aStar(new Vector3(0.5f, 0.5f, 0f), Vector3.zero);
    }

    // params use world pos, internally uses grid pos
    // 0,0,0 grid is 0.5 world
    private Vector3[] aStar(Vector3 worldStart, Vector3 worldEnd)
    {
        Vector3Int start = gridHandler.grid.WorldToCell(worldStart);
        Vector3Int end = gridHandler.grid.WorldToCell(worldEnd);

        // use background tilemap for accessible cells

        Debug.Log(start);

        // appease compilation errors
        return new Vector3[] { start, end };
    }
}
