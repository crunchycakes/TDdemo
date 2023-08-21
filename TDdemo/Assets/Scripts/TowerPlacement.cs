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
        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        currentTower = Instantiate(tower, Vector2.zero, Quaternion.identity);
    }

}
