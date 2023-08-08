using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHandler : MonoBehaviour
{

    public Grid grid;
    public Tilemap terrainTilemap;
    public Tilemap backgroundTilemap;

    // this won't be very robust if agent gets pushed into terrain
    private Dictionary<(int, int), (int, int)> pathMap;

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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // assigns each tile an "optimal next tile" so agents can pathfind
    // Vector3 end: world location of point to pathfind to
    public void regeneratePathMap(Vector3 end)
    {
        pathMap = new Dictionary<(int, int), (int, int)>();

        ArrayList open = new ArrayList();
        open.Add(end);
        ArrayList close = new ArrayList();
        (int, int) current;

        int attempts = 0;
        int maxAttempts = 10000;

        while (open.Count > 0 && attempts < maxAttempts)
        {

        }

        Debug.Log($"GridHandler.regeneratePathMap() exceeded {maxAttempts} attempts or otherwise broke out of loop: " +
            $"pathmap may not be complete");
        return;
    }

}
