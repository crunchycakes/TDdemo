using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this should probably be static hey
public class AStarPathfinder : MonoBehaviour
{

    // Start is called before the first frame update
    public void Init()
    {
        // only implemented; does not do anything here
    }

    // returns ordered array of grid poses to go to
    // params use world pos, internally uses grid pos as tuples
    // 0,0,0 grid is 0.5,0.5,0.5 world   TODO: do this programmatically!
    // world pos is 1 to 1 size wise to grid
    // Vector3 worldStart: starting point of agent in world coords
    // Vector3 worldEnd: world position of point to pathfind towards
    // returns list of vector3s that form a path
    public Vector3[] Pathfind(Vector3 worldStart, Vector3 worldEnd)
    {
        Vector3Int vectorStart = GridHandler.grid.WorldToCell(worldStart);
        Vector3Int vectorEnd = GridHandler.grid.WorldToCell(worldEnd);
        (int, int) start = (vectorStart.x, vectorStart.y);
        (int, int) end = (vectorEnd.x, vectorEnd.y);

        // use background tilemap for accessible cells

        ArrayList open = new ArrayList() { new GridHandler.GridNode(start, 0, manhattan(start, end)) }; // just tuple
        ArrayList closed = new ArrayList(); // gridnode
        GridHandler.GridNode current;
        int attempts = 0;
        int attemptLimit = 10000;

        (int, int)[] cardinalDirections = new (int, int)[4] { (-1, 0), (0, 1), (1, 0), (0, -1) };

        while (open.Count > 0 && attempts < attemptLimit)
        {
            attempts++;
            current = (GridHandler.GridNode) open[0]; // prio queue

            if (current.pos == end) {
                return rebuildPath(current);
            }

            open.RemoveAt(0);

            // going to neighbours
            foreach ((int, int) direction in cardinalDirections)
            {
                // check if it exists, then if not in open/closed and superior
                (int, int) newPos = addTuple(direction, current.pos);

                Vector3Int newPosVector = new Vector3Int(newPos.Item1, newPos.Item2, 0);
                if (!GridHandler.backgroundTilemap.HasTile(newPosVector)
                    || GridHandler.terrainTilemap.HasTile(newPosVector)
                )
                {
                    continue;
                }

                int newGValue = current.gvalue + 1;
                int newFValue = newGValue + manhattan(newPos, end);

                bool toContinue = false;
                foreach (GridHandler.GridNode node in open)
                {
                    if (node.pos == newPos && node.fvalue < newFValue)
                    {
                        toContinue = true;
                        break;
                    }
                }
                if (toContinue) { continue; }

                foreach (GridHandler.GridNode node in closed)
                {
                    if (node.pos == newPos && node.fvalue < newFValue)
                    {
                        toContinue = true;
                        break;
                    }
                }
                if (toContinue) { continue; }

                // all checks passed
                int insertPos;
                for (insertPos = 0; insertPos < open.Count; insertPos++) // maintain ascending
                {
                    if (newFValue <= ((GridHandler.GridNode) open[insertPos]).fvalue)
                    {
                        break;
                    }
                }
                GridHandler.GridNode neighbour = new GridHandler.GridNode(newPos, current, newGValue, newFValue);
                open.Insert(insertPos, neighbour);
            }

            closed.Add(current);

        }

        // if failure, just path to start
        Debug.Log("Failed to astar pathfind; going to start");
        return new Vector3[] { worldStart };
    }

    // manhattan distance between two tuple coords
    private int manhattan((int, int) start, (int, int) end)
    {
        return Math.Abs(start.Item1 - end.Item1) + Math.Abs(start.Item2 - end.Item2);
    }

    // add two tuples
    private (int, int) addTuple((int, int) t1, (int, int) t2)
    {
        return (t1.Item1 + t2.Item1, t1.Item2 + t2.Item2);
    }

    // from goal node, get a path back
    // needs an offset by 0.5 each axis
    private Vector3[] rebuildPath(GridHandler.GridNode node)
    {
        Vector3 offset = new Vector3(0.5f, 0.5f, 0f);

        ArrayList tempArray = new ArrayList();
        while (node.parent != node)
        {
            Vector3Int cellPos = new Vector3Int(node.pos.Item1, node.pos.Item2, 0);
            tempArray.Add(GridHandler.backgroundTilemap.CellToWorld(cellPos) + offset);
            node = node.parent;
        }
        Vector3Int finalcellPos = new Vector3Int(node.pos.Item1, node.pos.Item2, 0);
        tempArray.Add(GridHandler.backgroundTilemap.CellToWorld(finalcellPos) + offset);

        Vector3[] finalArray = (Vector3[])tempArray.ToArray(typeof(Vector3));
        Array.Reverse(finalArray);

        return finalArray[1..];
    }
}
