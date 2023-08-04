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

    // sizes and centers camera to guarantee a sizable border
    // TODO: instead, base resize on UI components
    private void resizeCamera()
    {
        // first, determine if width or height is limiting; or, proportionally larger when aspect normalised
        float BGSize;
        if (background.localBounds.extents.x / cam.aspect > background.localBounds.extents.y) // wider than tall
        {
            BGSize = (background.localBounds.extents.x / cam.aspect) * 1.2f;
        } else // taller than wide
        {
            BGSize = background.localBounds.extents.y * (1 + 0.2f * cam.aspect);
        }

        cam.orthographicSize = BGSize;
        cam.transform.position = new Vector3(background.localBounds.center.x, background.localBounds.center.y,
            cam.transform.position.z);
    }

}
