using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

    private GameObject currentTower;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTower != null)
        {
            Vector3Int cell = GridHandler.grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            currentTower.transform.position = GridHandler.grid.GetCellCenterWorld(cell);

            if (Input.GetMouseButtonDown(0))
            {
                currentTower.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
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
