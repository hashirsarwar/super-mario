using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 2.3f;
    private bool movingLeft = false;
    private float floorYPos = 0.35f; // TODO: Thid should be one variable used everywhere
    private bool movingRight = true;
    public bool stopMoving = false;

    void FixedUpdate()
    {
        if (!stopMoving)
        {
            Move();
        }
    }

    void Move()
    {
        if (movingLeft)
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }

        else if (movingRight)
        {
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        }
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (transform.position.y <= floorYPos)
        {
            if (movingRight)
            {
                movingLeft = true;
                movingRight = false;
            }

            else
            {
                movingLeft = false;
                movingRight = true;
            }
        }
    }
}
