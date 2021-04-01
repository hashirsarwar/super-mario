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
    private Vector3 targetPos;
    private bool elevateBrick = false;
    private bool liftDown = false;
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
    private AfterElevationAction afterElevationAction;
    private Vector3 targetStarPos;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        initPos = transform.position;
        targetPos = transform.position + new Vector3(0, 0.1f, 0);
        initCoinPos = targetPos + new Vector3(0, 0.15f, 0);
        targetCoinPos = initCoinPos + new Vector3(0, 0.4f, 0);
        targetMushroomPos = targetPos + new Vector3(0, 0.2f, 0);
        targetStarPos = targetPos + new Vector3(0, 0.23f, 0);
    }

    void Update()
    {
        ElevateBrickWhenHit();
        ElevateObjectWhenSpawned();
    }

    void ElevateObjectWhenSpawned()
    {
        if (elevateTargetCoin)
        {
            DoElevateObjectWhenSpawned(targetCoin,
                                       targetCoinPos,
                                       coinElevationSpeed,
                                       ref elevateTargetCoin,
                                       null,
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

    void MakeObjectMoveable(GameObject target)
    {
        target.GetComponent<MoveObject>().stopMoving = false;
        target.GetComponent<Rigidbody2D>().isKinematic = false;
        target.GetComponent<Collider2D>().enabled = true;
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

    void ElevateBrick()
    {
        elevateBrick = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && other.contacts[0].normal.y > 0.5f)
        {
            ElevateBrick();
            switch (brickObjects)
            {
                case BrickObjects.Coin:
                    GameObject coin = this.transform.GetChild(0).gameObject;
                    targetCoin = SpawnObject(coin, false, false, ref elevateTargetCoin);
                    break;
                case BrickObjects.Star:
                    GameObject star = this.transform.GetChild(1).gameObject;
                    targetStar = SpawnObject(star, true, true, ref elevateTargetStar);
                    break;
                case BrickObjects.Flower:
                    GameObject flower = this.transform.GetChild(2).gameObject;
                    break;
                case BrickObjects.RedMushroom:
                    GameObject mushroom = this.transform.GetChild(3).gameObject;
                    targetMushroom = SpawnObject(mushroom, true, true, ref elevateTargetMushroom);
                    break;
                default:
                    return;
            }
        }
    }

    GameObject SpawnObject(GameObject gameObject, bool hasPhysics, bool hasCollider, ref bool flag)
    {
        GameObject targetObj = Instantiate(gameObject, initPos, Quaternion.identity);
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
