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
        Debug.Log(cam.aspect);
        float BGWide = (background.localBounds.extents.x / cam.aspect) * (1f + 0.5f / cam.aspect);
        Debug.Log(BGWide);
        float BGTall = background.localBounds.extents.y * 1.5f;
        Debug.Log(BGTall);
        if (BGWide > BGTall) // wider than tall
        {
            cam.orthographicSize = BGWide;
        } else // taller than wide
        {
            cam.orthographicSize = BGTall;
        }

        cam.transform.position = new Vector3(background.localBounds.center.x, background.localBounds.center.y,
            cam.transform.position.z);
    }

}
