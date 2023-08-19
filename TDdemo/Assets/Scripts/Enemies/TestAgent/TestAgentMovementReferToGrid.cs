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

    // Update is called once per frame
    // TODO: remove after movement is reimplemented through LoopTicker
    /*
    void Update()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(
            this.gameObject.transform.position, pathPoint, Time.deltaTime * 3f
        );

        if (this.gameObject.transform.position == pathPoint)
        {
            Vector3 oldPathPoint = pathPoint;
            pathPoint = gridHandler.nextPathPoint(gameObject.transform.position);
            if (oldPathPoint == pathPoint)
            {
                EntitySummoner.RemoveEnemy(gameObject.GetComponent<TestAgent>());
            }
        }
    }
    */
}
