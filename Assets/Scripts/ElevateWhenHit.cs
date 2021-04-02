using UnityEngine;

public class ElevateWhenHit : MonoBehaviour
{
    private Vector3 initPos;
    private Vector3 targetPos;
    private bool elevateBrick = false;
    private bool liftDown = false;

    void Start()
    {
        initPos = transform.position;
        targetPos = transform.position + new Vector3(0, 0.1f, 0);
    }

    void Update()
    {
        ElevateBrickWhenHit();
    }

    void ElevateBrickWhenHit()
    {
        if (elevateBrick)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
            if (transform.position.y >= targetPos.y)
            {
                liftDown = true;
                elevateBrick = false;
            }
        }

        if (liftDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, initPos, Time.deltaTime);
            if (transform.position.y <= initPos.y)
                liftDown = false;
        }
    }

    public void Elevate()
    {
        elevateBrick = true;
    }
}
