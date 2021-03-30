using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool dead = false;
    private float playerSpeed = 2.5f;
    private float jumpSpeed = 5f;
    public GameObject cam;
    public Animator animator;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !dead)
        {
            if (transform.position.x <= cam.transform.position.x)
            {
                cam.GetComponent<CameraFollow>().setMoveCam(true);
            }
            transform.position += new Vector3(-playerSpeed, 0, 0) * Time.deltaTime;
            if (transform.rotation.y == -1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            animator.SetBool("moving", true);
        }

        else if (Input.GetKey(KeyCode.RightArrow) && !dead)
        {
            cam.GetComponent<CameraFollow>().setMoveCam(false);
            if (transform.position.x - cam.transform.position.x < 2.7f)
            {
                transform.position += new Vector3(playerSpeed, 0, 0) * Time.deltaTime;
                if (transform.rotation.y == 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                animator.SetBool("moving", true);
            }
        }

        else
        {
            animator.SetBool("moving", false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * jumpSpeed* Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            dead = true;
            animator.SetBool("isDead", true);
            transform.Translate(Vector3.up * jumpSpeed * 2 * Time.deltaTime, Space.World);
            this.GetComponent<Collider2D>().enabled = false;
        }
    }

    // void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.gameObject.tag == "Enemy")
    //     {
    //         killed = true;
    //         GameObject enemy = col.gameObject;
    //         enemy.GetComponent<Collider2D>().enabled = false;
    //         enemy.GetComponent<Animator>().SetBool("isDead", true);
    //         enemy.GetComponent<Enemy1Controller>().enemyKilled = true;
    //         enemy.transform.position = new Vector2(enemy.transform.position.x, 0.23f);
    //     }
    // }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            // TODO: Keep constant for every enemy.
            if (transform.position.y > 0.63f)
            {
                GameObject enemy = col.gameObject;
                enemy.GetComponent<Collider2D>().enabled = false;
                enemy.GetComponent<Animator>().SetBool("isDead", true);
                enemy.GetComponent<Enemy1Controller>().enemyKilled = true;
                enemy.transform.position = new Vector2(enemy.transform.position.x, 0.23f);
            }
            else
            {
                dead = true;
                animator.SetBool("isDead", true);
                transform.Translate(Vector3.up * jumpSpeed * 5 * Time.deltaTime, Space.World);
                this.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
