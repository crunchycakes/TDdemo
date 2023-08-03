using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this should probably be static hey
public class AStarPathfinder : MonoBehaviour
{

    private GridHandler gridHandler;

    // 2d tuples were ugly
    private class GridNode
    {
        public (int, int) pos;
        public GridNode parent;
        public int fvalue;
        public int gvalue;

        public GridNode ((int, int) pos, GridNode last, int gvalue, int fvalue)
        {
            this.pos = pos;
            this.parent = last;
            this.fvalue = fvalue;
            this.gvalue = gvalue;
        }

        public GridNode ((int, int) pos, int gvalue, int fvalue)
        {
            this.pos = pos;
            this.parent = this;
            this.fvalue = fvalue;
            this.gvalue = gvalue;
        }
    }


    // Start is called before the first frame update
    private void Start()
    {
        gridHandler = GameObject.Find("Admin").GetComponent<GridHandler>();
    }

    // returns ordered array of grid poses to go to
    // params use world pos, internally uses grid pos as tuples
    // 0,0,0 grid is 0.5,0.5,0.5 world
    // world pos is 1 to 1 size wise to grid
    public Vector3[] pathfind(Vector3 worldStart, Vector3 worldEnd)
    {
        Vector3Int vectorStart = gridHandler.grid.WorldToCell(worldStart);
        Vector3Int vectorEnd = gridHandler.grid.WorldToCell(worldEnd);
        (int, int) start = (vectorStart.x, vectorStart.y);
        (int, int) end = (vectorEnd.x, vectorEnd.y);

        // use background tilemap for accessible cells

        ArrayList open = new ArrayList() { new GridNode(start, 0, manhattan(start, end)) }; // just tuple
        ArrayList closed = new ArrayList(); // gridnode
        GridNode current;
        int attempts = 0;
        int attemptLimit = 10000;

        while (open.Count > 0 && attempts < attemptLimit)
        {
            Debug.Log(attempts);
            attempts++;
            current = (GridNode) open[0]; // prio queue

            if (current.pos == end) {
                return rebuildPath(current);
            }

            open.RemoveAt(0);

            // going to neighbours
            (int, int)[] cardinalDirections = new (int, int)[4] { (-1, 0), (0, 1), (1, 0), (0, -1)};
            foreach ((int, int) direction in cardinalDirections)
            {
                // check if it exists, then if not in open/closed and superior
                (int, int) newPos = addTuple(direction, current.pos);

                Vector3Int newPosVector = new Vector3Int(newPos.Item1, newPos.Item2, 0);
                if (!gridHandler.backgroundTilemap.HasTile(newPosVector)
                    || gridHandler.terrainTilemap.HasTile(newPosVector)
                )
                {
                    Debug.Log("checked tile does not exist or is blocked");
                    continue;
                }

                int newGValue = current.gvalue + 1;
                int newFValue = newGValue + manhattan(newPos, end);

                bool toContinue = false;
                foreach (GridNode node in open)
                {
                    if (node.pos == newPos && node.fvalue < newFValue)
                    {
                        Debug.Log("checked tile has superior alternative in open");
                        toContinue = true;
                        break;
                    }
                }
                if (toContinue) { continue; }

                foreach (GridNode node in closed)
                {
                    if (node.pos == newPos && node.fvalue < newFValue)
                    {
                        Debug.Log("checked tile has superior alternative in closed");
                        toContinue = true;
                        break;
                    }
                }
                if (toContinue) { continue; }

                // all checks passed
                Debug.Log("checked tile passes");
                int insertPos;
                for (insertPos = 0; insertPos < open.Count; insertPos++) // maintain ascending
                {
                    if (newFValue <= ((GridNode) open[insertPos]).fvalue)
                    {
                        break;
                    }
                }
                GridNode neighbour = new GridNode(newPos, current, newGValue, newFValue);
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
    private Vector3[] rebuildPath(GridNode node)
    {
        ArrayList tempArray = new ArrayList();
        while (node.parent != node)
        {
            Vector3Int cellPos = new Vector3Int(node.pos.Item1, node.pos.Item2, 0);
            tempArray.Add(gridHandler.backgroundTilemap.CellToWorld(cellPos));
            node = node.parent;
        }
        Vector3Int finalcellPos = new Vector3Int(node.pos.Item1, node.pos.Item2, 0);
        tempArray.Add(gridHandler.backgroundTilemap.CellToWorld(finalcellPos));

        Vector3[] finalArray = (Vector3[])tempArray.ToArray(typeof(Vector3));
        Array.Reverse(finalArray);

        return finalArray;
    }
}
