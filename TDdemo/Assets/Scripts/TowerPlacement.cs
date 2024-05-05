using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

    private GameObject currentTower;
    [HideInInspector]
    public List<Vector3Int> TowerPosList;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        TowerPosList = new List<Vector3Int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTower != null)
        {
            Vector3Int cell = GridHandler.grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            currentTower.transform.position = GridHandler.grid.GetCellCenterWorld(cell);

            if (Input.GetMouseButtonDown(0) && !TowerPosList.Contains(cell))
            {
                currentTower.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                Debug.Log(cell);
                TowerPosList.Add(cell);
                currentTower = null;
            }
        }
        
    }

    public void SetTowerToPlace(GameObject tower)
    {
        currentTower = Instantiate(tower, Vector2.zero, Quaternion.identity);
        currentTower.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

}
