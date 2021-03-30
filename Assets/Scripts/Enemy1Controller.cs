using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    private float enemySpeed = 2.3f;
    private bool movingLeft = false;
    private bool movingRight = true;
    public bool enemyKilled = false;

    void FixedUpdate()
    {
        if (!enemyKilled)
        {
            Move();
        }
    }

    void Move()
    {
        if (movingLeft)
        {
            transform.position += new Vector3(enemySpeed, 0, 0) * Time.deltaTime;
        }

        else if (movingRight)
        {
            transform.position += new Vector3(-enemySpeed, 0, 0) * Time.deltaTime;
        }
        
    }

    void OnCollisionEnter2D(Collision2D col)
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
