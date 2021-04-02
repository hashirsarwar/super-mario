using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool dead = false;
    private float playerSpeed = 2.5f;
    private float jumpSpeed = 5f;
    private int health;
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
    private bool harmless = false;
    private Color originalPlayerColor;
    private int colorSwap = 0;
    private bool firstFire2Coll = true;
    public Text gameOver;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        smallMarioSprite = renderer.sprite;
        originalPlayerColor = renderer.color;
        smallMarioAnimator = GetComponent<Animator>().runtimeAnimatorController;
        health = 50;
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
                if (transform.position.x - cam.transform.position.x < 3.8f)
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
            Vector2 pos;
            GameObject fire;
            if (transform.rotation.y == -1)
            {
                pos = new Vector2(transform.position.x + 0.3f, transform.position.y + 0.1f);
                fire = Instantiate(marioFire, pos, Quaternion.identity);
                fire.GetComponent<Rigidbody2D>().AddForce(new Vector2(2.6f, 0.6f) * 80);
            }
            else
            {
                pos = new Vector2(transform.position.x - 0.3f, transform.position.y + 0.1f);
                fire = Instantiate(marioFire, pos, Quaternion.identity);
                fire.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2.6f, 0.6f) * 80);
            }

            StartCoroutine(PlayFireAnimation());
            StartCoroutine(DestroyFire(fire));
        }
    }

    public int getPlayerHealth()
    {
        return health;
    }

    IEnumerator PlayFireAnimation()
    {
        animator.SetBool("moving", true);
        yield return new WaitForSeconds(4);
        animator.SetBool("moving", false);
    }

    IEnumerator DestroyFire(GameObject fire)
    {
        yield return new WaitForSeconds(1);
        Destroy(fire);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.tag == "Enemy" || col.gameObject.tag == "DeadEnemy") && this.gameObject.tag != "Invincible")
        {
            GameObject enemy = col.gameObject;
            if (!harmless)
            {
                // TODO: Keep constant for every enemy.
                if (transform.position.y > 0.62f)
                {
                    GetComponent<FloatingText>().display(enemy, "100");
                    // FloatingText.display(enemy, "100");
                    if (col.gameObject.tag == "Enemy")
                    {
                        enemy.GetComponent<Animator>().SetBool("isDead", true);
                        enemy.GetComponent<MoveObject>().stopMoving = true;
                        enemy.transform.position = new Vector2(enemy.transform.position.x, 0.23f);
                        if (enemy.name == "Enemy2")
                        {
                            enemy.transform.position = new Vector3(transform.position.x, 0.28f, 0);
                            enemy.tag = "Fire2";
                            cam.GetComponent<CameraFollow>().setMoveCam(false);
                            GetComponent<Rigidbody2D>().AddForce(new Vector2(150, 50), ForceMode2D.Impulse);
                        }
                        else
                        {
                            enemy.GetComponent<Collider2D>().enabled = false;
                            StartCoroutine(DestroyEnemyObject(enemy));
                        }
                    }

                    else
                    {
                        marioFire.GetComponent<KillFromFire>().InvertedFall(enemy);
                    }

                    GameController.AddScore(100);
                }
                else
                {
                    if (health == 50)
                    {
                        dead = true;
                        animator.SetBool("isDead", true);
                        transform.Translate(Vector3.up * jumpSpeed * 5 * Time.deltaTime, Space.World);
                        this.GetComponent<Collider2D>().enabled = false;
                        GameController.MinusLife();
                        if (GameController.lives == 0)
                        {
                            gameOver.text = "Game Over";
                        }

                        else
                        {
                            StartCoroutine(ResetGame());
                        }
                    }
                    else if (health == 100)
                    {
                        DecreasePlayerHealth();
                    }
                }
            }

            else
            {
                GetComponent<FloatingText>().display(enemy, "100");
                // FloatingText.display(enemy, "100");
                marioFire.GetComponent<KillFromFire>().InvertedFall(enemy);
                GameController.AddScore(100);
            }

        }

        else if (col.gameObject.tag == "RedMushroom")
        {
            Destroy(col.gameObject);
            if (health == 50)
            {
                IncreasePlayerHealth();
                GameController.AddScore(1000);
            }
        }

        else if (col.gameObject.tag == "Star")
        {
            Destroy(col.gameObject);
            GetComponent<Animator>().runtimeAnimatorController = fireEnabledMario;
            fireEnabled = true;
            GameController.AddScore(1000);
        }

        else if (col.gameObject.tag == "Flower")
        {
            Destroy(col.gameObject);
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            harmless = true;
            StartCoroutine(ColorBlink(renderer));
            StartCoroutine(HarmlessLimit(renderer));
            GameController.AddScore(1000);
        }

        else if (col.gameObject.tag == "Fire2")
        {
            if (firstFire2Coll)
            {
                firstFire2Coll = false;
                return;
            }
            else
            {
                GetComponent<FloatingText>().display(col.gameObject, "100");
                firstFire2Coll = true;
                col.gameObject.tag = "DeadEnemy";
                KillFromFire scriptKill = col.gameObject.AddComponent<KillFromFire>();
                FloatingText ftScript = col.gameObject.AddComponent<FloatingText>();
                ftScript.prefab = (GameObject)Resources.Load("FloatingText", typeof(GameObject));
                scriptKill.killOnlyOnce = false;
                MoveObject scriptMove = col.gameObject.GetComponent<MoveObject>();
                scriptMove.speed = 2.3f;
                scriptMove.isDirectionLeft = true;
                scriptMove.stopMoving = false;
                GameController.AddScore(1000);
            }
        }
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    IEnumerator DestroyEnemyObject(GameObject enemy)
    {
        yield return new WaitForSeconds(1);
        Destroy(enemy);
    }

    IEnumerator HarmlessLimit(SpriteRenderer renderer)
    {
        yield return new WaitForSeconds(8);
        harmless = false;
        renderer.color = originalPlayerColor;
    }

    IEnumerator ColorBlink(SpriteRenderer renderer)
    {
        while (harmless)
        {
            if (colorSwap % 3 == 0)
            {
                renderer.color = Color.red;
            }

            else if (colorSwap % 3 == 1)
            {
                renderer.color = Color.green;
            }

            else if (colorSwap % 3 == 2)
            {
                renderer.color = Color.blue;
            }

            ++colorSwap;

            yield return new WaitForSeconds(0.2f);
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
