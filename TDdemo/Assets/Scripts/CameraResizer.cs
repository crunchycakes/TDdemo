using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraResizer : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private Tilemap background;

    // Start is called before the first frame update
    void Start()
    {
        background.CompressBounds();
        resizeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // sizes and centers camera to guarantee a border of at least 1/6 of screen
    // TODO: instead, base resize on UI components
    private void resizeCamera()
    {
        // first, determine if width or height is limiting; or, proportionally larger when aspect normalised
        float BGSize;
        if ((float)background.cellBounds.xMax / cam.aspect > (float)background.cellBounds.yMax) // wider than tall
        {
            Debug.Log("x bigger");
            BGSize = (float)background.cellBounds.xMax / cam.aspect;
        } else // taller than wide
        {
            Debug.Log("y bigger");
            BGSize = (float)background.cellBounds.yMax;
        }

        cam.orthographicSize = (BGSize * (6f / 5f));
        cam.transform.position = new Vector3(background.cellBounds.center.x, background.cellBounds.center.y,
            cam.transform.position.z);
    }

}
