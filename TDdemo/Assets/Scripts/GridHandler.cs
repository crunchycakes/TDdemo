using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHandler : MonoBehaviour
{

    public static Grid grid;
    public static Tilemap terrainTilemap;
    public static Tilemap backgroundTilemap;

    // key: current spot
    // value: optimal next spot
    // all uses cell pos, NOT WORLD POS
    private static Dictionary<(int, int), (int, int)> pathMap;

    // 2d tuples are ugly
    public class GridNode
    {
        public (int, int) pos;
        public GridNode parent;
        public int fvalue;
        public int gvalue;

        public GridNode((int, int) pos, GridNode last, int gvalue, int fvalue)
        {
            this.pos = pos;
            this.parent = last;
            this.fvalue = fvalue;
            this.gvalue = gvalue;
        }

        public GridNode((int, int) pos, int gvalue, int fvalue)
        {
            this.pos = pos;
            this.parent = this;
            this.fvalue = fvalue;
            this.gvalue = gvalue;
        }

        public GridNode((int, int) pos, int gvalue)
        {
            this.pos = pos;
            this.parent = this;
            this.gvalue = gvalue;
            this.fvalue = gvalue;
        }
    }

    // Start is called before the first frame update
    public static void Init()
    {
        // get grid and tilemaps; right now, this depends on names
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        Tilemap[] temptilemaps = grid.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in temptilemaps)
        {
            if (tilemap.gameObject.name == "Terrain") { terrainTilemap = tilemap; }
            else if (tilemap.gameObject.name == "Background") { backgroundTilemap = tilemap; }
        }

        RegeneratePathMap(GameObject.Find("Goal").transform.position);
    }

    // assigns each tile an "optimal next tile" so agents can pathfind
    // dijkstra but backwards, and dynamic programming on each reachable tile
    // Vector3 worldEnd: world location of point to pathfind to
    public static void RegeneratePathMap(Vector3 worldEnd)
    {
        pathMap = new Dictionary<(int, int), (int, int)>();

        Vector3Int cellPosEnd = grid.WorldToCell(worldEnd);
        (int, int) end = (cellPosEnd.x, cellPosEnd.y);
        pathMap[end] = end; // at end, path to itself

        // open is prio queue
        ArrayList open = new ArrayList();
        open.Add(new GridNode(end, 0));
        ArrayList closed = new ArrayList();
        GridNode current;

        int attempts = 0;
        int maxAttempts = 10000;

        (int, int)[] cardinalDirections = new (int, int)[4] { (-1, 0), (0, 1), (1, 0), (0, -1) };

        while (open.Count > 0 && attempts < maxAttempts)
        {
            attempts++;
            current = (GridHandler.GridNode)open[0];

            open.RemoveAt(0);

            foreach ((int, int) direction in cardinalDirections)
            {
                (int, int) newPos = addTuple(direction, current.pos);

                Vector3Int newPosVector = new Vector3Int(newPos.Item1, newPos.Item2, 0);
                if (!backgroundTilemap.HasTile(newPosVector))
                {
                    continue;
                }

                // we will accept terrain tiles; they'll just cost a lot so agents avoid if possible
                // this way, agents knocked back into terrain still pathfind
                int newGValue = current.gvalue + 1;
                if (terrainTilemap.HasTile(newPosVector))
                {
                    newGValue += 4999;
                }

                bool toContinue = false;
                foreach (GridHandler.GridNode node in open)
                {
                    if (node.pos == newPos && node.gvalue < newGValue)
                    {
                        toContinue = true;
                        break;
                    }
                }
                if (toContinue) { continue; }

                foreach (GridHandler.GridNode node in closed)
                {
                    if (node.pos == newPos && node.gvalue < newGValue)
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
                    if (newGValue <= ((GridHandler.GridNode)open[insertPos]).gvalue)
                    {
                        break;
                    }
                }
                
                GridNode neighbour = new GridNode(newPos, newGValue);
                open.Insert(insertPos, neighbour);
                pathMap[newPos] = current.pos;
            }

            closed.Add(current);

        }

        if (attempts >= maxAttempts)
        {
            Debug.Log($"GridHandler.regeneratePathMap() exceeded {maxAttempts} attempts; " +
                $"pathmap may not be complete");
        }

        return;
    }

    // code duplication
    private static (int, int) addTuple((int, int) t1, (int, int) t2)
    {
        return (t1.Item1 + t2.Item1, t1.Item2 + t2.Item2);
    }

    // gets worldpos and returns optimal next pos on the grid
    // Vector3 worldPos: current pos on world
    // returns Vector3 of next world pos to path to, which corresponds to cell on grid
    public static Vector3 NextPathPoint(Vector3 worldPos)
    {
        Vector3Int cellPos = grid.WorldToCell(worldPos);
        (int, int) nextIntSpot = pathMap[(cellPos.x, cellPos.y)];
        // gross, TODO: apply world offset programmatically
        return grid.CellToWorld(new Vector3Int(nextIntSpot.Item1, nextIntSpot.Item2)) + new Vector3(0.5f, 0.5f);
    }

}
