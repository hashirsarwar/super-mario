using UnityEngine;

public class BrickObjectHandler : MonoBehaviour
{
    public enum BrickObjects
    {
        Coin,
        RedMushroom,
        Star,
        Flower,
    }
    public BrickObjects brickObjects;
    public int numOfCoins = 1;
    private Vector3 initPos;
    private GameObject targetCoin;
    private bool elevateTargetCoin = false;
    private Vector3 initCoinPos;
    private float coinElevationSpeed = 2;
    private Vector3 targetCoinPos;
    private GameObject targetMushroom;
    private bool elevateTargetMushroom = false;
    private Vector3 targetMushroomPos;
    private float mushroomElevationSpeed = 0.2f;
    private bool elevateTargetStar = false;
    private GameObject targetStar;
    private delegate void AfterElevationAction(GameObject target);
    private Vector3 targetStarPos;
    private GameObject targetFlower;
    private bool elevateTargetFlower = false;
    public GameObject player;
    private bool isEnabled = true;
    public Sprite brickSolidSprite;
    public Sprite brickBreakableSprite;
    public bool isHidden = false;
    private AudioSource audioSource;
    public AudioClip coinSound;
    public AudioClip elevationSound;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (isHidden)
        {
            GetComponent<SpriteRenderer>().sprite = brickBreakableSprite;
        }

        initPos = transform.position;
        initCoinPos = transform.position + new Vector3(0, 0.25f, 0);
        targetCoinPos = initCoinPos + new Vector3(0, 0.4f, 0);
        targetMushroomPos = transform.position + new Vector3(0, 0.3f, 0);
        targetStarPos = transform.position + new Vector3(0, 0.33f, 0);
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        ElevateObjectWhenSpawned();
        if (!isEnabled)
        {
            GetComponent<SpriteRenderer>().sprite = brickSolidSprite;
        }
    }

    void ElevateObjectWhenSpawned()
    {
        if (elevateTargetCoin)
        {
            DoElevateObjectWhenSpawned(targetCoin,
                                       targetCoinPos,
                                       coinElevationSpeed,
                                       ref elevateTargetCoin,
                                       "Coin",
                                       AfterCoinElevation);
        }

        else if (elevateTargetMushroom)
        {
            DoElevateObjectWhenSpawned(targetMushroom,
                                       targetMushroomPos,
                                       mushroomElevationSpeed,
                                       ref elevateTargetMushroom,
                                       "RedMushroom",
                                       AfterMushroomElevation);
        }

        else if (elevateTargetStar)
        {
            DoElevateObjectWhenSpawned(targetStar,
                                       targetStarPos,
                                       mushroomElevationSpeed,
                                       ref elevateTargetStar,
                                       "Star",
                                       AfterStarElevation);
        }

        else if (elevateTargetFlower)
        {
            DoElevateObjectWhenSpawned(targetFlower,
                                       targetStarPos,
                                       mushroomElevationSpeed,
                                       ref elevateTargetFlower,
                                       "Flower",
                                       AfterFlowerElevation);
        }
    }

    void DoElevateObjectWhenSpawned(GameObject target,
                                    Vector3 targetPos,
                                    float speed,
                                    ref bool flag,
                                    string tag,
                                    AfterElevationAction action)
    {
        target.transform.position = Vector3.MoveTowards(target.transform.position, targetPos, speed * Time.deltaTime);
        if (target.transform.position.y >= targetPos.y)
        {
            flag = false;
            target.tag = tag;
            action(target);
        }
    }

    void AfterMushroomElevation(GameObject target)
    {
        MakeObjectMoveable(target);
    }

    void AfterCoinElevation(GameObject target)
    {
        Destroy(target);
    }

    void AfterStarElevation(GameObject target)
    {
        MakeObjectMoveable(target);
    }

    void AfterFlowerElevation(GameObject target)
    {
        target.GetComponent<Collider2D>().enabled = true;
    }

    void MakeObjectMoveable(GameObject target)
    {
        target.GetComponent<MoveObject>().stopMoving = false;
        target.GetComponent<Rigidbody2D>().isKinematic = false;
        target.GetComponent<Collider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && other.contacts[0].normal.y > 0.5f && isEnabled)
        {
            GetComponent<ElevateWhenHit>().Elevate();
            if (brickObjects == BrickObjects.Flower || brickObjects == BrickObjects.Star)
            {
                int health = player.GetComponent<PlayerController>().getPlayerHealth();
                if (health == 50 || health == 0)
                {
                    brickObjects = BrickObjects.RedMushroom;
                }
            }

            switch (brickObjects)
            {
                case BrickObjects.Coin:
                    audioSource.PlayOneShot(coinSound);
                    GameObject coin = this.transform.GetChild(0).gameObject;
                    targetCoin = SpawnObject(coin, false, false, ref elevateTargetCoin);
                    --numOfCoins;
                    if (numOfCoins == 0)
                        isEnabled = false;
                    break;
                case BrickObjects.Star:
                    audioSource.PlayOneShot(elevationSound);
                    GameObject star = this.transform.GetChild(1).gameObject;
                    targetStar = SpawnObject(star, true, true, ref elevateTargetStar);
                    isEnabled = false;
                    break;
                case BrickObjects.Flower:
                    audioSource.PlayOneShot(elevationSound);
                    GameObject flower = this.transform.GetChild(2).gameObject;
                    targetFlower = SpawnObject(flower, false, true, ref elevateTargetFlower);
                    isEnabled = false;
                    break;
                case BrickObjects.RedMushroom:
                    audioSource.PlayOneShot(elevationSound);
                    GameObject mushroom = this.transform.GetChild(3).gameObject;
                    targetMushroom = SpawnObject(mushroom, true, true, ref elevateTargetMushroom);
                    isEnabled = false;
                    break;
                default:
                    return;
            }
        }
    }

    GameObject SpawnObject(GameObject gameObject, bool hasPhysics, bool hasCollider, ref bool flag)
    {
        GameObject targetObj = Instantiate(gameObject, initPos, Quaternion.identity);
        targetObj.transform.localScale = new Vector3(2, 2, 1);
        targetObj.SetActive(true);

        if (hasPhysics)
        {
            // Set script variables if required
            if (targetObj.tag == "RedMushroom" || targetObj.tag == "Star")
            {
                targetObj.GetComponent<MoveObject>().stopMoving = true;
            }

            targetObj.tag = "Untagged";
            targetObj.GetComponent<Rigidbody2D>().isKinematic = true;
        }

        if (hasCollider)
        {
            targetObj.GetComponent<Collider2D>().enabled = false;
        }

        flag = true;
        return targetObj;
    }
}
