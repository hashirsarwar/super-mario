using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool dead = false;
    private float playerSpeed = 2.5f;
    private float jumpSpeed = 5f;
    private int health = 50;
    public GameObject cam;
    public Animator animator;
    public Sprite bigMarioSprite;
    public RuntimeAnimatorController bigMarioAnimator;
    private Sprite smallMarioSprite;
    private RuntimeAnimatorController smallMarioAnimator;
    private float blinkTime = 0.2f;
    private float blinkDuration = 0.038f;
    public RuntimeAnimatorController fireEnabledMario;
    private bool fireEnabled = false;
    public GameObject marioFire;

    void Start()
    {
        smallMarioSprite = GetComponent<SpriteRenderer>().sprite;
        smallMarioAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    void FixedUpdate()
    {
        if (this.gameObject.tag != "Invincible")
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
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime, Space.World);
        }
    }

    void Update()
    {
        if (fireEnabled && Input.GetKeyDown(KeyCode.Z))
        {
            GameObject fire = Instantiate(marioFire,
                                          new Vector2(transform.position.x - 0.3f,
                                          transform.position.y + 0.1f),
                                          Quaternion.identity);
            fire.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2.6f, 0.6f) * 80);
            StartCoroutine(PlayFireAnimation());
        }
    }

    IEnumerator PlayFireAnimation()
    {
        animator.SetBool("moving", true);
        yield return new WaitForSeconds(4);
        animator.SetBool("moving", false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && this.gameObject.tag != "Invincible")
        {
            // TODO: Keep constant for every enemy.
            if (transform.position.y > 0.63f)
            {
                GameObject enemy = col.gameObject;
                enemy.GetComponent<Collider2D>().enabled = false;
                enemy.GetComponent<Animator>().SetBool("isDead", true);
                enemy.GetComponent<MoveObject>().stopMoving = true;
                enemy.transform.position = new Vector2(enemy.transform.position.x, 0.23f);
            }
            else
            {
                if (health == 50)
                {
                    dead = true;
                    animator.SetBool("isDead", true);
                    transform.Translate(Vector3.up * jumpSpeed * 5 * Time.deltaTime, Space.World);
                    this.GetComponent<Collider2D>().enabled = false;
                }
                else if (health == 100)
                {
                    DecreasePlayerHealth();
                }
            }
        }

        else if (col.gameObject.tag == "RedMushroom")
        {
            Destroy(col.gameObject);
            if (health == 50)
            {
                IncreasePlayerHealth();
            }
        }

        else if (col.gameObject.tag == "Star")
        {
            Destroy(col.gameObject);
            GetComponent<Animator>().runtimeAnimatorController = fireEnabledMario;
            fireEnabled = true;
        }
    }

    void DecreasePlayerHealth()
    {
        health = 50;
        fireEnabled = false;
        GetComponent<SpriteRenderer>().sprite = smallMarioSprite;
        GetComponent<Animator>().runtimeAnimatorController = smallMarioAnimator;
        GetComponent<BoxCollider2D>().size -= new Vector2(0, 0.15f);
        StartCoroutine(MakeInvincible(blinkDuration, blinkTime));
    }

    void IncreasePlayerHealth()
    {
        health = 100;
        GetComponent<SpriteRenderer>().sprite = bigMarioSprite;
        GetComponent<Animator>().runtimeAnimatorController = bigMarioAnimator;
        GetComponent<BoxCollider2D>().size += new Vector2(0, 0.15f);
        StartCoroutine(MakeInvincible(blinkDuration, blinkTime));
    }

    IEnumerator MakeInvincible(float duration, float time)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        this.gameObject.tag = "Invincible";
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(time);
        }

        this.gameObject.tag = "Player";
        renderer.enabled = true;
    }
}
