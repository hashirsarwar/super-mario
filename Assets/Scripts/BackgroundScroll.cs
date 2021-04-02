using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    private float xOffset = 0;
    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = transform.parent.gameObject.GetComponent<CameraFollow>();
    }
    void Update()
    {
        if (cameraFollow.isCamMoving())
        {
            xOffset -= 0.001f;
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xOffset, 0);
        }
    }
}
