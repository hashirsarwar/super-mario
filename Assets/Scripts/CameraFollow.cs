using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;
    private bool moveCam = true;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (moveCam)
        {
            transform.position = new Vector3(player.position.x + offset.x,
                                             transform.position.y,
                                             transform.position.z);
        }
    }

    public void setMoveCam(bool m)
    {
        moveCam = m;
    }

    public bool isCamMoving()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            return true;
        }
        return false;
    }
}
