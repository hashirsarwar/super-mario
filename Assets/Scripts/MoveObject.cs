using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 2.3f;
    private bool movingLeft = false;
    private float threshold = 0.4f; // TODO: Thid should be one variable used everywhere
    private bool movingRight = true;
    public bool stopMoving = false;
    public bool isDirectionLeft = true;

    void Start()
    {
        if (!isDirectionLeft)
        {
            movingLeft = true;
            movingRight = false;
        }
    }

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
            if (transform.rotation.y != 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else if (movingRight)
        {
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
            if (transform.rotation.y != -1)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (gameObject.tag == "DeadEnemy" && col.gameObject.tag == "Enemy")
            return;

        if (col.gameObject.tag != "Floor")
        {
            if (transform.position.y <= threshold)
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
}
